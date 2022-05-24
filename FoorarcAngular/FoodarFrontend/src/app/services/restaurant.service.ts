import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { CreateFood, Food } from "../model/food.model";
import { CreateRestaurant, Restaurant } from "../model/restaurant.model";

@Injectable({
    providedIn: 'root'
})
export class RestaurantService {

    constructor(
        private httpClient: HttpClient
    ){}

    CreateRestaurant(restaurant: CreateRestaurant){
        return this.httpClient.post<string>(environment.backendUrl + 'api/restaurant', restaurant)
    }

    GetRestaurantById(restaurantId: string){
        return this.httpClient.get<Restaurant | null>(environment.backendUrl + `api/restaurant/${restaurantId}`)
    }
    
    GetAllRestaurants(){
        return this.httpClient.get<Restaurant[]>(environment.backendUrl + 'api/restaurant')
    }

    GetFoodById(restaurantId: string, foodId: string){
        return this.httpClient.get<Food>(environment.backendUrl + `api/restaurant/${restaurantId}/${foodId}`)
    }

    AddFoodToRestaurant(food: CreateFood){
        return this.httpClient.post<string>(environment.backendUrl + 'api/restaurant/food', food)
    }

    UpdateFood(food: Food){
        return this.httpClient.put<string>(environment.backendUrl + 'api/restaurant/food', food)
    }

    UpdateRestaurant(restaurant: Restaurant){
        return this.httpClient.put<string>(environment.backendUrl + 'api/restaurant', restaurant)
    }

    DeleteFood(foodId: string){
        return this.httpClient.delete<string>(environment.backendUrl + `api/restaurant/food/${foodId}`)
    }
    
    DeleteRestaurant(){
        return this.httpClient.delete<string>(environment.backendUrl + 'api/restaurant')
    }
}