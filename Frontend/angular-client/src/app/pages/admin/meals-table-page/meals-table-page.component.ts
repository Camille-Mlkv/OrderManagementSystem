import { CommonModule } from '@angular/common';
import { Meal } from '../../../models/meal';
import { PagedList } from '../../../models/paged-list';
import { MealService } from './../../../services/meal.service';
import { Component, OnInit } from '@angular/core';
import { MealFilterDto } from '../../../models/meal-filter';
import { MealFilterComponent } from '../../../components/meal-filter/meal-filter.component';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-meals-table-page',
  imports: [CommonModule, MealFilterComponent, RouterModule],
  templateUrl: './meals-table-page.component.html',
  styleUrl: './meals-table-page.component.css'
})
export class MealsTablePageComponent implements OnInit {
  meals: Meal[]=[];
  currentPage: number = 1;
  totalPages: number = 0;
  pageSize: number = 3;

  filter: MealFilterDto = {
      IsAvailable: null,
      CuisineId: null,
      CategoryId: null,
      TagIds: [],
      MinPrice: null,
      MaxPrice: null,
      MinCalories: null,
      MaxCalories: null
  };

  constructor(private mealService:MealService,private router: Router){}

  ngOnInit(): void {
    this.loadMeals();
  }

  loadMeals(): void {
    this.mealService.getFilteredMeals(this.currentPage, this.pageSize, this.filter).subscribe((mealsList: PagedList<Meal>) => {
      this.meals = mealsList.items;
      this.totalPages = Math.ceil(mealsList.totalCount / this.pageSize);
    });
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadMeals();
    }
  }

  onFilterChanged(filter: MealFilterDto): void {
    this.filter = filter; 
    this.currentPage = 1; 
    this.loadMeals();
  }

  OnMealInfo(id: string){
    this.router.navigate(['/meal',id])
  }

}
