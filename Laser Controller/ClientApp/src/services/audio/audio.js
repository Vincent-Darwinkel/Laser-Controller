import { Post, Get } from '../shared/api/Api';
import { RunAudio, CancelAudio, AudioDevices, CalibrateAudioVolume, CalibrationValue } from '../shared/api/ApiActions';

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

export const CalibrateAudio = async (value) => {
    return await Post(CalibrateAudioVolume + value, null, null);
}

export const GetAudioDevices = async () => {
    return await Get(AudioDevices, null, null);
}

export const GetCalibrationValue = async () => {
    return await Get(CalibrationValue, null, null);
}