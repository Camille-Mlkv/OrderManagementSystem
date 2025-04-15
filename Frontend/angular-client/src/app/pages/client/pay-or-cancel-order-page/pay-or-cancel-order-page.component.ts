import { Component, OnInit } from '@angular/core';
import { OrderDetails } from '../../../models/order-details';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pay-or-cancel-order-page',
  imports: [CommonModule],
  templateUrl: './pay-or-cancel-order-page.component.html',
  styleUrl: './pay-or-cancel-order-page.component.css'
})
export class PayOrCancelOrderPageComponent implements OnInit {
  order: OrderDetails | null = null;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    const orderId = this.route.snapshot.paramMap.get('id');
    if (orderId) {
      this.orderService.getOrderById(orderId).subscribe({
        next: (order) => {
          this.order = order;
          console.log(this.order);
        },
        error: (err) => console.error('Failed to load order:', err)
      });
    }
  }

  cancelOrder(){
    this.orderService.cancelOrder(this.order!.id).subscribe({
      next: () => {
        console.log('order is cancelled');
        // redirect
      },
      error: err => console.error('Failed to cancel order:', err)
    });
  }

  payOrder(){
    this.orderService.createCheckoutSession(this.order!.id).subscribe({
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
