import { Post, Get } from '../shared/api/Api';
import { RunAudio, CancelAudio, AudioDevices } from '../shared/api/ApiActions';

export const StartAudio = async () => {
    try {
        Post(RunAudio, null, null);
    } catch (error) { }
}

export const StopAudio = async () => {
    try {
        Post(CancelAudio, null, null);
    } catch (error) { }
}

export const GetAudioDevices = async () => {
    return await Get(AudioDevices, null, null);
}