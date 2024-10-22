import { Component } from '@angular/core';
import { User } from '../../models/user';
import { BackendService } from '../../services/backend.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  isLoggedIn: Boolean = false;
  currentUser:User = {} as User;
  friends: User[] = [];
  numGames:number = {} as number;

  constructor(
    private backendService: BackendService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(){
    this.displayUserInfo();
  }

  displayUserInfo(){
    this.activatedRoute.paramMap.subscribe((params) => {
      let id: number = Number(params.get('id'));

      this.getUserInfo(id);

      this.getFriends(id);

    })
  }

  getUserInfo(id:number){
    this.backendService.getUserById(id).subscribe(response => {
      console.log(response);
      this.currentUser = response;
    })
  }

  getFriends(id:number){
    this.backendService.getFriendsById(id).subscribe(response => {
      console.log(response);
      this.friends = response;
    })
  }

  getGamesInBacklog(){
    
  }

}
