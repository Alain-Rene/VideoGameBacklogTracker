import { Routes } from '@angular/router';
import { UserLogsComponent } from './components/user-logs/user-logs.component';
import { HomeComponent } from './components/home/home.component';
import { GameSearchComponent } from './components/game-search/game-search.component';
import { GameDetailsComponent } from './components/game-details/game-details.component';

export const routes: Routes = [
    {path:"Backlog/:id", component:UserLogsComponent},
    {path:"", component:HomeComponent},
    {path:"Search", component:GameSearchComponent},
    {path:"Details/:id",component:GameDetailsComponent}
];
