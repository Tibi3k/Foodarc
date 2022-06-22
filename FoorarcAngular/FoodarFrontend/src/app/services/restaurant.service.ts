import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Apollo, gql } from "apollo-angular";
import { map } from "rxjs";
import { environment } from "src/environments/environment";
import { CreateFood, Food } from "../model/food.model";
import { CreateRestaurant, Restaurant } from "../model/restaurant.model";


const GET_ALL_RESTAURANTS = gql`
    query GetAllRestaurants {
        GetAllRestaurants {
          name,
          description,
          address,
          city,
          country,
          zipCode,
          imagePath,
          ownerId,
          id
        }
      }`

const GET_DETAILS = gql`
      query GetDetailsOfRestaurant($restaurantId: String!) {
        GetDetailsOfRestaurant(restaurantId: $restaurantId) {
            name,
            description,
            address,
            city,
            country,
            zipCode,
            imagePath,
            ownerId,
            id,
            availableFoods{
                id,
                name,
                description,
                imagePath,
                calories,
                price
            }
          }
        }`

const UPDATE_RESTAURANT = gql`
    mutation($editRestaurant: RestaurantInput!){
        UpdateRestaurant(editRestaurant: $editRestaurant){
            name,
            description,
            address,
            city,
            country,
            zipCode,
            imagePath
        }
    }`

const CREATE_RESTAURANT = gql`
    mutation($createRestaurant: CreateRestaurantInput!){
        CreateRestaurant(createRestaurant: $createRestaurant){
            name,
            description,
            address,
            city,
            country,
            zipCode,
            imagePath
        }
    }`

const DELETE_RESTAURANT = gql`
    mutation {
        DeleteRestaurant {
            id
        }
    }`

@Injectable({
    providedIn: 'root'
})
export class RestaurantService {

    constructor(
        private httpClient: HttpClient,
        private apollo: Apollo
    ){}

    CreateRestaurant(restaurant: CreateRestaurant){
        //return this.httpClient.post<string>(environment.backendUrl + 'api/restaurant', restaurant)
        console.log('mutate')
        return this.apollo.mutate({
            mutation: CREATE_RESTAURANT, variables: {createRestaurant: restaurant},
            awaitRefetchQueries: true,
            refetchQueries: [
                {
                    query: GET_ALL_RESTAURANTS,
                    
                }
            ]
        })
    }

    GetRestaurantById(restId: string){
        //return this.httpClient.get<Restaurant | null>(environment.backendUrl + `api/restaurant/${restaurantId}`)
        return this.apollo.query<{GetDetailsOfRestaurant: Restaurant}>({
            query: GET_DETAILS, 
            variables: {restaurantId: restId},
            fetchPolicy: "network-only"
            }).pipe(map(data => data.data.GetDetailsOfRestaurant))
    }
    

    GetAllRestaurants(){
        //return this.httpClient.get<Restaurant[]>(environment.backendUrl + 'api/restaurant')
        return this.apollo.query<{GetAllRestaurants: Restaurant[]}>({
            query: GET_ALL_RESTAURANTS,
            fetchPolicy: "network-only"
            }).pipe(map(data => data.data.GetAllRestaurants))
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
        //return this.httpClient.put<string>(environment.backendUrl + 'api/restaurant', restaurant)
        restaurant.orders = []
        restaurant.availableFoods = []
        console.log('mutate')
        return this.apollo.mutate({
            mutation: UPDATE_RESTAURANT, variables: {editRestaurant: restaurant},
            awaitRefetchQueries: true,
            refetchQueries: [
                {
                    query: GET_ALL_RESTAURANTS,
                    
                },
                {
                    query: GET_DETAILS,
                    variables: {restaurantId: restaurant.id}
                },

            ]
        })
    }

    DeleteFood(foodId: string){
        return this.httpClient.delete<string>(environment.backendUrl + `api/restaurant/food/${foodId}`)
    }
    
    DeleteRestaurant(){
        //return this.httpClient.delete<string>(environment.backendUrl + 'api/restaurant')
        return this.apollo.mutate({
            mutation: DELETE_RESTAURANT,
            awaitRefetchQueries: true,
            refetchQueries: [
                {
                    query: GET_ALL_RESTAURANTS,
                }
            ]
        })
    }
}