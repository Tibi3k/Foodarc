import { Component, OnInit } from '@angular/core';
import { Restaurant } from 'src/app/model/restaurant.model';
import { AuthService } from 'src/app/services/auth.service';
import { AccountInfo } from '@azure/msal-browser'
import { RestaurantService } from 'src/app/services/restaurant.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { BasketService } from 'src/app/services/basket.service';
import { Food } from 'src/app/model/food.model';
import { BasketFood, CreateBasketFood } from 'src/app/model/basket.model';
@Component({
  selector: 'app-restaurant-details',
  templateUrl: './restaurant-details.component.html',
  styleUrls: ['./restaurant-details.component.css']
})
export class RestaurantDetailsComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private restaurantService: RestaurantService,
    private router: Router,
    private route: ActivatedRoute,
    private basketService: BasketService
  ) { }
  loadState = 'loading'
  currentUser: AccountInfo | null = null
  restaurant: Restaurant | null = null
  role = ''
  restaurantId: string = ''
  ngOnInit(): void {
    this.authService.getCurrentUserListener()
      .subscribe(user => {
        this.currentUser = user 
        if(user != null){
          this.role = user?.idTokenClaims!['extension_Position'] as string ?? ''
        } else {
          this.role = ''
        }
      })
    this.route.paramMap.subscribe(paramMap => {
      if(paramMap.has('restaurantId')){
        this.restaurantId = paramMap.get('restaurantId')!
        this.restaurantService.GetRestaurantById(this.restaurantId!)
        .subscribe(result => {
          this.loadState = 'loaded'
          this.restaurant = result
        })
      } else {
        this.router.navigate([''])
      }
    })

  }

  onDeleteFood(foodId: string){
    this.restaurantService.DeleteFood(foodId)
    .subscribe(result => {
        this.restaurantService.GetRestaurantById(this.restaurantId)
        .subscribe(result => {
          this.restaurant = result
        })
    })
  }

  onDeleteRestaurant(){
    this.restaurantService.DeleteRestaurant()
      .subscribe(resulr => {
        this.router.navigate([''])
      })
  }

  orderProduct(food: Food){
    let basketFood: CreateBasketFood = {
      restaurantUrl: this.router.url,
      orderedFood: food,
    }
    this.basketService.AddFoodToBasket(basketFood)
      .subscribe(restult => {
        console.log('added')
      })
  }

}
