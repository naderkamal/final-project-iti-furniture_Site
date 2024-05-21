import { CommonModule } from '@angular/common';
import { Component, Input, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-wishlist',
  standalone: true,
  imports: [CommonModule, FormsModule
  ],
  templateUrl: './wishlist.component.html',
  styleUrl: './wishlist.component.css'
})
export class WishlistComponent {
  cartItems: any; // Initialize as an empty array

  constructor(private router: Router) {

  }

  userlogin: any; // Initialize as an empty array


  ngOnInit(): void {
    // debugger
    const storeduser = localStorage.getItem('userlogin');
    if (storeduser) {
      this.userlogin = JSON.parse(storeduser); // Parse JSON string to object
    }
    else{
      this.router.navigateByUrl('/login')
    }
    const storedCart = localStorage.getItem('wish');
    if (storedCart) {
      const cart = JSON.parse(storedCart);
      const userCartItems = cart[this.userlogin.Current_id];
      this.cartItems = userCartItems; // Parse JSON string to object
    }
  }

  addToCart(event: any): void {
    if (this.userlogin.Login_state == true) {
      const existingCart = localStorage.getItem('cart');

      if (existingCart) {
        const cart = JSON.parse(existingCart);

        if (cart.hasOwnProperty(this.userlogin.Current_id)) {
          // debugger
          const userCartItems = cart[this.userlogin.Current_id];
          const userCartItemss=userCartItems;
          const productExists = userCartItemss.some((item: any) => item.id === parseInt(event.target.id, 10)); // Ensure this.oneproduct is accessible here
          // debugger
          if (!productExists) {
            this.cartItems.forEach((element: any) => {
              if (element.id == parseInt(event.target.id, 10)) {
                const updatedUserCart = [...userCartItems, { ...element, totalquantity: 1, totalprice: element.price }];
                const updatedUserCartnn=updatedUserCart;
                cart[this.userlogin.Current_id] = updatedUserCartnn;
                localStorage.setItem('cart', JSON.stringify(cart));
                this.deleteRow(element);

              }
            });
          }
        } else {
          // If the user ID doesn't exist in the cart, create it
          this.cartItems.forEach((element: any) => {
            if (element.id == event.target.id) {
              const newCart = { [this.userlogin.Current_id]: [{ ...element, totalquantity: 1, totalprice: element.price }] };
              localStorage.setItem('cart', JSON.stringify(newCart));
              this.deleteRow(element);
            }
          });
        }
      } else {
        // If cart doesn't exist, create a new one with the user ID and the product
        this.cartItems.forEach((element: any) => {
          if (element.id == parseInt(event.target.id, 10)) {
            const newCart = { [this.userlogin.Current_id]: [{ ...element, totalquantity: 1, totalprice: element.price }] };
            localStorage.setItem('cart', JSON.stringify(newCart));
            this.deleteRow(element);
          }
        });
      }
    } else {
      this.router.navigateByUrl('/login')
    }
  }

  /////////////
  deleteRow(row: any) {
    const index = this.cartItems.findIndex((product: any) => product === row);

    if (index !== -1) {
      // Remove the item from the cartItems array
      this.cartItems.splice(index, 1);
      // Update the local storage data
      const newCart = { [this.userlogin.Current_id]: this.cartItems };
      localStorage.setItem('wish', JSON.stringify(newCart));
    }
  }
}
