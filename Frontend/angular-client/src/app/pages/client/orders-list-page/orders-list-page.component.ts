import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { OrderDto } from '../../../models/order-dto';
import { Subscription } from 'rxjs';
import { OrderSignalrService } from '../../../services/order-signalr.service';
import { OrderService } from '../../../services/order.service';
import { CommonModule } from '@angular/common';
import { OrderStatus } from '../../../models/order-status';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './orders-list-page.component.html',
  styleUrl: './orders-list-page.component.css'
})
export class OrderListComponent implements OnInit, OnDestroy {
  ordersByStatus: { [key in OrderStatus]: OrderDto[] } = {
    [OrderStatus.InProgress]: [],
    [OrderStatus.ReadyForDelivery]: [],
    [OrderStatus.OutForDelivery]: [],
    [OrderStatus.Delivered]: []
  };

  private signalrSubscription!: Subscription;
  orderStatuses: OrderStatus[] = [
    OrderStatus.InProgress,
    OrderStatus.ReadyForDelivery,
    OrderStatus.OutForDelivery,
    OrderStatus.Delivered
  ];

  constructor(
    private orderService: OrderService,
    private signalrService: OrderSignalrService,
    private ngZone: NgZone
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
    Object.values(OrderStatus).forEach(status => {
      this.orderService.getClientOrdersByStatus(status).subscribe(orders => {
        this.ordersByStatus[status] = orders;
      });
    });
  }

  private moveOrderToNewStatus(order: OrderDto): void {
    Object.values(OrderStatus).forEach(status => {
      this.ordersByStatus[status] = this.ordersByStatus[status].filter(o => o.id !== order.id);
    });

    const updatedOrder: OrderDto = { ...order };
    this.ordersByStatus[order.status].unshift(updatedOrder);
  }

  confirmDelivery(order: OrderDto){
    if (confirm(`Are you sure you want to confirm delivery for order #${order.orderNumber}?`)) {
      this.orderService.confirmOrderByClient(order.id).subscribe({
        next: () => {
          console.log(`Order #${order.orderNumber} delivery confirmed`);
        },
        error: (err) => console.error('Failed to confirm delivery:', err)
      });
    }
  }
}
