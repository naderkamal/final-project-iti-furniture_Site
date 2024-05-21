import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ContactService {

  
   private readonly DB_URL = "http://localhost:5277/api/ReviewDetaild";

   constructor(private readonly myClient:HttpClient) { }
 
   AddNewreview(data:any){
     return this.myClient.post(this.DB_URL,data);
   }
}
