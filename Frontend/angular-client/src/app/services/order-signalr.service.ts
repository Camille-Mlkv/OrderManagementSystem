import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { OrderDto } from '../models/order/order-dto';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderSignalrService {
  private hubConnection!: signalR.HubConnection;

  private orderUpdatedSource = new BehaviorSubject<OrderDto | null>(null);
  orderUpdated$ = this.orderUpdatedSource.asObservable();

  public startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5011/hubs/order',{
        skipNegotiation: true, 
        transport: signalR.HttpTransportType.WebSockets
      }) 
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'));

    this.hubConnection.on('OrderUpdated', (order: OrderDto) => {
      console.log('Order updated:', order);
      this.orderUpdatedSource.next(order);
    });
  }

  public stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop().then(() => console.log('[Connection stopped'));
    }
  }
}
