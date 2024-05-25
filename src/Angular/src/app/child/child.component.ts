import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-child',
  standalone: true,
  imports: [],
  template: `
    <button class="btn" (click)="addItem()">Add Item</button>
  `,
  styleUrl: './child.component.scss'
})
export class ChildComponent {
  addItem() {}
}
