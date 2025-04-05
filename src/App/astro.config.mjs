// @ts-check
import { defineConfig } from "astro/config";
import UnoCSS from "unocss/astro";

import node from "@astrojs/node";

export default defineConfig({
  integrations: [
    UnoCSS({
      injectReset: "@unocss/reset/tailwind-compat.css",
    }),
  ],
  output: "server",
  adapter: node({
    mode: "standalone",
  }),
});
