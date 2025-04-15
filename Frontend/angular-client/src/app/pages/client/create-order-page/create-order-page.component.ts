import {
  Component,
  ElementRef,
  EventEmitter,
  NgZone,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
} from '@angular/forms';
import { OrderService } from '../../../services/order.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { OrderRequest } from '../../../models/order-request';

@Component({
  selector: 'app-create-order-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-order-page.component.html',
  styleUrl: './create-order-page.component.css'
})
export class CreateOrderPageComponent implements OnInit{
  @ViewChild('inputField') inputField!: ElementRef;
  @Output() placeChanged = new EventEmitter<string>();

  autocomplete: google.maps.places.Autocomplete | undefined;
  listener: any;
  address: string ="";

  constructor(
    private ngZone: NgZone, 
    private orderService: OrderService,
    private router: Router,
    private toastr: ToastrService) {}

  ngOnInit() {}

  ngAfterViewInit() {
    this.autocomplete = new google.maps.places.Autocomplete(
      this.inputField.nativeElement
    );

    this.autocomplete.addListener('place_changed', () => {
      this.ngZone.run(() => {
        const result= this.inputField.nativeElement.value;
        this.placeChanged.emit(result);
        this.address = result;
      });
    });
  }

  ngOnDestroy() {
    if (this.autocomplete) {
      google.maps.event.clearInstanceListeners(this.autocomplete);
    }
  }

  onContinue(){
    const request: OrderRequest = { address: this.address };
    this.orderService.createOrder(request).subscribe({
      next: (orderId: string) => {
        this.router.navigate(['/pay-or-cancel', orderId]);
      },
      error: (err) => {
        console.error('Order creation failed:', err);
      }
    });
  }

}
