import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { User } from './user.model';

export interface GetUsersResponseData {
  id: string,
  name: string,
  email: string,
  role: string,
  image?: File,
}

export interface UserResponseData {
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class UsersService implements OnInit {
  constructor(private _http: HttpClient) {

  }

  ngOnInit(): void {
    
  }

  getAllUsers() {

    return this._http.get<GetUsersResponseData[]>("https://localhost:7162/Api/Users", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  getFilteredUsers(page: number) {
    let params = new HttpParams();
    params = params.append('page', page === null ? 0 : page);
    params = params.append('pageSize', 20);
    return this._http.get<GetUsersResponseData[]>("https://localhost:7162/Api/Users", {
      params: params
    });
  }

  getTargetUser(userId: string){
    return this._http.get("https://localhost:7162/api/Users/" + userId,
    {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  CreateNewUser(firstName: string, lastName: string, email: string, role: string, password: string) {
    return this._http.post<UserResponseData>("https://localhost:7162/api/Users", {
      firstName,
      lastName,
      email,
      role,
      password,
    });
  }

  updateTargetUser(userId: string, firstName: string, lastName: string, email: string, role: string, password: string){
    return this._http.put<UserResponseData>("https://localhost:7162/api/Users/" + userId, {
      firstName,
      lastName,
      email,
      role,
      password,
    },
    {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  deleteTargetUser(userId: string){
    return this._http.delete<UserResponseData>("https://localhost:7162/api/Users/" + userId,
    {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }
}
