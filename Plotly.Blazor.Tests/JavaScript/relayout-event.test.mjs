import assert from "node:assert/strict";
import { readFile } from "node:fs/promises";
import { test } from "node:test";

const interopSource = await readFile(
  new URL("../../Plotly.Blazor/wwwroot/plotly-interop-7.2.0.js", import.meta.url),
  "utf8",
);
const interopModule = await import(
  `data:text/javascript;base64,${Buffer.from(interopSource).toString("base64")}`,
);

test("relayout event keeps zero-valued split range endpoints", async () => {
  const previousDocument = globalThis.document;
  const previousWindow = globalThis.window;
  const handlers = new Map();
  const plot = {
    on: (eventName, callback) => handlers.set(eventName, callback),
  };

  globalThis.document = {
    getElementById: (id) => (id === "plotly-import" ? null : plot),
    createElement: () => ({ dataset: {} }),
    head: {
      appendChild: (script) => script.onload(),
    },
  };
  globalThis.window = { define: undefined };

  try {
    await interopModule.importScript("plotly-import", "plotly.js");

    let received;
    interopModule.subscribeRelayoutEvent(
      { invokeMethodAsync: (_methodName, payload) => (received = payload) },
      "chart",
    );
    handlers.get("plotly_relayout")({
      "xaxis.range[0]": 0,
      "xaxis.range[1]": 10,
    });

    assert.deepEqual(received.XRange, [0, 10]);
  } finally {
    globalThis.document = previousDocument;
    globalThis.window = previousWindow;
  }
});
