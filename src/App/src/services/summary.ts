import { createDirectusCollection } from "@/utils/directus";

export type Summary = {
  id: number;
  title: string;
  statement: string;
};

export const { get: getSummary } = createDirectusCollection<Summary>("summary");
