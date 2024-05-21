import { Component } from '@angular/core';
import { CardComponent } from '../card/card.component';
import { FilterComponent } from './filter/filter.component';
import { HttpClientModule } from '@angular/common/http';
import { ProductsService } from '../../services/products.service';
import { FilterService } from '../../services/filter.service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CardComponent, FilterComponent, HttpClientModule],
  providers: [ProductsService,FilterService],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent {
  allProduct: any;
  displayedProducts: any[] = [];

  constructor(private myproduct: ProductsService,private filterproduct :FilterService) { }

  ngOnInit(): void {
    this.myproduct.getAllProduct().subscribe(
      {
        next: (data) => {
          this.allProduct = data;
          console.log("products");
          
          console.log(data);
          
          this.updateDisplayedProducts();
        },
        error: (err) => { console.log(err) }
      }
    );
  }

   // filter
  recivefilter(returneddata: any) {
    if (typeof returneddata === 'object') {
      const keys = Object.keys(returneddata);
      if (keys.length >= 2) {
        // console.log("First Property:", returneddata[keys[0]]);
        // console.log("Second Property:", returneddata[keys[1]]);
        this.filterproduct.filterbyprice(returneddata[keys[0]],returneddata[keys[1]]).subscribe({
          next: (data) => {
            this.allProduct = data;
            this.updateDisplayedProducts();
          },
          error: (err) => { console.log(err) }
        })
      }
    } else {
      // console.log("return is number :", returneddata);
      this.filterproduct.filterbyid(returneddata).subscribe({
        next: (data) => {
          this.allProduct = data;
          this.updateDisplayedProducts();
        },
        error: (err) => { console.log(err) }
      })
    }
  }

  // Pagination
  itemsPerPage: number = 8;
  currentPage: number = 1; 
  get totalPages(): number {
    return Math.ceil(this.allProduct.length / this.itemsPerPage);
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updateDisplayedProducts();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updateDisplayedProducts();
    }
  }

  updateDisplayedProducts() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = Math.min(startIndex + this.itemsPerPage, this.allProduct.length);
    this.displayedProducts = this.allProduct.slice(startIndex, endIndex);
  }
}
