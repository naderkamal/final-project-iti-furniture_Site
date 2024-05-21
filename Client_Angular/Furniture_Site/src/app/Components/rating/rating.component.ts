import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ProductsService } from '../../services/products.service';
import { ordersProducts } from '../../services/ordersProducts.service';
import { userProducts } from '../../services/userProducts.service';

@Component({
  selector: 'app-rating',
  standalone: true,
  imports: [ RouterModule,CommonModule],
  providers:[ordersProducts,ProductsService,userProducts],
  templateUrl: './rating.component.html',
  styleUrl: './rating.component.css'
})
export class RatingComponent {
  @Input() order_ID:any;
  @Input() user_ID:any;
  OrderProducts :any []=[]
  UserProducts :any []=[]
  Products :any []=[]
  counter=1
rates: number[] = [1, 2, 3, 4, 5];
rating:any
  constructor( private ordersProducts_service :ordersProducts ,private Productd_service :ProductsService,private userProducts_service :userProducts)
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
    console.log("order item"+item.productId);
    // getting user product rating
    this.userProducts_service.getOne(this.user_ID,item.productId).subscribe({
      next:(data)=>{
        this.UserProducts.push(data );
console.log("user item"+item.productId);

      },
      error:(err)=>{console.log(err)}
    });

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
  // ------------rating 
   rate(product_id:any,rate_value:any) {
// fill star rate
console.log("Fill star rate "+rate_value);
for (const i of this.rates) {

  if(i<=rate_value){
    const star =document.getElementById('or'+this.order_ID+'_pro'+product_id+'_rate'+i) as HTMLHtmlElement
    star.setAttribute("src" , "../../../assets/Images/Filled_star.png");
  }else{
    const star =document.getElementById('or'+this.order_ID+'_pro'+product_id+'_rate'+i) as HTMLHtmlElement
    star.setAttribute("src" , "../../../assets/Images/empty_star.png");
  }
  }

    //save rating
     console.log("save rate1111 "+rate_value);
      
        let found=false
        let newP:any
        for (const up of this.UserProducts) {
          if(product_id==up.productId){
            found=true
            up.ratingStar=rate_value
            console.log("rating search");
            
            this.userProducts_service.Update(up).subscribe();
          }else{
            newP=up
          }

        }
        if(!found){
          newP={userId:this.user_ID,productId:product_id,ratingStar:rate_value}
          this.userProducts_service.Add(newP).subscribe({
            next:(data)=>{
             
                console.log("rating ok ok");
                console.log(data);
      
            },
            error:(err)=>{
              console.log("rating not xxxxxxxxxxx  ok");
              console.log(err)
            
            }
          })
          
        }

}
getRate(id:any):number{

  let found=false
  
  for (const up of this.UserProducts) {
    if(id==up.productId){
      found=true
     return  up.ratingStar
    }

  }
 
    return 0
  
 
}
}
