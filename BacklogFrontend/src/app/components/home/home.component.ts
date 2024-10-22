import { Component } from '@angular/core';
import { GameAPI, Genre, Platform } from '../../models/game';
import { BackendService } from '../../services/backend.service';
import { Router } from '@angular/router';
import { BackLogDTO } from '../../models/progresslog';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  // googleUser: SocialUser = {} as SocialUser;
  loggedIn: boolean = false;
  allGames:GameAPI[] = [];
  switchGames: GameAPI[] = [];
  PsGames: GameAPI[] = [];
  XboxGames: GameAPI[] = [];
  newProgressLog:BackLogDTO = {} as BackLogDTO;
  constructor(
    private backendService:BackendService,
    private router: Router
  ) {}

  ngOnInit() {
    this.displayGames();
    this.getSwitchGames();
    this.getPlaystationGames();
    this.getXboxGames();
  }
  displayGames() {
    this.backendService.getGames().subscribe(response => {
      console.log(response);
      this.allGames = response;
    });
  }

  getGenres(genres: Genre[]): string{
    if (genres == null)
    {
      return "N/A";
    }
    else
    {
      return genres.map(genre => genre.name).join(', ');
    }
    
  }

  getPlatforms(platforms: Platform[]): string{
    if (platforms == null){
      return "N/A";
    }
    else{
      return platforms.map(platform => platform.name).join(', ');
    }
  }

  getSwitchGames() {
    let platform:string = "Switch";
    this.backendService.getFilteredGames(undefined,undefined,86,"Nintendo",platform).subscribe(response => {
      console.log(response);
      this.switchGames = response;
    })
  }

  getPlaystationGames(){
    let platform:string = "PlayStation 5";
    this.backendService.getFilteredGames(undefined,undefined,88,undefined,platform).subscribe(response => {
      console.log(response);
      this.PsGames = response;
    })
  }

  getXboxGames(){
    let platform:string = "Xbox Series"
    this.backendService.getFilteredGames(undefined,undefined,83,undefined,platform).subscribe(response => {
      console.log(response);
      this.XboxGames = response;
    })
  }

  navigateToDetails(gameId: number){
    this.router.navigate(['details/', gameId]);
  }

  addToBacklog(userId:number, gameId:number){
    this.newProgressLog.gameId = gameId;
    this.newProgressLog.userId = userId;
    console.log(this.newProgressLog);
    this.backendService.addProgressLog(this.newProgressLog).subscribe(response => {
      console.log(response);
    });
  }
  
}

