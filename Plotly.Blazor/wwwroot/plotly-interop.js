﻿window.plotlyInterop = {
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
    relayout: function (id, layout = {} ) {
        window.Plotly.relayout(id, layout);
    },
    restyle: function (id, data, indizes) {
        window.Plotly.restyle(id, data, indizes);
    },
    registerEmitEvents: function (id, eventNames) {
        const plot = document.getElementById(id);
        if (!plot) {
            console.error(`Missing plot ${id}`);
            return;
        }

        // Bubble up all event names for Blazor Event Handlers
        for (const eventName of eventNames) {
            plot.on(eventName, function (data = undefined) {
                const event = new CustomEvent(eventName, {detail: data, bubbles: true});
                plot.dispatchEvent(event);
            });
        }
    }
}