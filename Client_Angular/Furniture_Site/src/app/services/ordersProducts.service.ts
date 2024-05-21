import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ordersProducts {
  private readonly db_URL = "http://localhost:5277/api/ProductOrders";

  constructor(private  orderPruduct :HttpClient) { }
  getAll(){
    return this.orderPruduct.get(this.db_URL);
  }
  getOne(Order_id:any,  Product_id:any){
    return this.orderPruduct.get(this.db_URL+"/"+Order_id+"/"+Product_id);
   
  }
  getByOrderID(Order_id:number){
    return this.orderPruduct.get(this.db_URL+"/"+Order_id);
   
  }
  Add(orderPruduct:any){
    return this.orderPruduct.post(this.db_URL,orderPruduct);
  }
  Update(orderPruduct:any){
    return this.orderPruduct.put(this.db_URL+"/"+orderPruduct.orderId+"/"+orderPruduct.productId,orderPruduct);
  }
  Deleteone(Order_id:any,  Product_id:any){
    return this.orderPruduct.delete(this.db_URL+"/"+Order_id+"/"+Product_id);
  }
  DeletebyOrderID(Order_id:any){
    return this.orderPruduct.delete(this.db_URL+"/"+Order_id);
  }
  DeletebyProductID(Product_id:any){
    return this.orderPruduct.delete(this.db_URL+"/Pr/"+Product_id);
  }

}
