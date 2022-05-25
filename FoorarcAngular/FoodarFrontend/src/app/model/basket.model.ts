import { InteractivityChecker } from "@angular/cdk/a11y"
import { Food } from "./food.model"

export interface Basket{
    id: string,
    userId: string,
    lastEdited: string,
    foods: BasketFood[]
    totalCost: number
}

export interface BasketFood{
    id: string,
    addTime: string,
    orderedFood: Food,
    restaurantUrl: string
}

export interface CreateBasketFood{
    orderedFood: Food,
    restaurantUrl: string
}