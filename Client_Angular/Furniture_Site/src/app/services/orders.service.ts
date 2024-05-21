import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ordersService {
  private readonly db_URL = "http://localhost:5277/api/Order";

  constructor(private  order1 :HttpClient) { }
  getAll(){
    return this.order1.get(this.db_URL);
  }
  
  getOneByID(id:any){
    return this.order1.get(this.db_URL+"/"+id);
   
  }
  getByUserID(User_id:string){
    return this.order1.get(this.db_URL+"/UserID/"+User_id);
   
  }
  Add(or:any){
    return this.order1.post(this.db_URL,or);
  }
  Update(or:any){
    return this.order1.put(this.db_URL+"/"+or.id,or);
  }
  Delete(id:any){
    return this.order1.delete(this.db_URL+"/"+id);
  }

}
