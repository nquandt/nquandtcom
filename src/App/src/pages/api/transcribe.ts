import { mkdir, readFile, writeFile } from "fs/promises";
import type { APIRoute } from "astro";
import { client } from "@/lib/ai";

export const POST: APIRoute = async ({ request }) => {
  const session = request.headers.get("session");
  const now = new Date();

  const recordingLength = new Number(request.headers.get("recording-length"));
  console.log(recordingLength);

  if (recordingLength.valueOf() < 0.5) {
    return new Response(JSON.stringify({ noContent: true }), {
      status: 200,
      headers: {
        "Content-Type": "application/json",
      },
    });
  }

  const blob = await request.blob();

  const fileNamePrefix = `./chunks/session_${session}/${now.getTime()}_user/`;

  await mkdir(fileNamePrefix, { recursive: true });

  const audioFile = fileNamePrefix + "voice.ogg";

  await writeFile(audioFile, await blob.stream());

  let transcription = "";
  for (let i = 0; i < 3; i++) {
    try {
      const blob = await readFile(audioFile);
      const file = new File([blob], audioFile.split("/").pop()!, {
        type: "audio/ogg",
      });
      transcription = await client.transcribe(file);
      if (transcription) {
        await writeFile(fileNamePrefix + "text.txt", transcription, {
          encoding: "utf-8",
        });

        break;
      }
    } catch (e) {}
  }

  return new Response(
    JSON.stringify({ size: blob.size, text: transcription }),
    {
      status: 200,
      headers: {
        "Content-Type": "application/json",
      },
    }
  );
};
