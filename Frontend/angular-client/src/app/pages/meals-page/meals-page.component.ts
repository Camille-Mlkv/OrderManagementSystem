import { CartService } from './../../services/cart.service';
import { Component, OnInit } from '@angular/core';
import { MealService } from '../../services/meal.service';
import { Meal } from '../../models/meal';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedList } from '../../models/paged-list';
import { CommonModule } from '@angular/common';
import { MealFilterDto } from '../../models/meal-filter';
import { MealFilterComponent } from "../../components/meal-filter/meal-filter.component";
import { AuthContext } from '../../auth/auth-context';
import { CartItem } from '../../models/cart-item';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-meals-page',
  imports: [CommonModule, MealFilterComponent],
  templateUrl: './meals-page.component.html',
  styleUrl: './meals-page.component.css'
})
export class MealsPageComponent implements OnInit {
  meals: Meal[]=[];
  cuisineId!: string;

  currentPage: number = 1;
  totalPages: number = 0;
  pageSize: number = 3;

  filter: MealFilterDto = {
    IsAvailable: true,
    CuisineId: null,
    CategoryId: null,
    TagIds: [],
    MinPrice: null,
    MaxPrice: null,
    MinCalories: null,
    MaxCalories: null
  };

  constructor(
    public authContext: AuthContext,
    private mealService: MealService,
    private cartService: CartService,  
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService){}

  ngOnInit(): void {
    this.cuisineId = this.route.snapshot.paramMap.get('cuisineId')!;
    this.filter.CuisineId = this.cuisineId;
    this.filter.IsAvailable = true;

    this.loadMeals();
  }

  loadMeals(): void {
    this.mealService.getFilteredMeals(this.currentPage, this.pageSize, this.filter).subscribe((mealsList: PagedList<Meal>) => {
      this.meals = mealsList.items; 
      this.totalPages = Math.ceil(mealsList.totalCount / this.pageSize);
    });
  }

  changePage(page: number): void {
    this.currentPage = page;
    this.loadMeals();
  }

  transformImage(url: string): string {
    return url.replace('/upload/', '/upload/h_300,w_300/');
  }

  onFilterChanged(filter: MealFilterDto): void {
    filter.IsAvailable = true;
    filter.CuisineId = this.cuisineId;
    
    this.filter = filter; 
    this.currentPage = 1; 
    this.loadMeals();
  }

  onBack(){
    this.router.navigate(['/cuisines']);
  }

  goToMealInfo(id: string): void {
    this.router.navigate(['/meal',id]);
  }

  onAddItem(meal: Meal){
    const newItem: CartItem = { mealId: meal.id, quantity: 1 };
    this.cartService.addToCart(newItem).subscribe(() => {
      this.toastr.success('Item added to cart!');
    });
  }
}
