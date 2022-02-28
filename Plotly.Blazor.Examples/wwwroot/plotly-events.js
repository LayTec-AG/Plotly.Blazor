window.plotlyExampleEvents = {
    plotlyClickXYEventArgsCreator: function (event) {
        return {
            x: event.detail.points[0].x,
            y: event.detail.points[0].y
        };
    },
    plotlyPIEEventArgsCreator: function (event) {
        return {
            label: event.detail.points[0].label,
            value: event.detail.points[0].value
        };
    },
    plotlyNoDataEventArgsCreator: function (event) {
        // No data associated with event
        return { };
    }
}

// Register custom event names and handlers
Blazor.registerCustomEventType('plotly_click_pie', {
    browserEventName: 'plotly_click',
    createEventArgs: plotlyExampleEvents.plotlyPIEEventArgsCreator
});
Blazor.registerCustomEventType('plotly_hover_pie', {
    browserEventName: 'plotly_hover',
    createEventArgs: plotlyExampleEvents.plotlyPIEEventArgsCreator
});
Blazor.registerCustomEventType('plotly_click_xy', {
    browserEventName: 'plotly_click',
    createEventArgs: plotlyExampleEvents.plotlyClickXYEventArgsCreator
});
Blazor.registerCustomEventType('plotly_doubleclick', {
    browserEventName: 'plotly_doubleclick',
    createEventArgs: plotlyExampleEvents.plotlyNoDataEventArgsCreator
});