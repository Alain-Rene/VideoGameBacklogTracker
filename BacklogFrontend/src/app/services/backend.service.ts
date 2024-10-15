import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GameAPI } from '../models/game';
import { ProgressLog } from '../models/progresslog';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class BackendService {
  url:string = "http://localhost:5264/";
  constructor(private http:HttpClient) { }

  getGames():Observable<GameAPI[]>
  {
    return this.http.get<GameAPI[]>(`${this.url}api/Game`);
  }

  getFilteredGames(name?:string, genre?:string, rating?:number, companyName?:string, platform?:string, releaseYear?:string):Observable<GameAPI[]>
  {
    return this.http.get<GameAPI[]>(`${this.url}filter`);
  }

  getGameById(id:number):Observable<GameAPI>
  {
    return this.http.get<GameAPI>(`${this.url}api/Game/${id}`);
  }

  getBacklogGamesByUserId(id:number):Observable<GameAPI[]>
  {
    return this.http.get<GameAPI[]>(`${this.url}backlog/${id}`);
  }

  getProgressLogs():Observable<ProgressLog[]>
  {
    return this.http.get<ProgressLog[]>(`${this.url}api/ProgressLog`);
  }

  deleteProgressLog():Observable<ProgressLog>
  {
    return this.http.delete<ProgressLog>(`${this.url}api/ProgressLog`);
  }

  getLogById(id:number):Observable<ProgressLog>
  {
    return this.http.get<ProgressLog>(`${this.url}api/ProgressLog/${id}`);
  }

  updateProgressLog(p:ProgressLog):Observable<ProgressLog>
  {
    return this.http.put<ProgressLog>(`${this.url}api/ProgressLog/${p.logID}`, p);
  }

  addProgressLog(p:ProgressLog):Observable<ProgressLog>
  {
    return this.http.post<ProgressLog>(`${this.url}DTO`, p);
  }

  getLogByIdDTO(id:number):Observable<ProgressLog>
  {
    return this.http.get<ProgressLog>(`${this.url}DTO/${id}`);
  }

  getUsers():Observable<User[]>
  {
    return this.http.get<User[]>(`${this.url}api/Users`);
  }

  addUser(u:User):Observable<User>
  {
    return this.http.post<User>(`${this.url}api/Users`, u);
  }

  deleteUser(u:User):Observable<User>
  {
    return this.http.delete<User>(`${this.url}api/Users`);
  }

  getUserById(id:number):Observable<User>
  {
    return this.http.get<User>(`${this.url}api/Users/${id}`);
  }

  updateUser(u:User):Observable<User>
  {
    return this.http.put<User>(`${this.url}api/Users/${u.id}`, u);
  }



}
