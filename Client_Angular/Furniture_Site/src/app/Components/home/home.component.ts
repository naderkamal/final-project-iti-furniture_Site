
import { Component, AfterViewInit, ViewChild, ElementRef, OnInit, ViewChildren, QueryList } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbCarousel } from '@ng-bootstrap/ng-bootstrap';
import { CardComponent } from '../card/card.component';
import { CommonModule } from '@angular/common';
import { HomeService } from '../../services/home.service';
import { HttpClientModule } from '@angular/common/http';
import { mergeMap } from 'rxjs/operators';
import { groupBy } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { toArray } from 'rxjs/operators';
import { Observable, forkJoin } from 'rxjs';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CardComponent,CommonModule,HttpClientModule],
  providers:[HomeService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements  OnInit,AfterViewInit {
  topRatedProducts: any;
  topLastedProducts: any;
  Products : any;
  @ViewChildren(NgbCarousel) carousels!: QueryList<NgbCarousel>;
    private interval: any;

  constructor(private apiService: HomeService) {}
  ngOnInit(): void {
    this.fetchTopRatedProducts();
    this.fetchLastedProducts();
    this.fetchGetProducts();
  }

  ngAfterViewInit(): void {
    if (this.carousels) {
      this.startCarousel();
    }
  }

  fetchLastedProducts(): void {
    this.apiService.getLastedProducts().subscribe({
      next: (data: any) => { 
        const dataArray = data as any[];
        // Sort products by ID in descending order
        dataArray.sort((a, b) => b.id - a.id);
  
        // Get the last three products (first three in the sorted list)
        this.topLastedProducts = dataArray.slice(0, 4);
      },
      error: (error: any) => {
        console.error('Error fetching latest products:', error);
      }
    });
  }
  
  fetchGetProducts(): void {
    this.apiService.getProducts().subscribe({
      next: (data: any) => {
        // Take only the first 3 products
        this.Products = data.slice(0, 4);
      },
      error: (error: any) => {
        console.error('Error fetching top-rated products:', error);
      }
  });
  }

  fetchTopRatedProducts(): void {
    this.apiService.getTopRatedProducts().subscribe(
      (products: any) => { 
        products.sort((a :any, b: any) => b.ratingStar - a.ratingStar);
        // Take only the first 3 products
        this.topRatedProducts = products.slice(0, 4);
      },
      (error) => {
        console.error('Error fetching top-rated products:', error);
      }
    );
  }
  
  
  startCarousel(): void {
    setTimeout(() => {
      this.carousels.forEach(carousel => {
        this.interval = setInterval(() => {
          carousel.next(); // Move to the next slide
        }, 3000);
      });
    }, 0);
  }

  stopCarousel(): void {
    clearInterval(this.interval); // Stop automatic sliding
  }

  goToNextSlide(): void {
    this.stopCarousel(); // Stop automatic sliding when the user navigates manually
    this.carousels.forEach(carousel => {
      carousel.next();
    });
    this.startCarousel(); // Restart automatic sliding after manual navigation
  }

  goToPreviousSlide(): void {
    this.stopCarousel(); // Stop automatic sliding when the user navigates manually
    this.carousels.forEach(carousel => {
      carousel.prev();
    });
    this.startCarousel(); // Restart automatic sliding after manual navigation
  }}