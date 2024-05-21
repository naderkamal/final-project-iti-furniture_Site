import { CommonModule, DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { shipmentService } from '../../services/shipment.service';
import { HttpClientModule } from '@angular/common/http';
import { UsersService } from '../../services/users.service';
import { Router } from '@angular/router';
import { ordersService } from '../../services/orders.service';
import { ordersProducts } from '../../services/ordersProducts.service';
import { ProductsService } from '../../services/products.service';

@Component({
  selector: 'app-check-out',
  standalone: true,
  imports: [ CommonModule,ReactiveFormsModule,FormsModule,HttpClientModule],
  providers:[
    UsersService ,shipmentService,ordersService , ordersProducts,ProductsService
  ],
  templateUrl: './check-out.component.html',
  styleUrl: './check-out.component.css'
})
export class CheckOutComponent {
  alertState=false
  WarrningState=false
ShipingDate:any; 
shipingDays=5;
currentID:any
addedShipmentId=0
addedOrderId=0
currentUser:any
OrderData:any [] =[]
UserOrder :any
shipingCost=300
Total=0
fname = ""
lname = ""
phone=0
email=""
address=""
moreDetails=""
constructor( private orderProducts_service: ordersProducts,private order_service: ordersService, private shipment_service: shipmentService,
  private users_service: UsersService,private products_service: ProductsService, private router: Router){
  this.getShipingDate();
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
  this.currentID=parseInt( loginData.Current_id)
  const LocalStorageUserOrder=localStorage.getItem('ordercheckout')
  if(LocalStorageUserOrder ==null){
   
    return;
  }
   this.OrderData= JSON.parse(LocalStorageUserOrder)
  console.log("Order data : "+this.OrderData);
 
   this.UserOrder=this.OrderData[this.currentID.toString()]
   if (this.UserOrder!==null){
    for (const i of this.UserOrder) {
      this.Total+=i.totalprice
    }
   }

}

ngOnInit(): void {
   
  this.users_service.getAuthUser(this.currentID).subscribe({
    next:(data)=>{
      this.currentUser = data;
      this.fname=this.currentUser.firstName
      this.lname=this.currentUser.lastName
      this.phone=this.currentUser.phoneNumber
      this.email=this.currentUser.email
      this.address=this.currentUser.governorate+" - "+this.currentUser.city+" - "+this.currentUser.street
    },
    error:(err)=>{console.log(err)}
  })
}
getShipingDate(){
  let newDate = (new Date()).getDate()+this.shipingDays;
  this.ShipingDate=(new DatePipe('en-IN')).transform( (new Date()).setDate(newDate),'dd-MM-yyyy') ;
}
placeOrder(){
  
let stock=true
 if(this.UserOrder.length>0||this.UserOrder!=null){
  if((this.fname.trim()==""||this.fname.length<3||this.fname.length>20)||(this.lname.trim()==""||this.lname.length<3||this.lname.length>20))
  {return;}
  if( (new RegExp('^(010|011|012)[0-9]{7}$')).test(this.phone.toString())){

    alert("Phone must be 11 number long and begains with (010 , 011 or 012) . ")
    return;
  }


  if(this.email.trim()==""|| !this.email.includes("@")){
    alert("Email must Contain @")
    return;
  }

  this.WarrningState=false
  let ProductforStock:any
  for (const i of this.UserOrder) {
    console.log("+++/////checkkkkkkkkkkkkkkkkkkkkkkkkk/////////")
    this.products_service.getOneProduct(i.id).subscribe({
      next:(data)=>{
        console.log("  Product : data  : ");
        console.log(data);
        ProductforStock=data
      

        if(ProductforStock.stock<i.totalquantity){
          stock=false

          alert(" Stock of Product " +i.name +" is les than what you have order ?")
          console.log(" Stock of Product " +i.name +" is les than what you have order ?");
          return;
        }
       
      },
      error:(err)=>{
        console.log(" product stock error ")
        console.log(err);
      }
    });
   
   }
if (stock==true){
  console.log(" Stock of Product okokokokokok ")
  this.getShipingDate();
  // shipment to place in order
  this.add_Shipment()
  console.log("111111111")
  localStorage.removeItem('ordercheckout');
  this.alertState=true
  
  setTimeout(() => {
    this.alertState=false
    this.router.navigateByUrl('/orders')
  }, 2000);

}
  
}else{
  this.WarrningState=true

}
  //npm install @syncfusion/ej2-angular-inputs --save


}



add_Shipment(){
  let ship :any
 let shipment={isCompleted:false,cost:this.shipingCost,date: new Date() ,address:this.address,moreDetails:this.moreDetails}
  this.shipment_service.Add(shipment).subscribe({
    next:(data)=>{
      console.log("shi   : data  : ");
      console.log(data);
       ship=data
       this.addedShipmentId = ship.id
      
        console.log("00000000000 : add_Order()");
        // order to place in server
        this.add_Order()
     
       
     
    },
    error:(err)=>{console.log(err)}
  });
  
  console.log("0000000222222222");
  
}
add_Order(){
  let ord :any
  let order={phoneNumber:this.phone.toString(),statue:"panding",totalPrice:this.Total,orderDate:new Date(),shipmentID:this.addedShipmentId,userID:this.currentID }
  console.log("2222222222")
  this.order_service.Add(order).subscribe({
    next:(data)=>{
      console.log("ord   : data  : ");
      console.log(data);
      ord=data
      this.addedOrderId = ord.id
      
        console.log("00000001111111 : add_OrderProducts()");
//- place products to order
      this.add_OrderProducts()
      this.changeProductsStock()
   
      
    },
    error:(err)=>{
      console.log("Order error : ")
      console.log(err);
      
    }
  });
  console.log("3333333333")
  
  
}

add_OrderProducts(){
  console.log("++++++++++++")
 for (const i of this.UserOrder) {
  console.log("Orderrrrrrrrr+++ id : "+this.addedOrderId);
  
  let orderProduct={orderId:this.addedOrderId,productId:i.id,itemPrice:i.price,count:i.totalquantity }
  console.log("+++//////////////")
  this.orderProducts_service.Add(orderProduct).subscribe({
    next:(data)=>{
      console.log("ord  Produ : data  : ");
      console.log(data);
     
      
    },
    error:(err)=>{
      console.log("Order product error ")
      console.log(err);
      
    }
  });

 }
      
}

    changeProductsStock(){
      

      for (const i of this.UserOrder) {
        let ProductforStock:any
   
        console.log("+++//////////////")
        this.products_service.getOneProduct(i.id).subscribe({
          next:(data)=>{
            console.log("  Product : data  : ");
            console.log(data);
            ProductforStock=data
            ProductforStock.stock-=i.totalquantity
            this.products_service.Update(ProductforStock).subscribe();
            
          },
          error:(err)=>{
            console.log(" product stock error ")
            console.log(err);
            
          }
        });
        
      
       }
    }

}


