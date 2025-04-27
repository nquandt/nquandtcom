
import { mkdir, readFile, writeFile } from "fs/promises";

import type { APIRoute } from "astro";
import { client } from "@/lib/ai";

export const POST: APIRoute = async ({ request }) => {
  const session = request.headers.get("session");
  const now = new Date();
  const requestBody = await request.json();

  const chatResponse = await client.chatCompletions(requestBody.messages);

  const fileNamePrefix = `./chunks/session_${session}/${now.getTime()}_assistant/`;

  await mkdir(fileNamePrefix, { recursive: true });

  const textFile = fileNamePrefix + "text.txt";

  await writeFile(textFile, chatResponse);

  return new Response(
    JSON.stringify({ text: chatResponse  }),
    {
      status: 200,
      headers: {
        "Content-Type": "application/json",
      },
    }
  );
};
