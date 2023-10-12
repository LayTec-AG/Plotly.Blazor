window.plotlyInterop = {
    newPlot: function (id, data = [], layout = {}, config = {}, frames = []) {
        window.Plotly.newPlot(id, data, layout, config, frames);
    },
    react: function (id, data = [], layout = {}, config = {}, frames = []) {
        window.Plotly.react(id, data, layout, config, frames);
    },
    extendTraces: function (id, x, y, indizes, max) {
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
    },
    prependTraces: function (id, x = null, y = null, indizes = [0], max) {
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
    },
    addTrace: function (id, data = {}, index = null) {
        if (index != null) {
            window.Plotly.addTraces(id, [data], [index]);
        } else {
            window.Plotly.addTraces(id, [data]);
        }
    },
    deleteTrace: function (id, index) {
        window.Plotly.deleteTraces(id, index);
    },
    purge: function (id) {
        window.Plotly.purge(id);
    },
    relayout: function (id, layout = {}) {
        window.Plotly.relayout(id, layout);
    },
    restyle: function (id, data, indizes) {
        window.Plotly.restyle(id, data, indizes);
    },
    toImage: function (id, format, height, width) {
        return window.Plotly.toImage(id, { format: format, height: height, width: width });
    },
    downloadImage: function (id, format, height, width, filename) {
        return window.Plotly.downloadImage(id, { format: format, height: height, width: width, filename: filename });
    },
    subscribeClickEvent: function (dotNetObj, id) {
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
    },
    subscribeHoverEvent: function (dotNetObj, id) {
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
    },
    subscribeRelayoutEvent: function (dotNetObj, id) {
        var plot = document.getElementById(id);
        plot.on('plotly_relayout', function (data) {

            var x1 = data["xaxis.range[0]"];
            var x2 = data["xaxis.range[1]"];

            var y1 = data["yaxis.range[0]"]
            var y2 = data["yaxis.range[1]"]

            var result = {};

            if (x1 && x2)
            {
                result.XRange = [x1, x2];
            } 

            if (y1 && y2)
            {
                result.YRange = [y1, y2];
            }
            dotNetObj.invokeMethodAsync('RelayoutEvent', result);
        });
    }
}