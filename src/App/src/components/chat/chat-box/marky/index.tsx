import type { PropsWithChildren } from "react";
import Markdown from "react-markdown";
import { Prism as SyntaxHighlighter } from "react-syntax-highlighter";
import { oneDark } from "react-syntax-highlighter/dist/cjs/styles/prism";
import { copyTextToClipboard } from "@/utils";

export const Marky = ({ document }: { document: string }) => {
  return (
    <>
      <button
        type="button"
        className={`p-2 rounded-sm cursor-pointer absolute right-0 top-0 m-2 bg-light i-carbon-copy`}
        onClick={() => copyTextToClipboard(document)}
      >
        Copy Full
      </button>
      <Prose>
        <Markdown
          children={document}
          //   remarkPlugins={[remarkGfm]}
          components={{
            code({ node, className, children, style, ...rest }) {
              const match = /language-(\w+)/.exec(className || "");

              //can we match a node by the actual content? i.e. is a code link
              return match ? (
                <div className="relative">
                  <button
                    type="button"
                    className={`p-2 rounded-sm cursor-pointer absolute right-0 top-0 m-2 bg-light i-carbon-copy`}
                    onClick={() => copyTextToClipboard(children as string)}
                  ></button>
                  {/*@ts-ignore*/}
                  <SyntaxHighlighter
                    {...rest}
                    PreTag="div"
                    lineProps={{
                      style: {
                        wordBreak: "break-all",
                        whiteSpace: "pre-wrap",
                      },
                    }}
                    wrapLines={true}
                    children={String(children).replace(/\n$/, "")}
                    language={match[1]}
                    style={oneDark}
                  />
                </div>
              ) : (
                //Could also do on click of code file links "expand"
                <code
                  onClick={() => copyTextToClipboard(children as string)}
                  {...rest}
                  className={`cursor-pointer inlineCode ${className}`}
                >
                  {children}
                </code>
              );
            },
          }}
        />
      </Prose>
    </>
  );
};

const Prose = ({ children }: PropsWithChildren) => {
  return (
    <div className="prose max-w-full dark:prose-invert prose-a:text-purple-400 prose-p:text-justify prose-img:rounded-xl prose-code:before:content-none prose-code:after:content-none prose-pre:p-0">
      {children}
    </div>
  );
};
