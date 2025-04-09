export interface Meal {
    id: string;
    name: string;
    price: number;
    description: string;
    calories: number;
    isAvailable: boolean;
    imageUrl: string;
    categoryId: string;
    cuisineId: string;
    tags: { name: string }[];
}