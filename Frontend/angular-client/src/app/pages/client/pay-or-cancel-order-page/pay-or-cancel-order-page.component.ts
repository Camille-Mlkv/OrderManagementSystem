import { Component, OnInit } from '@angular/core';
import { OrderDto } from '../../../models/order/order-dto';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { CommonModule } from '@angular/common';
import { OrderInfoComponent } from "../../../components/order-info/order-info.component";

@Component({
  selector: 'app-pay-or-cancel-order-page',
  imports: [CommonModule, OrderInfoComponent],
  templateUrl: './pay-or-cancel-order-page.component.html',
  styleUrl: './pay-or-cancel-order-page.component.css'
})
export class PayOrCancelOrderPageComponent {
  orderId: string = "";

  constructor(
    private orderService: OrderService,
    private router: Router,
    private route: ActivatedRoute
  ) 
  {
    const orderIdFromRoute = this.route.snapshot.paramMap.get('id');
    this.orderId = orderIdFromRoute ? orderIdFromRoute : '';
  }

  cancelOrder(){
    this.orderService.cancelOrder(this.orderId).subscribe({
      next: () => {
        console.log('order is cancelled');
        this.router.navigate(['/client-orders']);
      },
      error: err => console.error('Failed to cancel order:', err)
    });
  }

  payOrder(){
    this.orderService.createCheckoutSession(this.orderId).subscribe({
      next: (result) => {
        if (result.success && result.paymentUrl) {
          window.location.href = result.paymentUrl;
        } 
        else {
          console.error('Payment session failed', result);
        }
      },
      error: err => console.error('Failed to create checkout session:', err)
    });
  }
}
