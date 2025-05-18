import { config } from "@/utils/config";

import ky from "ky";

const { baseUrl } = config.directus;

const directus = ky.extend({ prefixUrl: baseUrl });

type DirectusData<T> = {
  data: T;
};

const getDirectus = async <T>(collectionUrlSuffix: string) =>
  await directus
    .get(`items/${collectionUrlSuffix}`)
    .json<DirectusData<T>>()
    .then((x) => x.data);

export const createDirectusCollection = <T>(collectionNameKey: string) => {
  const collectionName = config.directus[`${collectionNameKey}CollectionName`];

  return {
    get: async (id: number) => await getDirectus<T>(`${collectionName}/${id}`),
    getAll: async () => await getDirectus<T[]>(collectionName),
  };
};
