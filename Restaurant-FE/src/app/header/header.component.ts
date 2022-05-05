import { Component, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy, OnChanges {
  isAuthenticated = false;
  isAdmin = false;
  name = '';
  private userSub: Subscription;


  constructor(private router: Router, private _authService: AuthService) { }

  ngOnInit(): void {
    this.userSub = this._authService.user.subscribe(user => {
      if(user){
        this.isAuthenticated = !!user;
        this.isAdmin = user.role === "Admin" ? true : false;
        this.name = user.name;
      }
      else{
        this.isAuthenticated = true;
        const userData: {
          email: string;
          name: string;
          role: string;
          _token: string;
          _tokenExpirationDate: string;
        } = JSON.parse(localStorage.getItem('userData'));
        this.isAdmin = userData.role === 'Admin' ? true : false;
        this.name = userData.name;
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
    if(changes){
    }
  }

  onProfileEdit(){
    this.router.navigate(['/users/edit']);
  }

  onLogout(){
    this._authService.logout();
    this.isAuthenticated = false;
    this.isAdmin = false;
    this.router.navigate(['/login']);
  }

  ngOnDestroy(): void {
    this.userSub.unsubscribe();    
  }

}
