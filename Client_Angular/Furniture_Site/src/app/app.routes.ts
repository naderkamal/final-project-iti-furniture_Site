import { Routes } from '@angular/router';
import { OrdersComponent } from './Components/orders/orders.component';
import { CheckOutComponent } from './Components/check-out/check-out.component';
import { HomeComponent } from './Components/home/home.component';
import { ProductsComponent } from './Components/products/products.component';
import { ProductDetailsComponent } from './Components/product-details/product-details.component';
import { AboutUsComponent } from './Components/about-us/about-us.component';
import { ContactComponent } from './Components/contact/contact.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { WishlistComponent } from './Components/wishlist/wishlist.component';
import { CartComponent } from './Components/cart/cart.component';
import { ErrorComponent } from './Components/error/error.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';

export const routes: Routes = [
   {path:"",component:HomeComponent},
    {path:"home",component:HomeComponent},
    {path:"orders",component:OrdersComponent},
    {path:"checkout",component:CheckOutComponent},
    {path:"products",component:ProductsComponent},
    {path:"productDetails/:id",component:ProductDetailsComponent},
    {path:"about",component:AboutUsComponent},
    {path:"contact",component:ContactComponent},
    {path:"profile",component:ProfileComponent},
    {path:"wish",component:WishlistComponent},
    {path:"cart",component:CartComponent},
    {path:"login",component:LoginComponent},
    {path:"register",component:RegisterComponent},
    {path:"**",component:ErrorComponent}
];
