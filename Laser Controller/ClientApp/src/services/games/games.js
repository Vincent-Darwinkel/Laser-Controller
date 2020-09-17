import { Post, Get, Put } from '../shared/api/Api';
import { GetGames, PlayGame, MoveCharacter } from '../shared/api/ApiActions';

export const AllGames = async () => {
    return await Get(GetGames, null, null);
}

export const StartGame = async (game) => {
    return await Post(PlayGame + game, null, null);
}

export const MovePlayer = async (key) => {
    return await Post(MoveCharacter + key, null, null);
}