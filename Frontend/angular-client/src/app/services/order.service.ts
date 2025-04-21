import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderRequest } from '../models/order/order-request';
import { OrderDto } from '../models/order/order-dto';
import { PaymentResult } from '../models/order/payment-result';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private baseURL = `${environment.apiUrl}/orders`;

  constructor(private httpClient: HttpClient) {}

  createOrder(order: OrderRequest): Observable<string> {
    return this.httpClient.post<string>(this.baseURL,order);
  }

  getOrderById(orderId: string): Observable<OrderDto> {
    return this.httpClient.get<OrderDto>(`${this.baseURL}/${orderId}`);
  }

  cancelOrder(orderId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.baseURL}/${orderId}`);
  }
  
  createCheckoutSession(orderId: string): Observable<PaymentResult> {
    const url = `${environment.apiUrl}/payments?orderId=${orderId}`;
    return this.httpClient.post<PaymentResult>(url, {});
  }

  getClientOrdersByStatus(status: string) {
    return this.httpClient.get<OrderDto[]>(`${this.baseURL}/client/status-${status}`);
  }

  getOpenedOrders(){
    return this.httpClient.get<OrderDto[]>(`${this.baseURL}/opened-orders`);
  }

  assignCourier(orderId: string){
    return this.httpClient.patch(`${this.baseURL}/${orderId}/assign-courier`, {});
  }

  getCourierOrdersByStatus(status: string){
    return this.httpClient.get<OrderDto[]>(`${this.baseURL}/courier/status-${status}`);
  }

  confirmOrderByClient(orderId: string){
    return this.httpClient.patch(`${this.baseURL}/${orderId}/client-confirmation`, {});
  }

  confirmOrderByCourier(orderId: string){
    return this.httpClient.patch(`${this.baseURL}/${orderId}/courier-confirmation`, {});
  }

  getOrdersByStatus(status: string){
    return this.httpClient.get<OrderDto[]>(`${this.baseURL}/status-${status}`);
  }

  updateOrderWithReadyStatus(orderId: string){
    return this.httpClient.patch(`${this.baseURL}/${orderId}/status/ready`, {});
  }

  getCourierOrdersForAdmin(courierId: string, status: string){
    return this.httpClient.get<OrderDto[]>(`${this.baseURL}/courier/${courierId}/status-${status}`);
  }

  updateOrdersWithOutForDeliveryStatus(courierId: string){
    return this.httpClient.patch(`${this.baseURL}/${courierId}/status/out-for-delivery`, {});
  }

  getDeliveredOrdersByDate(from: string, to: string){
    return this.httpClient.get<OrderDto[]>(`${this.baseURL}/delivered`, {
      params: { from, to }
    });
  }
}
