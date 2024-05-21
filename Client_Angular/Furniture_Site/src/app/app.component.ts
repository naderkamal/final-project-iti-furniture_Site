import { Component } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { NavBarComponent } from './Components/nav-bar/nav-bar.component';
import { FooterComponent } from './Components/footer/footer.component';
import { HomeComponent } from './Components/home/home.component';
import { OrdersComponent } from './Components/orders/orders.component';
import { ProductDetailsComponent } from './Components/product-details/product-details.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { RegisterComponent } from './Components/register/register.component';
import { AboutUsComponent } from './Components/about-us/about-us.component';
import { LoginComponent } from './Components/login/login.component';
import { ProductsComponent } from './Components/products/products.component';
import { WishlistComponent } from "./Components/wishlist/wishlist.component";
import { CartComponent } from "./Components/cart/cart.component";
import { RatingComponent, RatingModule } from '@syncfusion/ej2-angular-inputs';



@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet,
        RouterModule,
        NavBarComponent,
        FooterComponent,
        HomeComponent,
        RatingModule,
        OrdersComponent,
        ProductDetailsComponent,
        ProfileComponent,
        RegisterComponent,
        AboutUsComponent,
        LoginComponent,
        ProductsComponent, WishlistComponent, CartComponent]
})

export class AppComponent {
  title = 'Furniture Site';
  
  constructor(private router: Router){
   
    
  }
}
