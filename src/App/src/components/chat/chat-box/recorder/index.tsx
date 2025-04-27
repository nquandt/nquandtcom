import { useEffect, useState, type SetStateAction } from "react";

type AudioListener = {
  timeout?: NodeJS.Timeout;
  stoppingTimeout?: NodeJS.Timeout;
  mediaRecorder: MediaRecorder;
  startTime: number;
  analyser: AnalyserNode;
  hasBeenTalkingCount: number;
  recording: boolean;
  stopping: boolean;
};

// Define the volume threshold
const VOLUME_THRESHOLD = 0.05; // Adjust based on your needs (range: 0 to 1)
const BUFFER_LENGTH = 1024; // Size of the buffer for volume analysis

function checkVolume(
  audioListener: AudioListener,
  audioData: Uint8Array,
  setIsVoiceActive: React.Dispatch<SetStateAction<boolean>>
) {
  const { analyser } = audioListener;
  analyser.getByteFrequencyData(audioData);

  // Calculate the average volume from the frequency data
  let totalVolume = 0;
  for (let i = 0; i < audioData.length; i++) {
    totalVolume += audioData[i];
  }
  const averageVolume = totalVolume / audioData.length;

  const isInVolume = averageVolume / 255 > VOLUME_THRESHOLD;

  if (isInVolume) {
    setIsVoiceActive(true);
    audioListener.hasBeenTalkingCount++;
    clearTimeout(audioListener.stoppingTimeout);
    audioListener.stopping = false;
  }

  // If the average volume exceeds the threshold, start recording
  if (isInVolume && !audioListener.recording) {
    audioListener.hasBeenTalkingCount = 0;
    console.log(averageVolume / 255);
    audioListener.mediaRecorder.start();
    audioListener.startTime = Date.now(); // Capture start time
    audioListener.recording = true;
    console.log("Recording started...");
  }
  // If the average volume falls below the threshold, stop recording
  else if (!isInVolume && audioListener.recording && !audioListener.stopping) {
    setIsVoiceActive(false);
    audioListener.stopping = true;
    audioListener.stoppingTimeout = setTimeout(() => {
      audioListener.hasBeenTalkingCount = 0;
      audioListener.mediaRecorder.stop();
      audioListener.recording = false;
      audioListener.stopping = false;
      console.log("Recording stopped...");
    }, 500);
  }
  // Repeat the volume check at regular intervals (e.g., every 50ms)

  audioListener.timeout = setTimeout(
    () => checkVolume(audioListener, audioData, setIsVoiceActive),
    audioListener.hasBeenTalkingCount < 10 ? 10 : 50
  );
}

export const useRecorder = (
  callback: (blob: Blob, recordingLength: number) => void
) => {
  const [isVoiceActive, setIsVoiceActive] = useState(false);
  const [audioData] = useState(new Uint8Array(BUFFER_LENGTH));
  const [audioListener, setAudioListener] = useState<AudioListener | null>(
    null
  );

  useEffect(() => {
    navigator.mediaDevices
      .getUserMedia({ audio: true })
      .then((stream) => {
        const audioContext = new (window.AudioContext ||
          (window as any).webkitAudioContext)();
        const analyser = audioContext.createAnalyser();

        // Connect microphone input to the analyser node
        const microphone = audioContext.createMediaStreamSource(stream);
        microphone.connect(analyser);

        // Create the media recorder
        const mediaRecorder = new MediaRecorder(stream);

        console.log("setting lsitener");

        setAudioListener({
          analyser,
          mediaRecorder,
          hasBeenTalkingCount: 0,
          recording: false,
          stopping: false,
          startTime: 0,
        });
      })
      .catch((err) => {
        console.error("Error accessing microphone:", err);
      });
  }, []);

  useEffect(() => {
    console.log({ audioListener });
    if (audioListener) {
      const { mediaRecorder } = audioListener;
      mediaRecorder.ondataavailable = (event) => {
        console.log("Recording data available:", event.data);
        // Handle recorded data here (e.g., save to file)

        if (audioListener.startTime) {
          const currentTime = Date.now();
          const recordingLength =
            (currentTime - audioListener.startTime) / 1000; // in seconds
          console.log(
            `Recording length: ${recordingLength.toFixed(2)} seconds`
          );

          callback(
            new Blob([event.data], { type: mediaRecorder.mimeType }),
            recordingLength
          );
        }
      };
    }
  }, [audioListener]);

  const startListening = () => {
    audioListener && checkVolume(audioListener, audioData, setIsVoiceActive);
  };

  const stopListening = () => {
    if (audioListener) {
      clearTimeout(audioListener.timeout);
      audioListener.hasBeenTalkingCount = 0;
      audioListener.mediaRecorder.stop();
      audioListener.recording = false;
    }
  };

  return [isVoiceActive, startListening, stopListening] as const;
};
