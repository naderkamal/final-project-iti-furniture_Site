import { Component } from '@angular/core';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css',
})
export class TestComponent {
  relatedProducts = [
    {
      id: 1,
      productName: 'chair1',
      productDescription: 'Comfort',
    },
    {
      id: 2,
      productName: 'chair1',
      productDescription: 'Comfort',
    },
    {
      id: 3,
      productName: 'chair1',
      productDescription: 'Comfort',
    },
    {
      id: 4,
      productName: 'chair1',
      productDescription: 'Comfort',
    },
  ];
  addZoomEffect() {
    document.querySelector('.card')?.classList.add('zoomed');
  }

  removeZoomEffect() {
    document.querySelector('.card')?.classList.remove('zoomed');
  }
}
