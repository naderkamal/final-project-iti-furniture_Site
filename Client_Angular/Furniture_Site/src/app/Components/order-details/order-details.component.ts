import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ordersProducts } from '../../services/ordersProducts.service';
import { ProductsService } from '../../services/products.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-details',
  standalone: true,
  imports: [ RouterModule,CommonModule],
  providers:[ordersProducts,ProductsService],
  templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.css'
})
export class OrderDetailsComponent {
  @Input() order_ID:any;
  OrderProducts :any []=[]
  Products :any []=[]
  counter=1

  constructor( private ordersProducts_service :ordersProducts ,private Productd_service :ProductsService)
  {
   
    
  }

  ngOnInit(): void {
    this.ordersProducts_service.getByOrderID( this.order_ID).subscribe({
      next:(data)=>{
        this.OrderProducts.push( ...(data as any[]));
        for (const item of this.OrderProducts) {
          this.Productd_service.getOneProduct(item.productId).subscribe({
            next:(data)=>{
              this.Products.push(data );
    console.log("item"+item.productId);
    
            },
            error:(err)=>{console.log(err)}
          });
          
        }
      },
      error:(err)=>{console.log(err)}
    });
  }


  getProuducts(){
   
  }

  getSelectedProduct(id:number):any{
    this.Products.forEach(element => {
      if(element.id==id){
        return element;
      }
      return 0;
    });
  }
}
