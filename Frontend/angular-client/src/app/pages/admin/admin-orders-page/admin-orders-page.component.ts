import { Component, NgZone } from '@angular/core';
import { OrderStatus } from '../../../models/order-status';
import { Subscription } from 'rxjs';
import { OrderService } from '../../../services/order.service';
import { OrderSignalrService } from '../../../services/order-signalr.service';
import { OrderDto } from '../../../models/order-dto';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-orders-page',
  imports: [CommonModule, RouterModule],
  templateUrl: './admin-orders-page.component.html',
  styleUrl: './admin-orders-page.component.css'
})
export class AdminOrdersPageComponent {
  ordersByStatus: Partial<{ [key in OrderStatus]: OrderDto[] }> = {
    [OrderStatus.InProgress]: [],
    [OrderStatus.ReadyForDelivery]: []
  };

  orderStatuses: OrderStatus[] = [
    OrderStatus.InProgress,
    OrderStatus.ReadyForDelivery
  ];

  private signalrSubscription!: Subscription;

  constructor(
    private orderService: OrderService,
    private signalrService: OrderSignalrService,
    private ngZone: NgZone
  ) {}

  ngOnInit(): void {
    this.loadAllOrders();

    this.ngZone.runOutsideAngular(() => {
      this.signalrService.startConnection();
      this.signalrSubscription = this.signalrService.orderUpdated$.subscribe(order => {
        if (order) {
          this.ngZone.run(() => {
            console.log('Received updated order with status:', order.status);
            this.moveOrderToNewStatus(order);
          });
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.signalrSubscription?.unsubscribe();
    this.signalrService.stopConnection();
  }

  loadAllOrders(): void {
    this.orderStatuses.forEach(status => {
      this.orderService.getOrdersByStatus(status).subscribe(orders => {
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

  markOrderAsReady(order: OrderDto){
    this.orderService.updateOrderWithReadyStatus(order.id).subscribe({
      next: () => {
        console.log(`Order #${order.orderNumber} is ready for delivery.`);
      },
    });
  }
}
