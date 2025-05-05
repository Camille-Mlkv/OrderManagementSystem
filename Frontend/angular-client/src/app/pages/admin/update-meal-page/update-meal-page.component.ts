import { Component, OnInit } from '@angular/core';
import { Meal } from '../../../models/meal/meal';
import { MealService } from '../../../services/meal.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { MealRequest } from '../../../models/meal/meal-request';
import { MealFormComponent } from "../../../components/meal-form/meal-form.component";

@Component({
  selector: 'app-update-meal-page',
  imports: [CommonModule, MealFormComponent, RouterModule],
  templateUrl: './update-meal-page.component.html',
  styleUrl: './update-meal-page.component.css'
})
export class UpdateMealPageComponent implements OnInit{
  mealRequest!: MealRequest;

  constructor(
        private mealService: MealService,
        private toastr: ToastrService,
        private route: ActivatedRoute,
        private router: Router){}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.mealService.getMealInfoById(id).subscribe({
        next: (meal: Meal) => {
          this.mealRequest = {
            name: meal.name,
            description: meal.description,
            price: meal.price,
            calories: meal.calories,
            isAvailable: meal.isAvailable,
            imageData: '',
            imageContentType: '',
            categoryId: meal.categoryId,
            cuisineId: meal.cuisineId,
            tagIds: meal.tags.map(tag => tag.id),
          };
        },
        error: () => this.toastr.error('Error loading meal info'),
      });
    }
  }

  onUpdate(meal: MealRequest) {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.mealService.updateMeal(id, meal).subscribe({
        next: () =>{
          this.toastr.success('Meal updated successfully.');
          this.router.navigate(['/meals-table']);
        },
        error: () => this.toastr.error('Error updating meal.')
      });
    }
  }
}
