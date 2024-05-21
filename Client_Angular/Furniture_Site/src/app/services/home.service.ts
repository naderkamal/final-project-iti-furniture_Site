import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private readonly db_URL = "http://localhost:5277/api/Product";
  private readonly productUser_URL = "http://localhost:5277/api/ProductUser";
  constructor(private  http :HttpClient) { }

  // Fetch all products
  getProducts() {
    return this.http.get(this.db_URL);
  }

  // Fetch top-rated products based on product ID
  getTopRatedProducts() {
    return this.http.get(this.productUser_URL+"/"+5);
  }
  getLastedProducts() {
    return this.http.get(this.db_URL);
  }

}
