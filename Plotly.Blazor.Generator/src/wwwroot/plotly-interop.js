window.plotlyInterop = {
    newPlot: function (id, data = [], layout = {}, config = {}) {
        window.Plotly.newPlot(id, data, layout, config);
    },
    react: function (id, data = [], layout = {}, config = {}) {
        window.Plotly.react(id, data, layout, config);
    }
}