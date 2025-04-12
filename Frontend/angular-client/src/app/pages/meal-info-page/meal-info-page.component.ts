import { CuisineService } from './../../services/cuisine.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MealService } from '../../services/meal.service';
import { Meal } from '../../models/meal';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../services/category.service';
import {Location} from '@angular/common';

@Component({
  selector: 'app-meal-info-page',
  imports: [CommonModule],
  templateUrl: './meal-info-page.component.html',
  styleUrl: './meal-info-page.component.css'
})
export class MealInfoPageComponent implements OnInit {
  meal!: Meal;
  categoryName!: string;
  cuisineName!: string;

  constructor(
    private route: ActivatedRoute, 
    private mealService: MealService,
    private categoryService: CategoryService,
    private cuisineService: CuisineService,
    private location: Location){}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if(id){
      this.mealService.getMealInfoById(id).subscribe(meal =>{
        this.meal = meal;

        this.categoryService.getCategoryById(this.meal.categoryId).subscribe(category =>{
          this.categoryName = category.name;
        });
        
        this.cuisineService.getCuisineById(this.meal.cuisineId).subscribe(cuisine =>{
          this.cuisineName = cuisine.name;
        });

      });
      
    }
  }

  transformImage(url: string): string {
    return url.replace('/upload/', '/upload/h_300,w_300/');
  }

  OnPreviousPage(){
    this.location.back();
  }

}
