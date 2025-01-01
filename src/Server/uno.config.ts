import {
  defineConfig,
  presetUno,
  presetIcons,
  presetWebFonts,
  transformerDirectives,
} from "unocss";

export default defineConfig({
  content: {
    filesystem: ["./**/*.cshtml"],
  },
  transformers: [transformerDirectives()],
  presets: [
    presetUno(),
    presetIcons(),
    presetWebFonts({
      provider: "google",
      fonts: {
        sans: "Roboto",
        mono: ["Fira Code", "Fira Mono:400,700"],
        lato: [
          {
            name: "Lato",
            weights: ["400", "700"],
            italic: true,
          },
          {
            name: "sans-serif",
            provider: "none",
          },
        ],
      },
    }),
  ],
  shortcuts: {
    "list-list": "list-disc list-inside -indent-12 pl-12",
  },
});
