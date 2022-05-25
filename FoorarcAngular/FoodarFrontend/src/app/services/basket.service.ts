import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Basket, BasketFood, CreateBasketFood } from "../model/basket.model";

@Injectable({
    providedIn: 'root'
})
export class BasketService {

    constructor(private httpClient: HttpClient){}

    GetUserBakset(){
        return this.httpClient.get<Basket | null>(environment.backendUrl + 'api/basket')
    }

    DeleteBasket(){
        return this.httpClient.delete<string>(environment.backendUrl + 'api/basket')
    }

    AddFoodToBasket(food: CreateBasketFood){
        return this.httpClient.post<string>(environment.backendUrl + 'api/basket',food)
    } 

    DeleteFoodFromBasket(id: string){
        return this.httpClient.delete<string>(environment.backendUrl + `api/basket/${id}`)
    }
}