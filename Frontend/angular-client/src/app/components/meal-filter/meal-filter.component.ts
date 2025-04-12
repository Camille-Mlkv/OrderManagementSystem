import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Category } from '../../models/category';
import { Tag } from '../../models/tag';
import { CategoryService } from '../../services/category.service';
import { TagService } from '../../services/tag.service';
import { MealFilterDto } from '../../models/meal-filter';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Cuisine } from '../../models/cuisine';
import { CuisineService } from '../../services/cuisine.service';
import { AuthContext } from '../../auth/auth-context';

@Component({
  selector: 'app-meal-filter',
  imports: [CommonModule, FormsModule],
  templateUrl: './meal-filter.component.html',
  styleUrl: './meal-filter.component.css'
})
export class MealFilterComponent implements OnInit {
  isAvailable: boolean | null = null;
  cuisines: Cuisine[]=[];
  categories: Category[] = [];
  tags: Tag[] = [];
  selectedCuisine: string = '';
  selectedCategory: string = '';
  selectedTags: string[] = [];
  minPrice: number | null = null;
  maxPrice: number | null = null;
  minCalories: number | null = null;
  maxCalories: number | null = null;

  @Output() filterChanged = new EventEmitter<MealFilterDto>();

  constructor(
    private categoryService: CategoryService,
    private tagService: TagService,
    private cuisineService: CuisineService,
    public authContext: AuthContext
  ) {}

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe(categories => this.categories = categories);
    this.tagService.getTags().subscribe(tags => this.tags = tags);
    this.cuisineService.getCuisines().subscribe(cuisines => this.cuisines = cuisines);
  }

  applyFilters(): void {
    const filter: MealFilterDto = {
      IsAvailable: this.isAvailable ? this.isAvailable : null,
      CuisineId: this.selectedCuisine ?  this.selectedCuisine : null,
      CategoryId: this.selectedCategory ? this.selectedCategory : null,
      TagIds: this.selectedTags.length > 0 ? this.selectedTags : [],
      MinPrice: this.minPrice,
      MaxPrice: this.maxPrice,
      MinCalories: this.minCalories,
      MaxCalories: this.maxCalories
    };

    this.filterChanged.emit(filter);
  }

  resetFilters(): void {
    this.isAvailable = null;
    this.selectedCategory = '';
    this.selectedCuisine = '';
    this.selectedTags = [];
    this.minPrice = null;
    this.maxPrice = null;
    this.minCalories = null;
    this.maxCalories = null;
    this.applyFilters();
  }
}
