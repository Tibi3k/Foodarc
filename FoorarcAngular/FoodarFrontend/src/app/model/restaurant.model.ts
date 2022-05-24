import { Food } from "./food.model"

export interface Restaurant{
    id: string,
    ownerId: string
    name: string,
    address: string,
    city: string,
    country: string,
    zipCode: number
    availableFoods: Food[],
    description: string,
    imagePath: string
}

export interface CreateRestaurant{
    name: string,
    address: string,
    description: string,
    imagePath: string,
    city: string,
    country: string,
    zipCode: number
}