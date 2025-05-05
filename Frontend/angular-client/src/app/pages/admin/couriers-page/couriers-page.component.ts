import { Component, OnInit } from '@angular/core';
import { User } from '../../../models/auth/user';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-couriers-page',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './couriers-page.component.html',
  styleUrl: './couriers-page.component.css'
})
export class CouriersPageComponent implements OnInit{
  couriers: User[] = [];
  filteredCouriers: User[] = [];
  searchControl = new FormControl('');

  constructor(
    private accountService: AccountService) {}

  ngOnInit(): void {
    this.accountService.getUsersByRole('Courier').subscribe((users) => {
      this.couriers = users;
      this.filteredCouriers = users;
    });

    this.searchControl.valueChanges.subscribe((value) => {
      const search = value?.toLowerCase() ?? '';
      this.filteredCouriers = this.couriers.filter((courier) =>
        courier.email.toLowerCase().includes(search)
      );
    });
  }

}
