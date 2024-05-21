import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';

import {
  FormControl,
  FormGroup,
  FormsModule,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
  FormBuilder
} from '@angular/forms';

import { CommonModule } from '@angular/common';
import PasswordValidationComponent from '../password-validation/password-validation.component';
import { UsersService } from '../../services/users.service';
// interface IRegister {
//   firstName: string;
//   lastName: string;
//   email: string;
//   phoneNumber: string;
//   age: number;
//   street: string;
//   city: string;
//   governorate: string;
//   ssn: string;
//   password: string;
// }
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [HttpClientModule, FormsModule, ReactiveFormsModule, CommonModule],
  providers: [UsersService],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  // registerForm = new FormGroup({
  //   firstName: new FormControl(''),
  //   lastName: new FormControl(''),
  //   email: new FormControl(''),
  //   ssn: new FormControl(''),
  //   password: new FormControl(''),
  //   confirmPassword: new FormControl(''),
  //   age: new FormControl(0),
  //   phoneNumber: new FormControl(''),
  //   street: new FormControl(''),
  //   city: new FormControl(''),
  //   governorate: new FormControl(''),
  // });
  // constructor(
  //   private router: Router,
  //   private AddingUser: UsersService,
  //   private formBuilder: FormBuilder
  // ) {}
  // ngOnInit(): void {
  //   this.registerForm = this.formBuilder.group(
  // {
  //   firstName: ['', Validators.required],
  //   lastName: ['', Validators.required],
  //   email: ['', [Validators.required, Validators.email]],
  //   ssn: ['', Validators.required],
  //   password: ['', [Validators.required, Validators.minLength(8)]],
  //   confirmPassword: ['', Validators.required],
  //   age: [0, [Validators.required, Validators.min(1), Validators.max(120)]],
  //   phoneNumber: [
  //     '',
  //     [Validators.required, Validators.pattern('^01[0125]09234307$')],
  //   ],
  //   street: ['', Validators.required],
  //   city: ['', Validators.required],
  //   governorate: ['', Validators.required],
  // },
  //     {
  //       validators: [Validation.match('password', 'confirmPassword')],
  //     }
  //   );
  // }
  // AddUser() {
  //   if (this.registerForm.valid) {
  //     this.AddingUser.addUser(this.registerForm.value).subscribe({
  //       next: (data: any) => {
  //         console.log(data);
  //         // Redirect to login
  //         //this.router.navigate(['/login']); // Adjust the path as necessary
  //       },
  //       error: (err) => {
  //         console.log(err);
  //         // Display an alert for the error
  //         window.alert(
  //           'An error occurred while registering. Please try again.'
  //         );
  //       },
  //     });
  //   }
  // }
  // //===========================================================//
  // get NameValid() {
  //   return this.registerForm.controls['firstName'].valid;
  // }
  // get LastNameValid() {
  //   return this.registerForm.controls['lastName'].valid;
  // }
  // get EmailValid() {
  //   return this.registerForm.controls['email'].valid;
  // }
  // get SsnValid() {
  //   return this.registerForm.controls['ssn'].valid;
  // }
  // get PasswordValid() {
  //   return this.registerForm.controls['password'].valid;
  // }
  // get ConfirmPasswordValid() {
  //   return this.registerForm.controls['confirmPassword'].valid;
  // }
  // get AgeValid() {
  //   return this.registerForm.controls['age'].valid;
  // }
  // get PhoneNumberValid() {
  //   return this.registerForm.controls['phoneNumber'].valid;
  // }
  // get StreetValid() {
  //   return this.registerForm.controls['street'].valid;
  // }
  // get CityValid() {
  //   return this.registerForm.controls['city'].valid;
  // }
  // get GovernorateValid() {
  //   return this.registerForm.controls['governorate'].valid;
  // }

  form: FormGroup = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    email: new FormControl(''),
    ssn: new FormControl(''),
    password: new FormControl(''),
    confirmPassword: new FormControl(''),
    age: new FormControl(0),
    phoneNumber: new FormControl(''),
    street: new FormControl(''),
    city: new FormControl(''),
    governorate: new FormControl(''),
  });
  submitted = false;

  constructor(
    private router: Router,
    private AddingUser: UsersService,
    private formBuilder: FormBuilder
  ) {
    //localStorage.setItem('Outstate', JSON.stringify({ Outstate: true }));
  }

  ngOnInit(): void {
    this.form = this.formBuilder.group(
      {
        firstName: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20),
            Validators.pattern('^[a-zA-Z]+$'), // Only allow alphabetic characters
          ],
        ],
        lastName: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20),
            Validators.pattern('^[a-zA-Z]+$'), // Only allow alphabetic characters
          ],
        ],
        email: ['', [Validators.required, Validators.email]],
        ssn: [
          '',
          [
            Validators.required,
            Validators.minLength(14),
            Validators.maxLength(14),
            Validators.pattern(/^\d{14}$/),
          ],
        ],
        // Ensure exactly 14 digits
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(40),
          ],
        ],
        confirmPassword: ['', Validators.required],
        age: [
          0,
          [
            Validators.required,
            Validators.min(18),
            Validators.max(120),
            Validators.pattern('^[0-9]+$'),
          ],
        ],
        phoneNumber: [
          '',
          [Validators.required, Validators.pattern('^(010|011|012)[0-9]{8}$')],
        ],
        street: [
          '',
          [
            Validators.required,
            Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9 ]+$'),
          ],
        ],
        // Alphanumeric characters with at least one alphabetic character
        city: [
          '',
          [
            Validators.required,
            Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9 ]+$'),
          ],
        ],
        governorate: [
          '',
          [
            Validators.required,
            Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9 ]+$'),
          ],
        ],
      },

      {
        validators: [
          PasswordValidationComponent.match('password', 'confirmPassword'),
        ],
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    console.log(JSON.stringify(this.form.value, null, 2));
  }

  onReset(): void {
    this.submitted = false;
    this.form.reset();
  }

  error: any;
  AddUser() {
    if (this.form.valid) {
      this.AddingUser.addUser(this.form.value).subscribe({
        next: (data: any) => {
          //console.log(data);
          if (data.error) {
            window.alert(data.error);
            //this.error = data.error;
            this.onReset();
          } else {
            // Redirect to login
            //this.router.navigate(['/login']); // Adjust the path as necessary
            this.router.navigateByUrl('/login');
          }

          // Redirect to login
          // this.router.navigate(['/l']); // Adjust the path as necessary
        },
        error: (err) => {
          console.log(err);
          // Display an alert for the error
          window.alert(
            'An error occurred while registering. Please try again.'
          );
        },
      });
    }
  }
}
