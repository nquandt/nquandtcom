import { createDirectusCollection } from "@/utils/directus";

export type Experience = {
  id: number;
  title: string;
  company: string;
  start: string;
  end?: string;
  location: string;
  details: string;
  overview?: string;
};

export const { get: getExperience, getAll: getExperiences } =
  createDirectusCollection<Experience>("experience");
