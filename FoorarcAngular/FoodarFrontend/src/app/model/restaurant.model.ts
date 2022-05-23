import { Food } from "./food.model"

export interface Restaurant{
    id: string,
    ownerId: string
    name: string,
    address: string,
    availableFoods: Food[],
    description: string,
    imagePath: string
}

export interface CreateRestaurant{
    name: string,
    address: string,
    description: string,
    imagePath: string
}