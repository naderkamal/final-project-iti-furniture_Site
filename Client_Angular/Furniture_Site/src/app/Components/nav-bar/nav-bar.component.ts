import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [ RouterModule],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent {
// Initialization for ES Users
login_State=false;
User_Id :any
constructor(private router: Router){
  
  const x= localStorage.getItem('userlogin')
  if(x ==null){
    this.login_State=false;
    return
  }
  const loginData= JSON.parse(x)
  console.log("Current_id :"+loginData.Current_id);
  
  if(!loginData.Login_state){
    this.login_State=false;
    return;
  }
  if(loginData.Current_id<1){
    this.login_State=false;
    return;
  }
  this.login_State=loginData.Login_state;
this.User_Id=parseInt( loginData.Current_id)
// for test only
this.login_State=true

}
LogOut(){
  this.login_State=false
  localStorage.setItem('userlogin', JSON.stringify({ Current_id:-1,Login_state: false}));
  this.router.navigateByUrl('/login')
}

}
