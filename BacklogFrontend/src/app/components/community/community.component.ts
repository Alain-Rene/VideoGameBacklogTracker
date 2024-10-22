import { Component } from '@angular/core';
import { User } from '../../models/user';
import { BackendService } from '../../services/backend.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-community',
  standalone: true,
  imports: [],
  templateUrl: './community.component.html',
  styleUrl: './community.component.css'
})
export class CommunityComponent {
  allUsers:User[] = [];

  constructor(
    private backendService: BackendService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(){
    this.displayUsers();
  }

  displayUsers() {
    this.backendService.getUsers().subscribe((response) => {
      console.log(response);
      this.allUsers = response;
      this.allUsers.sort((a, b) => b.totalXp - a.totalXp);
    })
  }
}
