import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [ FormsModule ],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  @Input() oneproduct: any;
//---- for rating
  starsCount: number[] = [1, 2, 3, 4, 5];
 
//-----------
  constructor(private router: Router){
    
   
  }

  userlogin: any; // Initialize as an empty array
  
  ngOnInit(): void {
    const storeduser = localStorage.getItem('userlogin');
    if (storeduser) {
      this.userlogin = JSON.parse(storeduser); // Parse JSON string to object
    }
    else{
      this.router.navigateByUrl('/login')
    }   
  }
  toproductdetails(id:any):void{
    this.router.navigateByUrl('/productDetails/'+id)
  }

  savecartproducts():void{
    if (this.userlogin.Login_state == true) {     
      const existingCart = localStorage.getItem('cart');
      
      if (existingCart) {
        const cart = JSON.parse(existingCart);
      // debugger
        if (cart.hasOwnProperty(this.userlogin.Current_id)) {
          const userCartItems = cart[this.userlogin.Current_id];
          const productExists = userCartItems.some((item: any)  => item.id === this.oneproduct.id); // Ensure this.oneproduct is accessible here
      
          if (!productExists) {
            const updatedUserCart = [...userCartItems, { ...this.oneproduct, totalquantity: 1, totalprice: this.oneproduct.price }];
            cart[this.userlogin.Current_id] = updatedUserCart;
            localStorage.setItem('cart', JSON.stringify(cart));
          }
        } else {
          // If the user ID doesn't exist in the cart, create it
          cart[this.userlogin.Current_id] = [{...this.oneproduct, totalquantity: 1, totalprice: this.oneproduct.price }];
          localStorage.setItem('cart', JSON.stringify(cart));
        }
      } else {
        // If cart doesn't exist, create a new one with the user ID and the product
        const newCart = { [this.userlogin.Current_id]: [{...this.oneproduct, totalquantity: 1, totalprice: this.oneproduct.price }] };
        localStorage.setItem('cart', JSON.stringify(newCart));
      }
    }else{
      this.router.navigateByUrl('/login')
    }
  }
  savewishproducts():void{
    if (this.userlogin.Login_state == true) {     
      const existingwish = localStorage.getItem('wish');
      
      if (existingwish) {
        const wish = JSON.parse(existingwish);
      
        if (wish.hasOwnProperty(this.userlogin.Current_id)) {
          const userwishItems = wish[this.userlogin.Current_id];
          const productExists = userwishItems.some((item: any)  => item.id === this.oneproduct.id); // Ensure this.oneproduct is accessible here
      
          if (!productExists) {
            const updatedUserwish = [...userwishItems, { ...this.oneproduct}];
            wish[this.userlogin.Current_id] = updatedUserwish;
            localStorage.setItem('wish', JSON.stringify(wish));
          }
        } else {
          // If the user ID doesn't exist in the wish, create it
          wish[this.userlogin.Current_id] = [{...this.oneproduct}];
          localStorage.setItem('wish', JSON.stringify(wish));
        }
      } else {
        // If wish doesn't exist, create a new one with the user ID and the product
        const newwish = { [this.userlogin.Current_id]: [{...this.oneproduct}] };
        localStorage.setItem('wish', JSON.stringify(newwish));
      }
    }else{
      this.router.navigateByUrl('/login')
    }
}
}