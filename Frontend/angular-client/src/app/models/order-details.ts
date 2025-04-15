import { MealInOrder } from "./meal-in-order";

export interface OrderDetails{
    id: string;
    orderNumber: string;
    clientId: string;
    courierId: string | null;
    status: string;
    address: string;
    meals: MealInOrder[];
    totalPrice: number;
    createdAt: string;
    deliveryDate: string | null;
}