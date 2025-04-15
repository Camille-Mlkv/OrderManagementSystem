import { CartItem } from './../../../models/cart-item';
import { Component } from '@angular/core';
import { CartService } from '../../../services/cart.service';
import { MealService } from '../../../services/meal.service';
import { CartDisplayItem } from '../../../models/cart-display-item';
import { forkJoin } from 'rxjs';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-cart-page',
  imports: [CommonModule, RouterModule],
  templateUrl: './cart-page.component.html',
  styleUrl: './cart-page.component.css'
})
export class CartPageComponent {
  cartItems: CartDisplayItem[] = [];
  hasUnavailableMeals: boolean = false;

  constructor(
    private cartService: CartService,
    private mealService: MealService,
    private router: Router 
  ) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.cartService.getCart().subscribe(cart => {
      const mealRequests = cart.map(item =>
        this.mealService.getMealInfoById(item.mealId)
      );
  
      forkJoin(mealRequests).subscribe(meals => {
        this.cartItems = cart.map(item => ({
          meal: meals.find(m => m.id === item.mealId)!,
          quantity: item.quantity
        }));
        this.hasUnavailableMeals = this.cartItems.some(item => item.meal.isAvailable === false);
      });
    });
  }

  increaseQuantity(item: CartDisplayItem): void {
    const newQuantity = item.quantity + 1;
    const newItem : CartItem = { mealId: item.meal.id, quantity: newQuantity};
    this.cartService.updateCart(newItem).subscribe({
      next: () => {
        item.quantity = newQuantity;
      },
      error: (error) => {
        console.error('Error updating cart item quantity', error);
      }
    });
  }

  decreaseQuantity(item: CartDisplayItem): void {
    if (item.quantity > 1) {
      const newQuantity = item.quantity - 1;
      const newItem : CartItem = { mealId: item.meal.id, quantity: newQuantity};
      this.cartService.updateCart(newItem).subscribe({
        next: () => {
          item.quantity = newQuantity;
        },
        error: (error) => {
          console.error('Error updating cart item quantity', error);
        },
      });
    }
  }

  deleteItemFromCart(item: any): void {
    this.cartService.deleteCartItem(item.meal.id).subscribe({
      next: () => {
        this.cartItems = this.cartItems.filter(cartItem => cartItem.meal.id !== item.meal.id);
        this.hasUnavailableMeals = this.cartItems.some(item => item.meal.isAvailable === false);
      },
      error: (error) => {
        console.error('Error deleting cart item', error);
      }
    });
  }

  getTotalSum(): number {
    return this.cartItems.reduce((total, item) => total + (item.meal.price * item.quantity), 0);
  }

  clearCart(): void {
    this.cartService.clearCart().subscribe({
      next: () => {
        this.cartItems = [];
      },
      error: (error) => {
        console.error('Error clearing cart', error);
      }
    });
  }

  goToCreateOrder(): void {
    this.router.navigate(['/create-order']);
  }
  
}
