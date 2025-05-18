import { createDirectusCollection } from "@/utils/directus";

type About = {
  id: number;
  overview: string;
};

export const { get: getAbout } = createDirectusCollection<About>("about");
