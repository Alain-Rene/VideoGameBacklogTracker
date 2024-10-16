import { Component } from '@angular/core';
import { GameAPI } from '../../models/game';
import { BackendService } from '../../services/backend.service';
import { ProgressLog, RetrieveBackLogDTO } from '../../models/progresslog';

@Component({
  selector: 'app-user-logs',
  standalone: true,
  imports: [],
  templateUrl: './user-logs.component.html',
  styleUrl: './user-logs.component.css'
})
export class UserLogsComponent {
  loggedIn: boolean = false;
  userLogs:RetrieveBackLogDTO[] = [];
  userGames: GameAPI[] = [];

  constructor(private backendService:BackendService) {}

  ngOnInit() {
    this.getGamesById();
  }

  getGamesById() {
    this.backendService.getLogByUserIdDTO(3).subscribe(response => {
      console.log(response);
      this.userLogs = response;
    })
  }




}
