
import { Component } from '@angular/core';
import {   FormsModule, ReactiveFormsModule} from '@angular/forms';
import { ContactService } from '../../services/contact.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { UsersService } from '../../services/users.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [ FormsModule, ReactiveFormsModule,CommonModule,HttpClientModule],
 providers:[ContactService,UsersService],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent {
  currentID:any
currentUser:any
fullname="";
phone:any;
email="";
message="";
 constructor(private usersService:ContactService, private users_service: UsersService, private router: Router)
 {
  const x= localStorage.getItem('userlogin')
  if(x ==null){
    this.router.navigateByUrl('/login')
    return;
  }
  const loginData= JSON.parse(x)
  console.log("Current_id :"+loginData.Current_id);
  
  if(!loginData.Login_state){
    this.router.navigateByUrl('/login')
    return;
  }
  if(loginData.Current_id<1){
    this.router.navigateByUrl('/login')
    return;
  }
  this.currentID=parseInt( loginData.Current_id)
 }

 ngOnInit(): void {
   
  this.users_service.getAuthUser(this.currentID).subscribe({
    next:(data)=>{
      this.currentUser = data;
      this.fullname=this.currentUser.firstName+" "+this.currentUser.lastName
      this.phone=this.currentUser.phoneNumber
      this.email=this.currentUser.email
    },
    error:(err)=>{console.log(err)}
  })
}

 Add(message: string) {
  // Check if the message is not empty
  if(this.fullname.length<5 ||this.fullname.trim() !== '' ) {
alert("Name must be at least 4 characters long")
    return;
  }
  if( (new RegExp('^(010|011|012)[0-9]{7}$')).test(this.phone.toString())){

    alert("Phone must be 11 number long and begains with (010 , 011 or 012) . ")
    return;
  }


  if(this.email.trim()==""|| !this.email.includes("@")){
    alert("Email must Contain @")
    return;
  }

  
  
  if (message.trim() !== '') {
    let currentDate = new Date().toISOString().split('T')[0];
    let newrev = {"Userid": this.currentID, "date": currentDate, "Comment": message};
    this.usersService.AddNewreview(newrev).subscribe();
  } else {
    
    alert("Cannot add an empty review.");
  }
}

  


  }