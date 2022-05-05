import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { AuthResponseData, AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  invalidLogin: boolean;
  isLoading = false;
  error: string = "";

  constructor(private router: Router, private http: HttpClient, private _authService: AuthService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  onLogin(form: NgForm){
    if(!form.valid){
      return;
    }
    this.isLoading = true;
    const email = form.value.email;
    const password = form.value.password

    let authObs: Observable<AuthResponseData>;

    authObs = this._authService.login(email, password);
    
    authObs.subscribe(response => {
      const token = (<AuthResponseData>response).access_token;
      localStorage.setItem('bearer', token);
      this.error = '';
      this.invalidLogin = false;
      this.isLoading = false;
      this.router.navigate(["/orders"]);
      this.toastr.success('You are logged in!', 'Success!' );
      console.log(response);

    }, err => {
      //debugger
      console.log(err);
      this.invalidLogin = true;
      this.error = err;
      this.toastr.error(err, 'Error:');
      this.isLoading = false;
    });
  }

}
