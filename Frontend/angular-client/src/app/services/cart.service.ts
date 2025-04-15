import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CartItem } from '../models/cart-item';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private baseURL = `${environment.apiUrl}/cart`;

  constructor(private httpClient: HttpClient) {}

  getCart(): Observable<CartItem[]> {
    return this.httpClient.get<CartItem[]>(this.baseURL);
  }

  addToCart(item: CartItem): Observable<void>{
    return this.httpClient.post<void>(this.baseURL, item);
  }

  updateCart(item: CartItem): Observable<void>{
    return this.httpClient.put<void>(this.baseURL, item);
  }

  deleteCartItem(mealId: string){
    return this.httpClient.delete<void>(`${this.baseURL}/${mealId}`);
  }

  clearCart(){
    return this.httpClient.delete<void>(`${this.baseURL}`);
  }
}
