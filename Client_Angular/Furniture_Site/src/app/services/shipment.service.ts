import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class shipmentService {
  private readonly db_URL = "http://localhost:5277/api/Shipment";

  constructor(private  shippment1 :HttpClient) { }
  getAll(){
    return this.shippment1.get(this.db_URL);
  }
  
  getOneByID(id:any){
    return this.shippment1.get(this.db_URL+"/"+id);
   
  }
  Add(sh:any){
    return this.shippment1.post(this.db_URL,sh);
  }
  Update(sh:any){
    return this.shippment1.put(this.db_URL+"/"+sh.id,sh);
  }
  Delete(id:any){
    return this.shippment1.delete(this.db_URL+"/"+id);
  }

}
