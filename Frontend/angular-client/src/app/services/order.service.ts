import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderRequest } from '../models/order-request';
import { OrderDetails } from '../models/order-details';
import { PaymentResult } from '../models/payment-result';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseURL = `${environment.apiUrl}/orders`;

  constructor(private httpClient: HttpClient) {}

  createOrder(order: OrderRequest): Observable<string> {
    return this.httpClient.post<string>(this.baseURL,order);
  }

  getOrderById(orderId: string): Observable<OrderDetails> {
    return this.httpClient.get<OrderDetails>(`${this.baseURL}/${orderId}`);
  }

  cancelOrder(orderId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.baseURL}/${orderId}`);
  }
  
  createCheckoutSession(orderId: string): Observable<PaymentResult> {
    const url = `${environment.apiUrl}/payments?orderId=${orderId}`;
    return this.httpClient.post<PaymentResult>(url, {});
  }
}
