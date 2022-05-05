import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, map, Observable, Subscription, tap, throwError } from 'rxjs';
import { User } from './user.model';

export interface AuthResponseData {
  access_token: string,
  token_type: string,
  expires_in: number,
}

export interface CurrentUserResponseData {
  id: string,
  name: string,
  email: string,
  role: string,
  image: File,
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl = "https://localhost:7162/api/Authorize";
  user = new BehaviorSubject<User>(null);

  private _tokenExpirationTimer: any;
  isLoggedIn = false;

  constructor(private _http: HttpClient, private _router: Router) {

  }

  login(email: string, password: string) {
    return this._http.post<AuthResponseData>(this.apiUrl, {
      email: email,
      password: password
    }).pipe(catchError(this.handleError), tap(resData => {
      this.handleAuthentication(email, resData.access_token, resData.expires_in);
    }));
  }

  autoLogin(){
    const userData: {
      email: string;
      name: string;
      role: string;
      _token: string;
      _tokenExpirationDate: string;
    } = JSON.parse(localStorage.getItem('userData'));
    if(!userData){
      return;
    }

    const loadedUser = new User(userData.email, userData.name, userData.role, userData._token, new Date(userData._tokenExpirationDate));

    if(loadedUser.token){
      this.user.next(loadedUser);
      this.isLoggedIn = true;
      const expirationDuration = new Date(userData._tokenExpirationDate).getTime() - new Date().getTime();
      this.autoLogout(expirationDuration  );
    }
  }

  logout() {
    localStorage.removeItem('bearer');
    this.user.next(null);
    this._router.navigate(['/login']);
    localStorage.removeItem('userData');
    if (this._tokenExpirationTimer) {
      clearTimeout(this._tokenExpirationTimer);
    }
    this._tokenExpirationTimer = null;
    this.isLoggedIn = false;
  }

  autoLogout(expirationDuration: number) {
    this._tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expirationDuration);
  }

  private handleAuthentication(email: string, token: string, expiresIn: number) {
    localStorage.setItem('bearer', token);
    this.getCurrentUser().subscribe(response => {
      console.log(response);
      const expirationDate = new Date(new Date().getTime() + expiresIn * 1000);
      const user = new User(
        email,
        response == null ? '' : response['name'],
        response == null ? '' : response['role'],
        token,
        expirationDate,
      );
      this.user.next(user);
      this.autoLogout(expiresIn * 1000);
      localStorage.setItem('userData', JSON.stringify(user));
      this.isLoggedIn = true;
      console.log(user);
    });
  }


  private handleError(errorResponse: HttpErrorResponse) {
    //debugger
    let errorMessage = 'An unknown error occurred!';
    console.log(errorResponse);
    if (!errorResponse.error || !errorResponse.error.error) {
      return throwError(errorMessage);
    }
    switch (errorResponse.error.error) {
      case '_A user with the provided email address and password was not found_':
        errorMessage = 'A user with the provided email address and password was not found.';
      case '_Invalid email address_':
        errorMessage = 'Invalid email address.';
      case '_Invalid password_':
        errorMessage = 'Invalid password.';
    } 
    return throwError(errorMessage);
  }

  getCurrentUser() {
    return this._http.get<CurrentUserResponseData>("https://localhost:7162/me", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }
}
