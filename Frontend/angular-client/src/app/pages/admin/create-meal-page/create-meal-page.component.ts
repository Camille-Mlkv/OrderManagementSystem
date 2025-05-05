import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MealService } from '../../../services/meal.service';
import { CommonModule } from '@angular/common';
import { MealRequest } from '../../../models/meal/meal-request';
import { ToastrService } from 'ngx-toastr';
import { MealFormComponent } from "../../../components/meal-form/meal-form.component";
import { Router, RouterModule } from '@angular/router';


@Component({
  selector: 'app-create-meal-page',
  imports: [ReactiveFormsModule, CommonModule, MealFormComponent, RouterModule],
  templateUrl: './create-meal-page.component.html',
  styleUrl: './create-meal-page.component.css'
})
export class CreateMealPageComponent { 

  constructor(
    private mealService: MealService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  onCreate(meal: MealRequest) {
    this.mealService.createMeal(meal).subscribe(() => {
      this.toastr.success('Meal was created.');
      this.router.navigate(['/meals-table']);
    });
  }
}
