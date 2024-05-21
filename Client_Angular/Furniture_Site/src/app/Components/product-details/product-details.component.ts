import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { ProductsService } from '../../services/products.service';
import { CategoriesService } from '../../services/categories.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CardComponent } from '../card/card.component';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [HttpClientModule, CardComponent],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.css',
  providers: [ProductsService, CategoriesService],
})
export class ProductDetailsComponent {
  images: any[] = [];
  imgActiveIndex = 0;
  num = 1;
  stock = 1;
  product: any;
  Id = 2;
  category = 'chair';
  //======//
  relatedProducts: any;
  userlogin: any;
  starsCount: number[] = [1, 2, 3, 4, 5];
  constructor(
    private router: Router,
    private getCategoryProducts: CategoriesService,
    private getProductService: ProductsService,
    myActivated: ActivatedRoute
  ) {
    this.Id = myActivated.snapshot.params['id'];
    //const authUser = localStorage.getItem('user');
    // if (authUser) {
    //   this.Id = Number(JSON.parse(authUser).id);
    // }
  }

  setImageActive(index: number) {
    this.imgActiveIndex = index;
  }

  plus() {
    if (this.num < this.stock) this.num++;
  }
  minus() {
    if (this.num > 1) this.num--;
  }
  ngOnInit(): void {
    const storeduser = localStorage.getItem('userlogin');
    if (storeduser) {
      this.userlogin = JSON.parse(storeduser); // Parse JSON string to object
    } else {
      this.userlogin = null;
    }

    if (this.Id) {
      this.getProductService.getOneProduct(this.Id).subscribe({
        next: (data: any) => {
          console.log(data);
          this.product = data;
          this.stock = this.product.stock;
          this.images.push(
            this.product.image,
            this.product.image,
            this.product.image
          );

          // Now that this.product is set, you can safely call the second service
          this.getCategoryProducts
            .getProductsByCategory(this.product.categoryName)
            .subscribe({
              next: (data: any) => {
                console.log(data);
                // Assuming you want to set relatedProducts here
                this.relatedProducts = data.products;
                console.log(this.relatedProducts);
              },
              error: (err) => {
                console.log(err);
              },
            });
        },
        error: (err) => {
          console.log(err);
        },
      });
    }
  }

  addZoomEffect() {
    document.querySelector('.card')?.classList.add('zoomed');
  }

  removeZoomEffect() {
    document.querySelector('.card')?.classList.remove('zoomed');
  }
  addToCart(product: any) {
    if (this.userlogin) {
      const existingCart = localStorage.getItem('cart');
      if (existingCart) {
        const cart = JSON.parse(existingCart);
        // debugger
        if (cart.hasOwnProperty(this.userlogin.Current_id)) {
          const userCartItems = cart[this.userlogin.Current_id];
          const productExists = userCartItems.some(
            (item: any) => item.id === product.id
          ); // Ensure this.oneproduct is accessible here
          if (!productExists) {
            const updatedUserCart = [
              ...userCartItems,
              {
                ...product,
                totalquantity: this.num,
                totalprice: this.num * product.price,
              },
            ];
            cart[this.userlogin.Current_id] = updatedUserCart;
            localStorage.setItem('cart', JSON.stringify(cart));
          }
        } else {
          // If the user ID doesn't exist in the cart, create it
          cart[this.userlogin.Current_id] = [
            {
              ...product,
              totalquantity: this.num,
              totalprice: this.num * product.price,
            },
          ];
          localStorage.setItem('cart', JSON.stringify(cart));
        }
      } else {
        // If cart doesn't exist, create a new one with the user ID and the product
        const newCart = {
          [this.userlogin.Current_id]: [
            {
              ...product,
              totalquantity: this.num,
              totalprice: product.price * this.num,
            },
          ],
        };
        localStorage.setItem('cart', JSON.stringify(newCart));
      }
    } else {
      this.router.navigateByUrl('/login');
    }
    const cart = localStorage.getItem('cart');
    if (cart) {
      const cartProducts = JSON.parse(cart);
      product.quantity = this.num;
      cartProducts.push(product);
      localStorage.setItem('cart', JSON.stringify(cartProducts));
    } else {
      product.quantity = this.num;
      localStorage.setItem('cart', JSON.stringify([product]));
    }
  }
}

// relatedProducts = [
//   {
//     id: 1,
//     productName: 'chair1',
//     productDescription: 'Comfort',
//   },
//   {
//     id: 2,
//     productName: 'chair1',
//     productDescription: 'Comfort',
//   },
//   {
//     id: 3,
//     productName: 'chair1',
//     productDescription: 'Comfort',
//   },
//   {
//     id: 4,
//     productName: 'chair1',
//     productDescription: 'Comfort',
//   },
// ];
