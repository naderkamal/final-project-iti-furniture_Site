
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
 
  cartItems: any; // Initialize as an empty array
  currentproduct:any;
  index:any;
  constructor(private router: Router) {

  }

  userlogin: any; // Initialize as an empty array
  totalpriceforall:number=0;

  ngOnInit(): void {
    // debugger
    const storeduser = localStorage.getItem('userlogin');
    if (storeduser) {
      this.userlogin = JSON.parse(storeduser); // Parse JSON string to object
    }
    const storedCart = localStorage.getItem('cart');
    if (storedCart) {
      const cart = JSON.parse(storedCart);
      const userCartItems = cart[this.userlogin.Current_id];
      this.cartItems = userCartItems; // Parse JSON string to object
      this.calculatetotalprice()
    }
  }
  calculatetotalprice(){
    this.totalpriceforall=0;
    for (const element of this.cartItems) {
      this.totalpriceforall+=element.totalprice;
      }
  }

  // plusindex:number=0;
  plus(event: any) {
    this.index=0;   
    // console.log(this.cartItems);
    if (event.totalquantity < event.stock){
      for (const element of this.cartItems) {
        if (element.id == event.id) {
          // debugger
          const newtotalquantity=++element.totalquantity;
          const newtotalprice=element.totalprice+element.price;
          this.cartItems[this.index]["totalquantity"]=newtotalquantity;
          this.cartItems[this.index]["totalprice"]=newtotalprice;
          const newCart = { [this.userlogin.Current_id]: this.cartItems };
          localStorage.setItem('cart', JSON.stringify(newCart));
          this.totalpriceforall+=element.price;
          break
          // console.log(this.cartItems);
          // debugger
        }
        else{
          this.index++;
        }
      }
    }
  }
  minus(event: any) {
    this.index=0;
    if (event.totalquantity > 1){
        this.cartItems.forEach((element: any) => {
          if (element.id == event.id) {
            // debugger
            this.cartItems[this.index]["totalquantity"]=--element.totalquantity;
            this.cartItems[this.index]["totalprice"]=element.totalprice-element.price;
            const newCart = { [this.userlogin.Current_id]: this.cartItems };
            localStorage.setItem('cart', JSON.stringify(newCart));
            this.totalpriceforall-=element.price;
          }
          else{
            this.index++;
          }
        });
      }
    }

    checkorder(){
      const newCart = {[this.userlogin.Current_id]: this.cartItems };
      localStorage.setItem('ordercheckout', JSON.stringify(newCart));
      localStorage.removeItem('cart');
      this.router.navigateByUrl('/checkout')
  }

  deleteRow(row: any) {
    const index = this.cartItems.findIndex((product:any) => product === row);
  
    if (index !== -1) {
      // Remove the item from the cartItems array
      this.cartItems.splice(index, 1);
      // Update the local storage data
      const newCart = {[this.userlogin.Current_id]: this.cartItems };
      localStorage.setItem('cart', JSON.stringify(newCart));
      this.calculatetotalprice()
    }
  }
}