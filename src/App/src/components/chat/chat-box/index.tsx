import { useEffect, useState } from "react";
import { Marky } from "./marky";
import { useRecorder } from "./recorder";

type MessageType = {
  role: "system" | "assistant" | "user";
  content: string;
};

type MessagesType = {
  messages: MessageType[];
};

const defaultMessages: MessageType[] = [
  {
    role: "system",
    content: "You are a software and digital technologies assistant.",
  },
];

export const ChatBox = () => {
  const [session] = useState(crypto.randomUUID());
  const [messages, setMessages] = useState(defaultMessages);
  const [isRecording, start, stop] = useRecorder((blob, recordingLength) => {
    fetch("/api/transcribe", {
      method: "POST",
      headers: {
        "session": session,
        "recording-length": `${recordingLength}`,
      },
      body: blob,
    }).then(async (x) => {
      const json = await x.json();

      if (json.noContent) {
        return;
      }
      setMessages((m) => [...m, { role: "user", content: json.text }]);
    });
  });

  useEffect(() => {
    const lm = messages[messages.length - 1];
    if (lm.role !== "user") {
      return;
    }

    setTimeout(() => {
      const lastMessage = messages[messages.length - 1];
      if (lm != lastMessage || lastMessage.role !== "user" || isRecording) {
        return;
      }

      fetch("/api/chat", {
        method: "POST",
        headers: {
          "session": session,
        },
        body: JSON.stringify({
          messages,
        }),
      }).then(async (x) => {
        if (x.status === 204) {
          return;
        }

        const json = await x.json();
        setMessages((m) => [...m, { role: "assistant", content: json.text }]);
      });
    }, 1400);
  }, [messages]);

  return (
    <div className="flex flex-col w-full">
      <div className="flex ml-auto gap-4">
        <div
          className={`p-4 bg-light ${
            !isRecording
              ? "i-carbon-recording"
              : "i-carbon-recording-filled-alt"
          }`}
        >
          {isRecording}
        </div>
        <button
          type="button"
          className="px-4 py-2 bg-zinc-800 rounded-sm"
          onClick={start}
        >
          Listen
        </button>
        <button
          type="button"
          className="px-4 py-2 bg-zinc-800 rounded-sm"
          onClick={stop}
        >
          Stop
        </button>
      </div>
      <Messages messages={messages} />
    </div>
  );
};

const Messages = ({ messages }: MessagesType) => {
  const [system, ...rest] = messages;
  return (
    <div className="flex flex-col w-full gap-4">
      <div className="flex">
        <div>System Prompt: </div>
        <div>{system.content}</div>
      </div>
      <hr className="dark:border-t-zinc-100 light:border-t-zinc-900 w-full !print:border-t-black" />
      <div className="flex flex-col w-full gap-4">
        {rest.map((m, i) => (
          <Message key={i} {...m} />
        ))}
      </div>
    </div>
  );
};

const Message = ({ content, role }: MessageType) => {
  const align =
    role === "assistant" ? "mr-auto bg-zinc-800" : "ml-auto bg-slate-800";
  return (
    <div className={`${align} px-4  rounded-lg max-w-4/5`}>
      <Marky document={content} />
    </div>
  );
};
