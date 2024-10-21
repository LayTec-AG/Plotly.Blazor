const scriptCache = new Map();
let plotlyReady = false;
const plotlyReadyCallbacks = [];

export async function importScript(id, scriptUrl) {
    return new Promise((resolve, reject) => {
        var existingElement = document.getElementById(id);
        if (existingElement) {
            if (existingElement.dataset.originalSrc === scriptUrl) {
                resolve();
                return;
            } else {
                existingElement.remove();
            }
        }

        const script = document.createElement('script');
        script.id = id;
        script.src = scriptUrl;
        script.dataset.originalSrc = scriptUrl;
        script.type = 'text/javascript';
        script.async = true;
        script.onload = () => {
            scriptCache.set(id, scriptUrl);
            plotlyReady = true;
            plotlyReadyCallbacks.forEach(callback => callback());
            resolve();
        };
        script.onerror = (error) => reject(new Error(`Failed to load script ${scriptUrl}: ${error.message}`));
        document.head.appendChild(script);
    });
}

function onPlotlyReady(divId, callback) {
    //check the plot has not been derendered before attempting to do anything
    if (divId != null && document.getElementById(divId) == null) {
        return;
    }
    else if (plotlyReady) {
        callback();
    } else {
        plotlyReadyCallbacks.push(callback);
    }
}

export function newPlot(id, data = [], layout = {}, config = {}, frames = []) {
    onPlotlyReady(id, () => {
        window.Plotly.newPlot(id, data, layout, config, frames);
    });
}

export function react(id, data = [], layout = {}, config = {}, frames = []) {
    onPlotlyReady(id, () => {
        window.Plotly.react(id, data, layout, config, frames);
    });
}

export function extendTraces(id, x, y, indices, max) {
    onPlotlyReady(id, () => {
        var data = {};
        if (x != null) {
            data["x"] = x;
        }
        if (y != null) {
            data["y"] = y;
        }
        if (max != null) {
            window.Plotly.extendTraces(id, data, indices, max);
        } else {
            window.Plotly.extendTraces(id, data, indices);
        }
    });
}

export function extendTraces3D(id, x, y, z, indices, max) {
    onPlotlyReady(id, () => {
        var data = {};
        if (x != null) {
            data["x"] = x;
        }
        if (y != null) {
            data["y"] = y;
        }
        if (z != null) {
            data["z"] = z;
        }

        if (max != null) {
            window.Plotly.extendTraces(id, data, indices, max);
        } else {
            window.Plotly.extendTraces(id, data, indices);
        }
    });
}

export function prependTraces(id, x = null, y = null, indices = [0], max) {
    onPlotlyReady(id, () => {
        var data = {};
        if (x != null) {
            data["x"] = x;
        }
        if (y != null) {
            data["y"] = y;
        }
        if (max != null) {
            window.Plotly.prependTraces(id, data, indices, max);
        }
        else {
            window.Plotly.prependTraces(id, data, indices);
        }
    });
}

export function prependTraces3D(id, x = null, y = null, z = null, indices = [0], max) {
    onPlotlyReady(id, () => {
        var data = {};
        if (x != null) {
            data["x"] = x;
        }
        if (y != null) {
            data["y"] = y;
        }
        if (z != null) {
            data["z"] = z;
        }

        if (max != null) {
            window.Plotly.prependTraces(id, data, indices, max);
        }
        else {
            window.Plotly.prependTraces(id, data, indices);
        }
    });
}

export function addTrace(id, data = {}, index = null) {
    onPlotlyReady(id, () => {
        if (index != null) {
            window.Plotly.addTraces(id, [data], [index]);
        } else {
            window.Plotly.addTraces(id, [data]);
        }
    });
}

export function deleteTrace(id, index) {
    onPlotlyReady(id, () => {
        window.Plotly.deleteTraces(id, index);
    });
}

export function purge(id) {
    onPlotlyReady(id, () => {
        window.Plotly.purge(id);
    });
}

export function relayout(id, layout = {}) {
    onPlotlyReady(id, () => {
        window.Plotly.relayout(id, layout);
    });
}

export function restyle(id, data, indices) {
    onPlotlyReady(id, () => {
        window.Plotly.restyle(id, data, indices);
    });
}

export function toImage(id, format, height, width) {
    return new Promise((resolve) => {
        onPlotlyReady(id, () => {
            resolve(window.Plotly.toImage(id, { format: format, height: height, width: width }));
        });
    });
}

