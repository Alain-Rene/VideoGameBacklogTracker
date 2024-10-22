import { Component } from '@angular/core';
import { GameAPI } from '../../models/game';
import { BackendService } from '../../services/backend.service';
import { BackLogDTO, ProgressLog, RetrieveBackLogDTO } from '../../models/progresslog';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { User } from '../../models/user';

@Component({
  selector: 'app-user-logs',
  standalone: true,
  imports: [FormsModule],
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
  currentUser = {} as User;
  test:BackLogDTO = {} as BackLogDTO;

  constructor(
    private backendService:BackendService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.getGamesById();
    this.getCurrentUser();
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
    this.backendService.updateProgressLog(1, this.test).subscribe(response => {
      console.log(response);
      updatedLog.playTime = response.playtime;
      updatedLog.status = response.status;

    });
    if(updatedLog.status == "Complete")
    {
      this.backendService.updateEXP(this.currentUser).subscribe((response) => {
      console.log(response);
      })
    }
    this.getGamesById();
  }

  getCurrentUser(){
    this.activatedRoute.paramMap.subscribe((params) => {
      let id: number = Number(params.get('id'));

      this.backendService.getUserById(id).subscribe((response) => {
        console.log(response);
        this.currentUser = response;
      });

     
    });
  }

  displayToggle(index:number){
    this.display[index] = !this.display[index];
    this.updatedGame.game.id = this.userLogs[index].game.id;
  }



}
