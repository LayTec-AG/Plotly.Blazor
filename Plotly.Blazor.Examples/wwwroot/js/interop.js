window.onBlazorReady = () => {
    document.querySelectorAll("#code").forEach((block) => {
        hljs.highlightBlock(block);
    });
}