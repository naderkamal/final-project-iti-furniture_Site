import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly db_URL = 'http://localhost:5277/api/Auth';

  constructor(private Authentication: HttpClient) { }
  
  login(email: string, password: string) {
    return this.Authentication.post(`${this.db_URL}/login`, { email, password });
  }
}
