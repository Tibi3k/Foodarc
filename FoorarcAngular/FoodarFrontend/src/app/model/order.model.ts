import { Basket } from "./basket.model";

export interface Order {
    id: string,
    userId: string,
    orderDate: string,
    orders: Basket[]
}