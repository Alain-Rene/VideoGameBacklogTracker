import { Component } from '@angular/core';
import { GameAPI, Genre } from '../../models/game';
import { BackendService } from '../../services/backend.service';

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
  constructor(private backendService:BackendService) {}

  ngOnInit() {
    this.displayGames();
  }
  displayGames() {
    this.backendService.getGames().subscribe(response => {
      console.log(response);
      this.allGames = response;
    });
  }

  getGenres(genres: Genre[]): string{
    return genres.map(genre => genre.name).join(', ');
  }
}

