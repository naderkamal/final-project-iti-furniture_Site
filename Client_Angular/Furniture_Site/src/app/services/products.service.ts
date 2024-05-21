import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  private DB_Url = 'http://localhost:5277/api/Product';

  constructor(private client: HttpClient) {}

  getOneProduct(id: any) {
    console.log(id);
    return this.client.get(this.DB_Url + '/' + id);
  }

  getAllProduct(){
    return this.client.get(this.DB_Url);
  }
  Update(pro:any){
    return this.client.put(this.DB_Url+"/"+pro.id,pro);
  }
}
