import { Component, NgZone, OnDestroy, OnInit } from '@angular/core';
import { OrderService } from '../../../services/order.service';
import { OrderSignalrService } from '../../../services/order-signalr.service';
import { OrderDto } from '../../../models/order-dto';
import { Subscription } from 'rxjs';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-opened-orders-page',
  imports: [CommonModule, RouterModule],
  templateUrl: './opened-orders-page.component.html',
  styleUrl: './opened-orders-page.component.css'
})
export class OpenedOrdersPageComponent implements OnInit, OnDestroy {
  orders: OrderDto[]=[];
  private signalrSubscription!: Subscription;

  constructor(
    private orderService: OrderService,
    private signalrService: OrderSignalrService,
    private ngZone: NgZone,
    private toastr: ToastrService){}

    ngOnInit(): void {
      this.loadAllOrders();
  
      this.ngZone.runOutsideAngular(() => {
        this.signalrService.startConnection();
        this.signalrSubscription = this.signalrService.orderUpdated$.subscribe(order => {
          if (order) {
            this.ngZone.run(() => {
              console.log('Received updated order with courier id:', order.courierId);

              if (order.courierId) {
                this.orders = this.orders.filter(o => o.id !== order.id);
              }
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
      this.orderService.getOpenedOrders().subscribe(orders => {
        this.orders = orders;
      });
    }

    onTakeOrder(orderId: string){
      this.orderService.assignCourier(orderId).subscribe(_ => {
        this.toastr.show('Order is added to your list.');
      });
    }


}
