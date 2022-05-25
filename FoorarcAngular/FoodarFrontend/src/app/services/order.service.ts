import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { forkJoin, Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Basket } from "../model/basket.model";
import { Order } from "../model/order.model";

@Injectable({
    providedIn: 'root'
  })
export class OrderService {

    constructor(
        private httpClient: HttpClient
    ){}

    GetUserOrders(){
        return this.httpClient.get<Order>(environment.backendUrl + 'api/order')
    }

    GetRestaurantOrders(){
        return this.httpClient.get<Order>(environment.backendUrl + 'api/order/restaurant')
    }

    AddOrderToUser(order: Basket){
        return this.httpClient.post<string>(environment.backendUrl + 'api/order', order)
    }

    AddOrderToRestaurants(order: Basket){
        let alreadySent: string[] = []
        let results: Array<Observable<string>> = []
        results.push(this.AddOrderToUser(order))
        for (let i = 0; i < order.foods.length; i++) {
            if(alreadySent.indexOf(order.foods[i].restaurantUrl) == -1){
                let subRes: Basket = {...order};
                subRes.foods =  subRes.foods.filter(filter => filter.restaurantUrl == order.foods[i].restaurantUrl)
                let split = order.foods[i].restaurantUrl.split('/')
                let restaurantId = split[split.length -1]
                results.push(this.httpClient.post<string>(environment.backendUrl + `api/order/${restaurantId}`, order))
                alreadySent.push(order.foods[i].restaurantUrl)
            }
        }
        return forkJoin(results)
    }
  }