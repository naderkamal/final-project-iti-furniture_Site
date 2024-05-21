import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, EventEmitter, HostListener, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatSliderModule } from '@angular/material/slider';
import { CategoriesService } from '../../../services/categories.service';

@Component({
  selector: 'app-filter',
  standalone: true,
  imports: [FormsModule,CommonModule,MatSliderModule,HttpClientModule],
  providers:[CategoriesService],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css'
})
//npm install --save @angular/material @angular/cdk
export class FilterComponent {
  showFilter: boolean = false;
  categoryid: any;
  allcategory:any;
  @Output() myevent=new EventEmitter();

  constructor(private mycategory:CategoriesService){}

  ngOnInit(): void {
    this.mycategory.getallCategory().subscribe({
      next:(data) =>{
        this.allcategory=data;
      },
      error:(err) => {
        console.log(err);
      },
    })
  }
  
  @HostListener('window:click', ['$event'])
  onClick(event: any) {
    if (
      !event.target.closest('.filter-container') &&
      !event.target.closest('.filter')
    ) {
      this.showFilter = false;
    }
  }

  chooseCategory(event: any) {
    this.categoryid = event.target.id;
    this.myevent.emit(this.categoryid);
  }

  sliderValue: number = 70;
  onSliderChange() {
    console.log('Slider value changed to: ', this.sliderValue);
  }
  startValue: number = 0;
  endValue: number = 5000;
  changestartPrice(event: any) {
    this.startValue = event.target.value;
  }
  changeendPrice(event: any) {
    this.endValue = event.target.value;
  }
  changePrice(event1: any,event2: any) {
    this.startValue = event1;
    this.endValue = event2;
    this.myevent.emit({start:this.startValue,end:this.endValue});
  }
}