export function toImageFromChartData(chartData, format, height, width) {
    return new Promise((resolve) => {
        onPlotlyReady(null, () => {
            resolve(window.Plotly.toImage(chartData, { format: format, height: height, width: width }));
        });
    });
}

export function downloadImage(id, format, height, width, filename) {
    return new Promise((resolve) => {
        onPlotlyReady(id, () => {
            resolve(window.Plotly.downloadImage(id, { format: format, height: height, width: width, filename: filename }));
        });
    });
}

export function subscribeLegendClickEvent(dotNetObj, id) {
    onPlotlyReady(id, () => {
        var plot = document.getElementById(id);
        plot.on('plotly_legendclick', function (data) {
            dotNetObj.invokeMethodAsync('LegendClickEvent', {
                CurveNumber: data.curveNumber,
                ExpandedIndex: data.expandedIndex
            });
        });
    });
}

export function subscribeSelectedEvent(dotNetObj, id) {
    onPlotlyReady(id, () => {
        var plot = document.getElementById(id);
        plot.on('plotly_selected', function (data) {
            dotNetObj.invokeMethodAsync('SelectedEvent',
                data.points.map(function (d) {
                    return {
                        TraceIndex: d.fullData.index,
                        PointIndex: d.pointIndex,
                        PointNumber: d.pointNumber,
                        CurveNumber: d.curveNumber,
                        Text: d.text,
                        X: d.x,
                        Y: d.y,
                        Z: d.z,
                        Lat: d.lat,
                        Lon: d.lon
                    };
                }));
        });
    });
}

export function subscribeClickEvent(dotNetObj, id) {
    onPlotlyReady(id, () => {
        var plot = document.getElementById(id);
        plot.on('plotly_click', function (data) {
            dotNetObj.invokeMethodAsync('ClickEvent',
                data.points.map(function (d) {
                    return {
                        TraceIndex: d.fullData.index,
                        PointIndex: d.pointIndex,
                        PointNumber: d.pointNumber,
                        CurveNumber: d.curveNumber,
                        Text: d.text,
                        X: d.x,
                        Y: d.y,
                        Z: d.z,
                        Lat: d.lat,
                        Lon: d.lon
                    };
                }));
        });
    });
}

export function subscribeHoverEvent(dotNetObj, id) {
    onPlotlyReady(id, () => {
        var plot = document.getElementById(id);
        plot.on('plotly_hover', function (data) {
            dotNetObj.invokeMethodAsync('HoverEvent',
                data.points.map(function (d) {
                    return {
                        TraceIndex: d.fullData.index,
                        PointIndex: d.pointIndex,
                        PointNumber: d.pointNumber,
                        CurveNumber: d.curveNumber,
                        Text: d.text,
                        X: d.x,
                        Y: d.y,
                        Z: d.z,
                        Lat: d.lat,
                        Lon: d.lon
                    };
                }));
        });
    });
}

export function subscribeRelayoutEvent(dotNetObj, id) {
    onPlotlyReady(id, () => {
        var plot = document.getElementById(id);

        plot.on('plotly_relayout', function (eventdata) {
            var x = eventdata["xaxis.range"];
            var x1 = eventdata["xaxis.range[0]"];
            var x2 = eventdata["xaxis.range[1]"];

            var y = eventdata["yaxis.range"];
            var y1 = eventdata["yaxis.range[0]"];
            var y2 = eventdata["yaxis.range[1]"];

            var z = eventdata["zaxis.range"];
            var z1 = eventdata["zaxis.range[0]"];
            var z2 = eventdata["zaxis.range[1]"];

            var result = {};

            if (x) {
                result.XRange = x;
            } else if (x1 && x2) {
                result.XRange = [x1, x2];
            }

            if (y) {
                result.YRange = y;
            } else if (y1 && y2) {
                result.YRange = [y1, y2];
            }

            if (z) {
                result.ZRange = z;
            } else if (z1 && z2) {
                result.ZRange = [z1, z2];
            }

            result.RawData = eventdata;

            dotNetObj.invokeMethodAsync('RelayoutEvent', result);
        });
    });
}

export function subscribeRestyleEvent(dotNetObj, id) {
    onPlotlyReady(id, () => {
        var plot = document.getElementById(id);
        plot.on('plotly_restyle', function (eventdata) {
            var result = {};
            result.Changes = eventdata[0];
            result.Indices = eventdata[1];

            dotNetObj.invokeMethodAsync('RestyleEvent', result);
        });
    });
}
