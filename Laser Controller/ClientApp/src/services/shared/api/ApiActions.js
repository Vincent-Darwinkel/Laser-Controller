export const ApiUrl = "api";

export const ComPorts = ApiUrl + "/settings/getcomports/";
export const StoreSettings = ApiUrl + "/settings/savesettings/";
export const FetchSettings = ApiUrl + "/settings/getsettings/";

export const RunAudio = ApiUrl + "/audio/start";
export const CancelAudio = ApiUrl + "/audio/stop";
export const AudioDevices = ApiUrl + "/audio/devices";
export const CalibrateAudioVolume = ApiUrl + "/audio/calibrate/";
export const CalibrationValue = ApiUrl + "/audio/calibration-value/";

export const GetGames = ApiUrl + "/games/all/";
export const PlayGame = ApiUrl + "/games/start/";
export const MoveCharacter = ApiUrl + "/games/move/";

export const GetPatterns = ApiUrl + "/pattern/patterns/";
export const PlayPattern = ApiUrl + "/pattern/play/";