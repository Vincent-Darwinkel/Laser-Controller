import { Post, Get, Put } from '../shared/api/Api';
import { GetPatterns, PlayPattern } from '../shared/api/ApiActions';

export const AllPatterns = async () => {
    return await Get(GetPatterns, null, null);
}

export const StartPattern = async (json) => {
    return await Post(PlayPattern, null, json);
}