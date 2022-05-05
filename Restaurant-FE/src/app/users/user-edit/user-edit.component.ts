import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UsersService } from '../users.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  name: string;
  role: string;
  email: string;
  password: string;
  editUserForm: FormGroup;
  error: string = '';
  emailTaken: boolean = false;
  userId: string;

  // error messages
  emailError = 'Please eneter a valid email (less than 255 characters).';

  constructor(private usersService: UsersService, private route: ActivatedRoute, private _http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(){
    this.userId = this.route.snapshot.params['userId'];
    // let name: string;
    // let email: string;
    let role: string;
    this.usersService.getTargetUser(this.userId).subscribe(user => {
      // console.log(user);
      // name = user['name'];
      // email = user['email'];
      role = user['role'];
    });

    this.editUserForm = new FormGroup({
      'role': new FormControl(0),
      'firstName': new FormControl(null),
      'lastName': new FormControl(null),
      'email': new FormControl(null, [
        Validators.email,
      ]),
      'password': new FormControl(null)
    })
    this.editUserForm.controls['role'].setValue(role === 'Admin' ? 'Admin' : 'Waiter', {onlySelf: true});
  }

  onSubmit(){
    let role = this.editUserForm.get('role').value;
    let firstName = this.editUserForm.get('firstName').value;
    let lastName = this.editUserForm.get('lastName').value;
    let email = this.editUserForm.get('email').value;
    let password = this.editUserForm.get('password').value;

    this.usersService.updateTargetUser(this.userId, firstName, lastName, email, role, password).subscribe(response => {
      if(response.message === '_Email address is already in use_'){
        this.emailError = "Email address is already in use";
        this.emailTaken = true;
        alert(this.emailError);
        console.log(this.emailError);
      }
      else if(response.message === '_User updated successfully_'){
        this.emailTaken = false;
        this.router.navigate(['/users']);
      }

      const userData: {
        email: string;
        name: string;
        role: string;
        _token: string;
        _tokenExpirationDate: string;
      } = JSON.parse(localStorage.getItem('userData'));

      let fName = '';
      let lName = '';
      let mail = '' ? userData.email : email;
      let name = '';

      if(firstName != null || lastName != null){
        fName = firstName === null ? userData.name.split(' ')[0] : firstName;
        lName = lastName === null ? userData.name.split(' ')[1] : lastName;
      }

      name = fName + ' ' + lName;

      userData['email'] = mail;
      userData.name = name;
      localStorage.setItem('userData', JSON.stringify(userData));
      console.log('success');
    },
    err => {
      
    });
  }
}
