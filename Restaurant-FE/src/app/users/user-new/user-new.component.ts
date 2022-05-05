import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsersService } from '../users.service';

@Component({
  selector: 'app-user-new',
  templateUrl: './user-new.component.html',
  styleUrls: ['./user-new.component.css']
})
export class UserNewComponent implements OnInit {
  name: string;
  role: string;
  email: string;
  password: string;
  newUserForm: FormGroup;
  error: string = '';
  emailTaken: boolean = false;

  // error messages:
  roleError = 'Please select a role';
  firstNameError = 'Please enter a first name (less than 100 characters)';
  lastNameError = 'Please enter a last name (less than 100 characters)';
  emailError = 'Please eneter a valid email (less than 255 characters).';
  passwordError = 'Please enter a valid password (between 8 and 100 characters).';

  constructor(private usersService: UsersService, private _http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(){
    this.newUserForm = new FormGroup({
      'role': new FormControl(0, Validators.required),
      'firstName': new FormControl(null, [
        Validators.required,
      ]),
      'lastName': new FormControl(null, [
        Validators.required,
      ]),
      'email': new FormControl(null, [
        Validators.email,
      ]),
      'password': new FormControl(null, [
        Validators.required,
      ])
    })
    this.newUserForm.controls['role'].setValue('Waiter', {onlySelf: true});
  }

  onSubmit(){
    let firstName = this.newUserForm.get('firstName').value;
    let lastName = this.newUserForm.get('lastName').value;
    let email = this.newUserForm.get('email').value;
    let role = this.newUserForm.get('role').value;
    let password = this.newUserForm.get('password').value;

    this.usersService.CreateNewUser(firstName, lastName, email, role, password).subscribe(response => {
      if(response.message === '_Email address is already in use_'){
        this.emailError = "Email address is already in use";
        this.emailTaken = true;
        alert(this.emailError);
        console.log(this.emailError);
      }
      else if(response.message === '_User created successfully_'){
        this.emailTaken = false;
        this.router.navigate(['/users']);
      }
    },
    err => {
      
    });
  }
}
