import { GameAPI } from "./game";
import { User } from "./user";

export interface ProgressLog {
    logID: number;
    userID: number;
    gameID: number;
    status: string;
    playtime: number;
    user: User;
    order: number;
}

export interface BackLogDTO {
    userId: number;
    gameId: number;
    status?: string;
    playTime?: number;
    order: number;
}

export interface RetrieveBackLogDTO {
    status?: string;
    playTime?: number;
    game: GameAPI;
    order: number;
}