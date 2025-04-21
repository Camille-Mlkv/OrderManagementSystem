import { MealInOrder } from "../meal/meal-in-order";
import { OrderStatus } from "./order-status";

export interface OrderDto{
    id: string;
    orderNumber: string;
    clientId: string;
    courierId: string | null;
    status: OrderStatus;
    address: string;
    meals: MealInOrder[];
    totalPrice: number;
    createdAt: string;
    confirmedByClient: boolean;
    confirmedByCourier: boolean;
    deliveryDate: string | null;
}