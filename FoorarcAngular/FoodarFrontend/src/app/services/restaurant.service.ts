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

    GetRestaurantById(){
        return this.httpClient.get<Restaurant>(environment.backendUrl + 'api/restaurant/single')
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
}