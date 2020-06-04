window.plotlyInterop = {
    newPlot: function (id, data = [], layout = {}, config = {}) {
        window.Plotly.newPlot(id, data, layout, config);
    },
    react: function (id, data = [], layout = {}, config = {}) {
        window.Plotly.react(id, data, layout, config);
    },
    extendTraces: function (id, x, y, indizes) {
        var data = {};
        if (x != null) {
            data["x"] = x;
        }
        if (y != null) {
            data["y"] = y;
        }
        window.Plotly.extendTraces(id, data, indizes);
    },
    prependTraces: function (id, x = null, y = null, indizes = [0]) {
        var data = {};
        if (x != null) {
            data["x"] = x;
        }
        if (y != null) {
            data["y"] = y;
        }
        window.Plotly.prependTraces(id, data, indizes);
    }
}