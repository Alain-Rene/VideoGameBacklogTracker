import { Component } from '@angular/core';
import { GameAPI } from '../../models/game';
import { BackendService } from '../../services/backend.service';
import { BackLogDTO, ProgressLog, RetrieveBackLogDTO } from '../../models/progresslog';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CdkDragDrop, DragDropModule, moveItemInArray } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-logs',
  standalone: true,
  imports: [FormsModule, DragDropModule, CommonModule],
  templateUrl: './user-logs.component.html',
  styleUrl: './user-logs.component.css'
})
export class UserLogsComponent {
  loggedIn: boolean = false;
  display: boolean[] = [];
  userLogs:RetrieveBackLogDTO[] = [];
  allLogs:ProgressLog[] = [];
  userGames: GameAPI[] = [];
  updatedGame = {} as RetrieveBackLogDTO;
  test:BackLogDTO = {} as BackLogDTO;

  constructor(
    private backendService:BackendService,
    private router: Router
  ) {}

  ngOnInit() {
    this.getGamesById();
  }

  getGamesById() {
    this.backendService.getLogByUserIdDTO(1).subscribe(response => {
      console.log(response);
      this.userLogs = response;
      this.display = new Array(this.userLogs.length).fill(false); // Initialize the display array
    })
  }

  navigateToDetails(gameId: number){
    this.router.navigate(['details/', gameId]);
  }

  updateGame(updatedLog:RetrieveBackLogDTO){
    this.test.gameId = updatedLog.game.id;
    this.test.status = updatedLog.status;
    this.test.playTime = updatedLog.playTime;
    this.test.order = updatedLog.order
    this.backendService.updateProgressLog(1, this.test).subscribe(response => {
      console.log(response);
      updatedLog.playTime = response.playtime;
      updatedLog.status = response.status;
      updatedLog.order = response.order;
    });
    this.getGamesById();
  }

  displayToggle(index:number){
    this.display[index] = !this.display[index];
    this.updatedGame.game.id = this.userLogs[index].game.id;
  }

  drop(event: CdkDragDrop<RetrieveBackLogDTO[]>){
    moveItemInArray(this.userLogs, event.previousIndex, event.currentIndex);
    this.userLogs.forEach((log, index) => {
      log.order = index;
      this.updateGame(log);
    });
  }

    getUpdatedImage(url:string): string{
    return url.replace("t_thumb", "t_original");
  }

  

}
