import { Post, Get } from '../shared/api/Api';
import { RunAudio, CancelAudio, AudioDevices } from '../shared/api/ApiActions';

export const StartAudio = async (device) => {
    return await Post(RunAudio, null, device);
}

export const StopAudio = async () => {
    return await Post(CancelAudio, null, null);
}

export const GetAudioDevices = async () => {
    return await Get(AudioDevices, null, null);
}