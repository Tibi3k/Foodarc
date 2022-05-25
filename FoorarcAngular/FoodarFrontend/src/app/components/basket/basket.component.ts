import { Component, OnInit } from '@angular/core';
import { Basket } from 'src/app/model/basket.model';
import { BasketService } from 'src/app/services/basket.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {

  constructor(private basketService: BasketService, private orderService: OrderService) { }
  userBasket: Basket | null = null
  ngOnInit(): void {
    this.loadUserBasket()
  }

  onDeleteFood(id: string){
    this.basketService.DeleteFoodFromBasket(id)
      .subscribe(result => {
        this.loadUserBasket()
      })
  }

  loadUserBasket(){
    this.basketService.GetUserBakset()
     .subscribe(result => this.userBasket = result)
  }

  orderBasket(){
    this.orderService.AddOrderToRestaurants(this.userBasket!)
      .subscribe(result => {
        this.basketService.DeleteBasket().subscribe(
          result => {
            this.loadUserBasket()
          }
        )
      })

  }

}
