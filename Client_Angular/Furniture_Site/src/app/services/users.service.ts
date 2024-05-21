import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
interface IUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  age: number;
  street: string;
  city: string;
  governorate: string;
}
interface IRegister {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  age: number;
  street: string;
  city: string;
  governorate: string;
  ssn: string;
  password: string;
}
// separated folder better
@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private DB_Url = 'http://localhost:5277/api/User';
  constructor(private client: HttpClient) {}

  getAuthUser(id: any) {
    console.log(id);
    return this.client.get(this.DB_Url + '/' + id);
  }
  updateUser(user: IUser) {
    // Assuming you have an endpoint to update the user's profile
    return this.client.put(this.DB_Url + '/' + user.id, user);
  }
  deleteUser(id: number) {
    return this.client.delete(this.DB_Url + '/' + id);
  }
  addUser(user: any) {
    return this.client.post(this.DB_Url, user);
  }
}
