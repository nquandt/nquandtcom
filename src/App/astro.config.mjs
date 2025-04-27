// @ts-check
import { defineConfig } from "astro/config";
import UnoCSS from "unocss/astro";

import node from "@astrojs/node";

import react from "@astrojs/react";

export default defineConfig({
  integrations: [UnoCSS({
    injectReset: "@unocss/reset/tailwind-compat.css",
  }), react()],
  output: "server",
  adapter: node({
    mode: "standalone",
  }),
});