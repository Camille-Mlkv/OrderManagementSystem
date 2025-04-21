import { Component, OnDestroy, OnInit } from '@angular/core';
import { OrderDto } from '../../../models/order/order-dto';
import { Subscription } from 'rxjs';
import { OrderService } from '../../../services/order.service';
import { OrderSignalrService } from '../../../services/order-signalr.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrderStatus } from '../../../models/order/order-status';

@Component({
  selector: 'app-courier-orders-page',
  imports: [CommonModule, RouterModule],
  templateUrl: './courier-orders-page.component.html',
  styleUrl: './courier-orders-page.component.css'
})
export class CourierOrdersPageComponent implements OnInit, OnDestroy {
  ordersByStatus: Partial<{ [key in OrderStatus]: OrderDto[] }> = {
    [OrderStatus.ReadyForDelivery]: [],
    [OrderStatus.OutForDelivery]: []
  };

  orderStatuses: OrderStatus[] = [
    OrderStatus.ReadyForDelivery,
    OrderStatus.OutForDelivery
  ];

  private signalrSubscription!: Subscription;

  constructor(
    private orderService: OrderService,
    private signalrService: OrderSignalrService
  ) {}

  ngOnInit(): void {
    this.loadAllOrders();

    this.signalrService.startConnection();
    this.signalrSubscription = this.signalrService.orderUpdated$.subscribe(order => {
      if (order) {
          console.log('Received updated order with status:', order.status);
          this.moveOrderToNewStatus(order);
      }
    });
  }

  ngOnDestroy(): void {
    this.signalrSubscription?.unsubscribe();
    this.signalrService.stopConnection();
  }

  loadAllOrders(): void {
    this.orderStatuses.forEach(status => {
      this.orderService.getCourierOrdersByStatus(status).subscribe(orders => {
        this.ordersByStatus[status] = orders;
      });
    });
  }

  private moveOrderToNewStatus(order: OrderDto): void {
    this.orderStatuses.forEach(status => {
      this.ordersByStatus[status] = this.ordersByStatus[status]?.filter(o => o.id !== order.id) || [];
    });

    if (this.orderStatuses.includes(order.status)) {
      this.ordersByStatus[order.status]?.unshift(order);
    }
  }

  confirmDelivery(order: OrderDto){
    if (confirm(`Are you sure you want to confirm delivery for order #${order.orderNumber}?`)) {
      this.orderService.confirmOrderByCourier(order.id).subscribe({
        next: () => {
          console.log(`Order #${order.orderNumber} delivery confirmed`);
        },
        error: (err) => console.error('Failed to confirm delivery:', err)
      });
    }
  }
}
