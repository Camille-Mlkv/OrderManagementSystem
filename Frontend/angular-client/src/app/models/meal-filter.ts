export interface MealFilterDto {
    CategoryId: string | null;
    TagIds: string[];
    MinPrice: number | null;
    MaxPrice: number | null;
    MinCalories: number | null;
    MaxCalories: number | null;
  }
  