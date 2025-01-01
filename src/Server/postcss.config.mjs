import UnoCSS from '@unocss/postcss'
import postimport from "postcss-import";
import cssnano from "cssnano";

export default {
  plugins: [
    UnoCSS(),
    postimport(),    
    cssnano()
  ],
}