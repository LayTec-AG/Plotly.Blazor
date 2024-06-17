const scriptCache = new Map();

export async function importScript(id, scriptUrl) {
    return new Promise((resolve, reject) => {
        // Check if already exists
        var existingElement = document.getElementById(id);
        if (existingElement) {
            // Double check, if the src has changed
            if (existingElement.dataset.originalSrc === scriptUrl) {
                resolve();
                return;
            } else {
                // Remove if different src
                existingElement.remove();
            }
        }

        // Load script dynamically by adding it to the head
        const script = document.createElement('script');
        script.id = id;
        script.src = scriptUrl;
        script.dataset.originalSrc = scriptUrl;
        script.type = 'text/javascript';
        script.async = true;
        script.onload = () => {
            scriptCache.set(id, scriptUrl);
            resolve();
        };
        script.onerror = (error) => reject(new Error(`Failed to load script ${scriptUrl}: ${error.message}`));
        document.head.appendChild(script);
    });
}

export function newPlot(id, data = [], layout = {}, config = {}, frames = []) {
    window.Plotly.newPlot(id, data, layout, config, frames);
}
export function react(id, data = [], layout = {}, config = {}, frames = []) {
    window.Plotly.react(id, data, layout, config, frames);
}
export function extendTraces(id, x, y, indizes, max) {
    var data = {};
    if (x != null) {
        data["x"] = x;
    }
    if (y != null) {
        data["y"] = y;
    }
    if (max != null) {
        window.Plotly.extendTraces(id, data, indizes, max);
    } else {
        window.Plotly.extendTraces(id, data, indizes);
    }
}

export function extendTraces3D(id, x, y, z, indizes, max) {
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
        window.Plotly.extendTraces(id, data, indizes, max);
    } else {
        window.Plotly.extendTraces(id, data, indizes);
    }
}
export function prependTraces(id, x = null, y = null, indizes = [0], max) {
    var data = {};
    if (x != null) {
        data["x"] = x;
    }
    if (y != null) {
        data["y"] = y;
    }
    if (max != null) {
        window.Plotly.prependTraces(id, data, indizes, max);
    }
    else {
        window.Plotly.prependTraces(id, data, indizes);
    }
}

export function prependTraces3D(id, x = null, y = null, z = null, indizes = [0], max) {
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
        window.Plotly.prependTraces(id, data, indizes, max);
    }
    else {
        window.Plotly.prependTraces(id, data, indizes);
    }
}
export function addTrace(id, data = {}, index = null) {
    if (index != null) {
        window.Plotly.addTraces(id, [data], [index]);
    } else {
        window.Plotly.addTraces(id, [data]);
    }
}
export function deleteTrace(id, index) {
    window.Plotly.deleteTraces(id, index);
}
export function purge(id) {
    window.Plotly.purge(id);
}
export function relayout(id, layout = {}) {
    window.Plotly.relayout(id, layout);
}
export function restyle(id, data, indizes) {
    window.Plotly.restyle(id, data, indizes);
}
export function toImage(id, format, height, width) {
    return window.Plotly.toImage(id, { format: format, height: height, width: width });
}
export function downloadImage(id, format, height, width, filename) {
    return window.Plotly.downloadImage(id, { format: format, height: height, width: width, filename: filename });
}
export function subscribeLegendClickEvent(dotNetObj, id) {
    var plot = document.getElementById(id);
    plot.on('plotly_legendclick', function (data) {
        dotNetObj.invokeMethodAsync('LegendClickEvent', {
            CurveNumber: data.curveNumber,
            ExpandedIndex: data.expandedIndex
        })
    })
}
export function subscribeClickEvent(dotNetObj, id) {
    var plot = document.getElementById(id);
    plot.on('plotly_click', function (data) {
        dotNetObj.invokeMethodAsync('ClickEvent',
            data.points.map(function (d) {
                if (d.x != null) {
                    return ({
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
                    });
                }
                else {
                    return ({
                        TraceIndex: d.fullData.index,
                        PointIndex: d.pointIndex,
                        PointNumber: d.pointNumber,
                        CurveNumber: d.curveNumber,
                        Text: d.text,
                        X: d.value,
                        Y: d.label,
                        Z: null,
                        Lat: d.lat,
                        Lon: d.lon
                    });
                }
            }));
    })
}
export function subscribeHoverEvent(dotNetObj, id) {
    var plot = document.getElementById(id);
    plot.on('plotly_hover', function (data) {
        dotNetObj.invokeMethodAsync('HoverEvent',
            data.points.map(function (d) {
                if (d.x != null) {
                    return ({
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
                    });
                }
                else {
                    return ({
                        TraceIndex: d.fullData.index,
                        PointIndex: d.pointIndex,
                        PointNumber: d.pointNumber,
                        CurveNumber: d.curveNumber,
                        Text: d.text,
                        X: d.value,
                        Y: d.label,
                        Z: null,
                        Lat: d.lat,
                        Lon: d.lon
                    });
                }
            }));
    })
}
export function subscribeRelayoutEvent(dotNetObj, id) {
    var plot = document.getElementById(id);
    plot.on('plotly_relayout', function (data) {

        var x1 = data["xaxis.range[0]"];
        var x2 = data["xaxis.range[1]"];

        var y1 = data["yaxis.range[0]"]
        var y2 = data["yaxis.range[1]"]

        var z1 = data["zaxis.range[0]"]
        var z2 = data["zaxis.range[1]"]

        var result = {};

        if (x1 && x2) {
            result.XRange = [x1, x2];
        }

        if (y1 && y2) {
            result.YRange = [y1, y2];
        }

        if (z1 && z2) {
            result.ZRange = [z1, z2];
        }

        dotNetObj.invokeMethodAsync('RelayoutEvent', result);
    });
}