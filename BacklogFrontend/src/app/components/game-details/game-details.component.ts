import { Component } from '@angular/core';
import { GameAPI, GameVideo, Genre, InvolvedCompany, Platform, ReleaseDate } from '../../models/game';
import { BackendService } from '../../services/backend.service';
import { ActivatedRoute } from '@angular/router';
import { BackLogDTO } from '../../models/progresslog';
import { YoutubePlayerComponent } from '../youtube-player/youtube-player.component';

@Component({
  selector: 'app-game-details',
  standalone: true,
  imports: [YoutubePlayerComponent],
  templateUrl: './game-details.component.html',
  styleUrl: './game-details.component.css',
})
export class GameDetailsComponent {
  currentGame: GameAPI = {} as GameAPI;
  newProgressLog:BackLogDTO = {} as BackLogDTO;
  currentGameVideos: GameVideo[] = {} as GameVideo[];
  showMore: Boolean = false;
  similarGames:GameAPI[] = [];

  constructor(
    private backendService: BackendService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.displayGameInfo();
    this.getSimilarGames();
  }

  displayGameInfo() {
    this.activatedRoute.paramMap.subscribe((params) => {
      let id: number = Number(params.get('id'));

      this.backendService.getGameById(id).subscribe((response) => {
        console.log(response);
        this.currentGame = response;
      });

      this.backendService.getGameVideosByGameId(id).subscribe((response) => {
        console.log(response);
        this.currentGameVideos = response;
      })
    });
  }

  toggleShowMore(){
    this.showMore = !this.showMore;
  }
  activeVideoIndex: number | null = null;

playVideo(index: number) {
  this.activeVideoIndex = index;
}

  getGenres(genres: Genre[]): string {
    if (genres == null) {
      return 'N/A';
    } else {
      return genres.map((genre) => genre.name).join(', ');
    }
  }
  getOneGenre(genres: Genre[]): string {
    if (genres == null){
      return '';
    }
    else {
      return genres[0].name;
    }

  }

  getPlatforms(platforms: Platform[]): string {
    if (platforms == null) {
      return 'N/A';
    } else {
      return platforms.map((platform) => platform.name).join(', ');
    }
  }

  getCompanies(companies: InvolvedCompany[]): string {
    if (companies == null) {
      return 'N/A';
    } else {
      return companies.map((company) => company.company.name).join(', ');
    }
  }

  getUpdatedImage(url:string): string{
    return url.replace("t_thumb", "t_original");
  }

  //Takes in string date, converts to Date datatype
  parseDate(dateStr: string): Date | number{
    let values: string[] = dateStr.split(' ');

    if(values.length === 1) {
      let year: number = Number(values[0]);
      return Number(values[0]);
    }
    let monthValues: string[] = [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec',
    ];
    let month: number = monthValues.indexOf(values[0]);
    let day: number = Number(values[1].substring(0, values[1].length - 1));
    let year: number = Number(values[2]);
    return new Date(year, month, day); // Add a space after the comma
  }
  //takes in array of dates (Api's format) and returns earliest Date
  getEarliestDate(values: ReleaseDate[]): Date | number {
    let result: Date | number = this.parseDate(values[0].human);
    values.forEach((d) => {
      let newDate: Date | number = this.parseDate(d.human);
      if (typeof newDate === 'number') {
        // If it's a year only date
        if (result === null || (typeof result === 'number' && newDate < result)) {
          result = newDate;
        }
      } else {
        if (result === null || (typeof result === 'number') ||newDate < result) {
          result = newDate;
        }
      }
      
    });
    return result;
  }
  isDateInstance(releaseDate: Date | number): string {
    if (releaseDate instanceof Date){
      return releaseDate.toDateString();
    }
    else {
      return releaseDate.toString();
    }
  }
  addToBacklog(userId:number, gameId:number){
    this.newProgressLog.gameId = gameId;
    this.newProgressLog.userId = userId;
    console.log(this.newProgressLog);
    this.backendService.addProgressLog(this.newProgressLog).subscribe(response => {
      console.log(response);
    });
  }

  getSimilarGames(){
    this.activatedRoute.paramMap.subscribe((params) => {
      let id: number = Number(params.get('id'));
      this.backendService.getSimilarGamesById(id).subscribe(response => {
        console.log(response);
        this.similarGames = response;
      })
    })
  }

  goToDetails(gameId:number){
    this.backendService.navigateToDetails(gameId);
  }
}
