import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryService } from '../../services/category.service';
import { CuisineService } from '../../services/cuisine.service';
import { TagService } from '../../services/tag.service';
import { Category } from '../../models/category';
import { Cuisine } from '../../models/cuisine';
import { Tag } from '../../models/tag';
import { CommonModule } from '@angular/common';
import { MealRequest } from '../../models/meal-request';

@Component({
  selector: 'app-meal-form',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './meal-form.component.html',
  styleUrl: './meal-form.component.css'
})
export class MealFormComponent implements OnInit{ 
  @Input() mealRequest: MealRequest | undefined;
  @Output() formSubmitted = new EventEmitter<MealRequest>();
  
  form!: FormGroup;
  categories: Category[] = [];
  cuisines: Cuisine[] = [];
  tags: Tag[] = [];
  imageBase64?: string;
  contentType?:string;
  imagePreviewUrl?: string;

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private cuisineService: CuisineService,
    private tagService: TagService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      calories: [0, [Validators.required, Validators.min(0)]],
      isAvailable: [true],
      categoryId: ['', Validators.required],
      cuisineId: ['', Validators.required],
      tagIds: [[]],
      imageFile: new FormControl<null | File>(null)
    });

    this.categoryService.getCategories().subscribe(data => this.categories = data);
    this.cuisineService.getCuisines().subscribe(data => this.cuisines = data);
    this.tagService.getTags().subscribe(data => this.tags = data);

    if(this.mealRequest !== undefined){
      this.form.patchValue(this.mealRequest);
    }
  }

  imageSelected(event: Event){
    const input = event.target as HTMLInputElement;
    if(input.files && input.files.length > 0){
      const file: File = input.files[0];
      this.form.patchValue({imageFile: file});

      this.toBase64(file).then((result: string) => {
        this.imagePreviewUrl = result;
        const matches = result.match(/^data:(.*);base64,(.*)$/);
        if (matches) {
          this.imageBase64 = matches[2]; 
          this.contentType = matches[1]; 
        }
      }).catch(error => console.error(error));
    }
  }

  toBase64(file: File): Promise<string>{
    return new Promise((resolve, reject)=>{
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = (error) =>reject(error);
    })
  }

  onSubmit(): void {
    if (this.form.valid) {
      const formValues = this.form.value;
      const meal: MealRequest = {
        name: formValues.name,
        description: formValues.description,
        price: formValues.price,
        calories: formValues.calories,
        isAvailable: formValues.isAvailable,
        imageData: this.imageBase64,
        imageContentType: this.contentType, 
        categoryId: formValues.categoryId,
        cuisineId: formValues.cuisineId,
        tagIds: formValues.tagIds
      };
      console.log(meal);
      this.formSubmitted.emit(meal);
    }
  }
}
