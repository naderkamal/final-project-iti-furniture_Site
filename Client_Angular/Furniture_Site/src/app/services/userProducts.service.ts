import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class userProducts {
  private readonly db_URL = "http://localhost:5277/api/ProductUser";

  constructor(private  orderPruduct :HttpClient) { }
  
  getOne(user_id:any,  Product_id:any){
    return this.orderPruduct.get(this.db_URL+"/"+user_id+"/"+Product_id);
   
  }

  Add(userPruduct:any){
    return this.orderPruduct.post(this.db_URL,userPruduct);
  }
  Update(userPruduct:any){
    return this.orderPruduct.put(this.db_URL+"/"+userPruduct.userId+"/"+userPruduct.productId,userPruduct);
  }
 

}
