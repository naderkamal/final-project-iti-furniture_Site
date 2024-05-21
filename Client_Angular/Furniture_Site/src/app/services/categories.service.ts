import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService {
  private DB_Url = 'http://localhost:5277/api/Category';

  constructor(private client: HttpClient) {}

  getProductsByCategory(id: any) {
    console.log(id);
    return this.client.get(this.DB_Url + '/' + id);
  }

  getallCategory() {
    return this.client.get(this.DB_Url);
  }
}
