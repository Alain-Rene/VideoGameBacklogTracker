import { Component } from '@angular/core';
import { GameAPI, Genre } from '../../models/game';
import { BackendService } from '../../services/backend.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-game-search',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './game-search.component.html',
  styleUrl: './game-search.component.css'
})
export class GameSearchComponent {
  filteredGames:GameAPI[] = [];
  display:boolean = false;
  name:string = "";
  genre:string = "";
  rating:number|undefined;
  companyName:string|undefined;
  platform:string|undefined;
  releaseYear:string|undefined;

  constructor(private backendService:BackendService) {}

  filterGames(){
    this.name || undefined,
    this.genre || undefined,
    this.rating !== null && this.rating !== 0 ? this.rating : undefined,
    this.companyName || undefined,
    this.platform || undefined,
    this.releaseYear || undefined
    this.backendService.getFilteredGames(this.name, this.genre, this.rating, this.companyName, this.platform, this.releaseYear).subscribe(response =>{
      console.log(response);
      this.filteredGames = response;
    })
  }

  displayToggle(){
    this.display = !this.display;
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

  
}
