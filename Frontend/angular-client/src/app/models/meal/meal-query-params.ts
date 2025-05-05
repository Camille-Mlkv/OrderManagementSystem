export interface MealQueryParams {
    pageNo: number;
    pageSize: number;
    IsAvailable?: boolean | null;
    CuisineId?: string | null;
    CategoryId?: string | null;
    TagIds?: string[];
    MinPrice?: number | null;
    MaxPrice?: number | null;
    MinCalories?: number | null;
    MaxCalories?: number | null;
  }