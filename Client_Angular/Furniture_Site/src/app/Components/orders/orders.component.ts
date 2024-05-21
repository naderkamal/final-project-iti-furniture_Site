import { Component, ElementRef, Version, ViewChild } from '@angular/core';
import { OrderDetailsComponent } from '../order-details/order-details.component';
import { ordersService } from '../../services/orders.service';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule, DatePipe } from '@angular/common';
import { shipmentService } from '../../services/shipment.service';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ordersProducts } from '../../services/ordersProducts.service';
import { ProductsService } from '../../services/products.service';
import { RatingComponent } from '../rating/rating.component';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [OrderDetailsComponent,HttpClientModule,CommonModule,FormsModule,ReactiveFormsModule,RatingComponent],
  providers:[ordersService,shipmentService,ordersProducts,ProductsService],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {
  checkform = this.formBuilder.group({
    state_active: false
  }); 
  @ViewChild('ordersBody') mydiv:any; 
  state_Txt="Orders in Progress"
  //Waiting for Acception
  state=false
  userId :any
  allOrders :any []=[]
  complatedOrders :any []=[]
  complatedCount = 0
  progressOrders :any []=[]
  progressCount = 0
  displayOrders :any []=[]
  oneOrder:any
  allShipments:any []=[]
  oneShipment:any
  OrderProducts:any []=[]

  constructor( private orders_service: ordersService,private shipment_service : shipmentService 
    ,private ordersProducts_service :ordersProducts ,private products_service: ProductsService ,private formBuilder: FormBuilder ,
    private elementRef: ElementRef<HTMLElement> , private router: Router )
  {
   
    const x= localStorage.getItem('userlogin')
    if(x ==null){
      this.router.navigateByUrl('/login')
      return;
    }
    const loginData= JSON.parse(x)
    console.log("Current_id :"+loginData.Current_id);
    
    if(!loginData.Login_state){
      this.router.navigateByUrl('/login')
      return;
    }
    if(loginData.Current_id<1){
      this.router.navigateByUrl('/login')
      return;
    }
    this.userId=parseInt( loginData.Current_id)

//localStorage.setItem('userlogin', JSON.stringify({ Current_id: 1,Login_state: true}));
  
  }
  ngOnInit(): void {
    this.getData();
  }
  // get page data 
  getData(){
   console.log("user id :"+this.userId);
   
    this.orders_service.getByUserID( this.userId.toString()).subscribe({
      next:(data)=>{
        this.allOrders.push( ...(data as any[]));
        console.log("allOrders :"+this.allOrders.join(","));
       this.FiltrOrder();
      },
      error:(err)=>{console.log(err)}
    });
    this.shipment_service.getAll().subscribe({
      next:(data)=>{
        this.allShipments = data as any[];
      },
      error:(err)=>{console.log(err)}
    });
    this.FiltrOrder();
  }
 // filter all orders 
 FiltrOrder() {
  this.complatedOrders=[]
  this.progressOrders=[]
    for (const o of this.allOrders ){
      if(o.statue=== "complate"){
        this.complatedOrders.push(o);
        this.complatedCount=this.complatedOrders.length
       
      }
      else{
        this.progressOrders.push(o);
        this.progressCount=this.progressOrders.length
      }
   }
   this.displayOrders=this.progressOrders
console.log("displayOrders : "+this.displayOrders);
  }
  getComplated(){
    
    this.displayOrders =[]
  // this.mydiv.nativeElement.innerHTML=`getComplated`
    this.FiltrOrder(); 
    this.state=true
    this.state_Txt="Complated Orders"
    this.displayOrders=this.complatedOrders
    console.log(this.displayOrders);
    
  }
  getProgress(){
    this.displayOrders.splice(0);
    //this.mydiv.nativeElement.innerHTML=`getProgress`
    this.FiltrOrder(); 
    this.state=false
    this.state_Txt="Orders in Progress"
    this.displayOrders=this.progressOrders
    console.log(this.displayOrders);
  }
// change filter display
  onButtonGroupClick($event:any){
    let clickedElement = $event.target || $event.srcElement;

    if( clickedElement.nodeName === "BUTTON" ) {

      let isCertainButtonAlreadyActive = clickedElement.parentElement.querySelector(".active");
      // if a Button already has Class: .active
      if( isCertainButtonAlreadyActive ) {
        isCertainButtonAlreadyActive.classList.remove("active");
      }

      clickedElement.className += " active";
    }

  }
  // ensure Order dilivary
  changeStateComplete(id:any){

   let  Order :any
   let  position =-1
   this.progressOrders.forEach((element,index) => { 
   if(element.id===id){
   Order=element
   position =index
   }
   });
    
   console.log("o_id :"+id);
   console.log("orders:"+Order.statue);
   console.log("index :"+position);
   if(position>=0){
    this.progressOrders.splice(position,1);
  }
   
   this.displayOrders=this.progressOrders
   Order.statue="complate"
   console.log("orders:"+Order.statue);
   this.complatedOrders.push(Order);
   if(Order!=null){
    this.orders_service.Update(Order).subscribe()
      }
  }
  // delete Order 
  CancelOrder(id:any){
    let  Order :any
    let  position =-1
    this.progressOrders.forEach((element,index) => { 
    if(element.id===id){
    Order=element
    position =index
    }
    });
     
    console.log("o_id :"+id);
    console.log("orders:"+Order.statue);
    console.log("index :"+position);
    if(position>=0){
      this.progressOrders.splice(position,1);
      this.changeProductsStock(id);
      this.orders_service.Delete(id).subscribe()
    
    }
    this.progressCount=this.progressOrders.length
    this.complatedCount=this.complatedOrders.length
   }
   getDetails(id:any){
    const detaislDiv =document.getElementById('details_'+id) as HTMLHtmlElement
    const detaislBtn =document.getElementById('detailBtn_'+id) as HTMLHtmlElement
    
    console.log(  detaislDiv.innerHTML)
    if(detaislDiv.style.display=="block"){
      detaislDiv.style.display="none";
      detaislBtn.style.width="70px"
      detaislBtn.innerText="Details"

    }else{
      detaislDiv.style.display="block";
      detaislBtn.style.width="120px"
      detaislBtn.innerText="Hide Details"
    }
    
   }
   getrateing(id:any){
    const RatingDiv =document.getElementById('rate_'+id) as HTMLHtmlElement
    const ratingBtn =document.getElementById('ratingBtn_'+id) as HTMLHtmlElement
    
    console.log(  RatingDiv.innerHTML)
    if(RatingDiv.style.display=="block"){
      RatingDiv.style.display="none";
      ratingBtn.style.width="70px"
      ratingBtn.innerText="Rating"

    }else{
      RatingDiv.style.display="block";
      ratingBtn.style.width="120px"
      ratingBtn.innerText="Hide Rating"
    }
    
   }
   changeProductsStock(id:any){
    let ProductofOrder:any
    this.ordersProducts_service.getByOrderID( id).subscribe({
      next:(data)=>{
        this.OrderProducts.push( ...(data as any[]));
        for (const item of this.OrderProducts) {
          this.products_service.getOneProduct(item.productId).subscribe({
            next:(data)=>{
              ProductofOrder=data ;
              ProductofOrder.stock+=item.count
              this.products_service.Update(ProductofOrder).subscribe();

            console.log("item"+item.productId);
    
            },
            error:(err)=>{console.log(err)}
          });
          
        }
      },
      error:(err)=>{console.log(err)}
    });

  }


   FormatDate(date:Date):any{
    return  (new DatePipe('en-IN')).transform( date,'dd-MM-yyyy')?.toString() ;
   }
   
}
