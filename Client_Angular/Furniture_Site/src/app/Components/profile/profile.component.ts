import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UsersService } from '../../services/users.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

interface IUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  age: number;
  street: string;
  city: string;
  governorate: string;
}

//replace the fackendpoint by real endpoint
//do the update logic

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [HttpClientModule, FormsModule, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
  styles: ``,
  providers: [UsersService],
})
export class ProfileComponent {
  user: IUser | null = null;
  originalUser: IUser | null = null; // New property to hold the original user data
  isEditableProfile = false;
  isEditableAddress = false;

  currentID: any;
  //locastoarge
  constructor(private getIdService: UsersService, private router: Router) {
    const x = localStorage.getItem('userlogin');
    if (x == null) {
      this.router.navigateByUrl('/login');
      return;
    }
    const loginData = JSON.parse(x);
    console.log('Current_id :' + loginData.Current_id);

    if (!loginData.Login_state) {
      this.router.navigateByUrl('/login');
      return;
    }
    if (loginData.Current_id < 1) {
      this.router.navigateByUrl('/login');
      return;
    }
    this.currentID = parseInt(loginData.Current_id);
  }
  ngOnInit(): void {
    if (this.currentID) {
      this.getIdService.getAuthUser(this.currentID).subscribe({
        next: (data: any) => {
          if (data.error) {
            window.alert(data.error);
            if (this.user) this.originalUser = { ...this.user };
          } else {
            this.originalUser = { ...data };
            this.user = { ...data };
          }
        },
        error: (err) => {
          console.log(err);
        },
      });
    }
  }
  openEditMode(model: 'address' | 'profile') {
    if (model === 'address') {
      this.isEditableAddress = true;
    } else {
      this.isEditableProfile = true;
    }
  }
  cancelEditMode(model: 'address' | 'profile') {
    if (this.originalUser && this.user) this.originalUser = { ...this.user };
    if (model === 'address') {
      this.isEditableAddress = false;
    } else {
      this.isEditableProfile = false;
    }
  }

  saveProfile() {
    this.isEditableProfile = false;
    this.isEditableAddress = false;
    if (this.user && this.originalUser) {
      this.getIdService.updateUser(this.originalUser).subscribe({
        next: (data: any) => {
          if (data.error) {
            window.alert(data.error);
            if (this.user) this.originalUser = { ...this.user };
          } else {
            this.originalUser = { ...data };
            this.user = { ...data };
          }
        },
        error: (err) => {
          if (this.user && this.originalUser)
            this.originalUser = { ...this.user };

          console.error(
            'An error occurred while updating your profile. Please try again later.',
            err
          );
        },
      });
    }
  }

  deleteUser() {
    this.getIdService.deleteUser(this.currentID).subscribe({
      next: (data: any) => {
        console.log(data);
        localStorage.setItem(
          'userlogin',
          JSON.stringify({ Current_id: -1, Login_state: false })
        );
        this.router.navigateByUrl('/login');
        return;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
  // refreshUserData() {
  //   if (this.Id) {
  //     this.getIdService.getAuthUser(this.Id).subscribe({
  //       next: (data: any) => {
  //         this.user = { ...data };
  //         this.originalUser = { ...data };
  //       },
  //       error: (err) => {
  //         console.log(err);
  //       },
  //     });
  //   }
  // }

  // Assuming you have a method in your service to update the user

  editProfile() {
    // Add logic for editing profile details
  }

  editShippingData() {
    // Add logic for editing shipping data
  }
  x: any;
  y: number = 3;
}
