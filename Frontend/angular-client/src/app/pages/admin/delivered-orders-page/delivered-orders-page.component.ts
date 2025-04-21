import { Component } from '@angular/core';
import { OrderService } from '../../../services/order.service';
import { OrderDto } from '../../../models/order/order-dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-delivered-orders-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './delivered-orders-page.component.html',
  styleUrl: './delivered-orders-page.component.css'
})
export class DeliveredOrdersPageComponent {
  fromDate: string = '';
  toDate: string = '';
  orders: OrderDto[] = [];

  today: string = new Date().toISOString().split('T')[0];

  constructor(private orderService: OrderService) {}

  get isDateRangeValid(): boolean {
    return !!this.fromDate && !!this.toDate && this.fromDate <= this.toDate;
  }

  fetchOrders() {
    if (!this.isDateRangeValid) return;

    this.orderService.getDeliveredOrdersByDate(this.fromDate, this.toDate).subscribe({
      next: (orders) => {
        this.orders = orders;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
