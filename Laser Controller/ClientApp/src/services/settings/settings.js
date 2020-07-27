import { Post, Get, Put } from '../shared/api/Api';
import { ComPorts, StoreSettings, FetchSettings } from '../shared/api/ApiActions';

export const GetComPorts = async () => {
    return await Get(ComPorts, null, null);
}

export const SaveSettings = async (json) => {
    return await Post(StoreSettings, null, json);
}

export const GetSettings = async () => {
    return await Get(FetchSettings, null, null);
}