import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { OrderStatus } from '../../../models/order-status';
import { OrderDto } from '../../../models/order-dto';
import { Subscription } from 'rxjs';
import { OrderService } from '../../../services/order.service';
import { OrderSignalrService } from '../../../services/order-signalr.service';
import { ActivatedRoute, RouterModule } from '@angular/router';

@Component({
  selector: 'app-courier-ready-orders-page',
  imports: [CommonModule, RouterModule],
  templateUrl: './courier-ready-orders-page.component.html',
  styleUrl: './courier-ready-orders-page.component.css'
})
export class CourierReadyOrdersPageComponent {
  ordersByStatus: Partial<{ [key in OrderStatus]: OrderDto[] }> = {
    [OrderStatus.ReadyForDelivery]: [],
    [OrderStatus.OutForDelivery]: []
  };

  orderStatuses: OrderStatus[] = [
    OrderStatus.ReadyForDelivery,
    OrderStatus.OutForDelivery
  ];

  courierId: string ="";

  private signalrSubscription!: Subscription;

  constructor(
    private orderService: OrderService,
    private signalrService: OrderSignalrService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.courierId = this.route.snapshot.paramMap.get('id') ?? '';
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
      this.orderService.getCourierOrdersForAdmin(this.courierId,status).subscribe(orders => {
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

  handleReadyForDelivery() {
    this.orderService.updateOrdersWithOutForDeliveryStatus(this.courierId).subscribe({
      next: (updatedOrders) => {
        console.log('All orders marked as "OutForDelivery":', updatedOrders);
      },
      error: (error) => {
        console.error('Error updating orders:', error);
      }
    });
  }
  
}
