import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,FormsModule,HttpClientModule],
  providers:[AuthService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) { 

    localStorage.setItem('Outstate', JSON.stringify({ Outstate: true}));
  }

  login(): void {
    this.authService.login(this.email, this.password).subscribe(
        {
          next:(data:any)=>{
            localStorage.setItem('userlogin', JSON.stringify({ Current_id: data.id,Login_state: true}));
            localStorage.setItem('Outstate', JSON.stringify({ Outstate: false}));
            console.log(data)
            window.location.replace('/home')
            // this.router.navigate(['home']);
          },
          error:(err)=>{console.log('Login failed:',err)
           alert("Login failed")
        }
          
        }
      );
    }
}
