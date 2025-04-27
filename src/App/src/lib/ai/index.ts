import Groq from "groq-sdk";

const _client = new Groq({
  maxRetries: 3,
  timeout: 5000,
  apiKey: process.env["GROQ_API_KEY"], // This is the default and can be omitted
});

export const client = {
  transcribe: async (file: File) => {
    const response = await _client.audio.transcriptions.create({
      model: "whisper-large-v3",
      file: file,
    });

    return response.text;
  },
  chatCompletions: async (
    messages: { role: "user" | "system" | "assistant"; content: string }[]
  ) => {
    const chatCompletion = await _client.chat.completions.create({
      //
      // Required parameters
      //
      messages: messages,

      // The language model which will generate the completion.
      model: "llama-3.1-8b-instant",

      //
      // Optional parameters
      //

      // Controls randomness: lowering results in less random completions.
      // As the temperature approaches zero, the model will become deterministic
      // and repetitive.
      temperature: 0.5,

      // The maximum number of tokens to generate. Requests can use up to
      // 2048 tokens shared between prompt and completion.
      max_completion_tokens: 1024,

      // Controls diversity via nucleus sampling: 0.5 means half of all
      // likelihood-weighted options are considered.
      top_p: 1,

      // A stop sequence is a predefined or user-specified text string that
      // signals an AI to stop generating content, ensuring its responses
      // remain focused and concise. Examples include punctuation marks and
      // markers like "[end]".
      stop: null,

      // If set, partial message deltas will be sent.
      stream: false,
    });

    return chatCompletion.choices[0]?.message?.content || "";
  },
};
