import { GameAPI } from "./game";
import { User } from "./user";

export interface ProgressLog {
    logID: number;
    userID: number;
    gameID: number;
    status: string;
    playtime: string;
    user: User;
}

export interface BackLogDTO {
    userId: number;
    gameId: number;
    status: string;
    playTime: number;
}

export interface RetrieveBackLogDTO {
    status: string;
    playTime: number;
    game: GameAPI;
}