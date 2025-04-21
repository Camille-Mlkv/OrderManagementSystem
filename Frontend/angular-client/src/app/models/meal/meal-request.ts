export interface MealRequest {
    name: string;
    description: string;
    price: number;
    calories: number;
    isAvailable: boolean;
    imageData: string | undefined; 
    imageContentType: string | undefined;
    categoryId: string;
    cuisineId: string;
    tagIds: string[];
}