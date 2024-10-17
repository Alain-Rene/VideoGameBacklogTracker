import { Component } from '@angular/core';
import { GameAPI, Genre, InvolvedCompany, Platform, ReleaseDate } from '../../models/game';
import { BackendService } from '../../services/backend.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-game-details',
  standalone: true,
  imports: [],
  templateUrl: './game-details.component.html',
  styleUrl: './game-details.component.css',
})
export class GameDetailsComponent {
  currentGame: GameAPI = {} as GameAPI;

  constructor(
    private backendService: BackendService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.displayGameInfo();
  }

  displayGameInfo() {
    this.activatedRoute.paramMap.subscribe((params) => {
      let id: number = Number(params.get('id'));

      this.backendService.getGameById(id).subscribe((response) => {
        console.log(response);
        this.currentGame = response;
      });
    });
  }

  getGenres(genres: Genre[]): string {
    if (genres == null) {
      return 'N/A';
    } else {
      return genres.map((genre) => genre.name).join(', ');
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

  //Takes in string date, converts to Date datatype
  parseDate(dateStr: string): Date {
    let values: string[] = dateStr.split(' ');
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
  getEarliestDate(values: ReleaseDate[]): Date {
    let result: Date = this.parseDate(values[0].human);
    values.forEach((d) => {
      let newDate: Date = this.parseDate(d.human);
      if (newDate < result) {
        result = newDate;
      }
    });
    return result;
  }
}
