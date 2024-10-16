import { Component } from '@angular/core';
import { GameAPI } from '../../models/game';
import { BackendService } from '../../services/backend.service';
import { ProgressLog } from '../../models/progresslog';

@Component({
  selector: 'app-user-logs',
  standalone: true,
  imports: [],
  templateUrl: './user-logs.component.html',
  styleUrl: './user-logs.component.css'
})
export class UserLogsComponent {
  loggedIn: boolean = false;
  userLogs:ProgressLog[] = [];
  userGames: GameAPI[] = [];

  constructor(private backendService:BackendService) {}

  ngOnInit() {
    this.displayLogs();
    this.getGamesById();
  }

  displayLogs() {
    this.backendService.getProgressLogs().subscribe(response => {
      console.log(response);
      this.userLogs = response;
    });
  }

  getGamesById() {
    this.backendService.getBacklogGamesByUserId(1).subscribe(response => {
      console.log(response);
      this.userGames = response;
    })
  }




}
