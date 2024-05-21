import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FilterService {
  private readonly db_URL = 'http://localhost:5277/api/Filter';

  constructor(private Filteration: HttpClient) { }

  filterbyid(id: any) {
    return this.Filteration.get(this.db_URL + '/' + id);
  }

  filterbyprice(start: any, end: any) {
    return this.Filteration.get(this.db_URL + '/' + start + "," + end);
  }
}
