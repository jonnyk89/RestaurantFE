import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from './user.model';
import { UsersService } from './users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[];
  isLoading = false;
  page = 1;
  totalUsers = 0;

  constructor(private usersService: UsersService, private router: Router, private route: ActivatedRoute, private _http: HttpClient) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.usersService.getAllUsers().subscribe(response => {
      this.totalUsers = response.length;
    });
    
    this.usersService.getFilteredUsers(this.page).subscribe(response => {
      this.users = response;
      console.log(this.users);
      this.isLoading = false;
    });
  }

  onPageChanged(page: number){
    this.page = page;
    this.usersService.getFilteredUsers(this.page).subscribe(response => {
      this.users = response;
      console.log(this.users);
      this.isLoading = false;
    });
  }

  onAddNewUser(){
    console.log(this.route.component);
    this.router.navigate(['new'], {relativeTo: this.route});
  }
  
  onEditUser(userId: string){
    this.router.navigate(['edit', userId], {relativeTo: this.route});
  }

  onDeleteUser(userId: string){
    this.usersService.deleteTargetUser(userId).subscribe(response => {
      this.ngOnInit();
      console.log(response);
    });
  }
}
