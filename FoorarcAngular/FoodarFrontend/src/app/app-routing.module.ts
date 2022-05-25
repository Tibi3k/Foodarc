import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BasketComponent } from './components/basket/basket.component';
import { CreateFoodComponent } from './components/create-food/create-food.component';
import { CreateRestaurantComponent } from './components/create-restaurant/create-restaurant.component';
import { ListRestaurantsComponent } from './components/list-restaurants/list-restaurants.component';
import { OrdersComponent } from './components/orders/orders.component';
import { RestaurantDetailsComponent } from './components/restaurant-details/restaurant-details.component';

const routes: Routes = [
  { path: '', component: ListRestaurantsComponent },
  { path: 'restaurant/:restaurantId', component: RestaurantDetailsComponent },
  { path: 'createrestaurant', component: CreateRestaurantComponent },
  { path: 'editrestaurant/:restaurantId', component: CreateRestaurantComponent },
  { path: 'createfood', component: CreateFoodComponent },
  { path: 'editfood/:foodId', component: CreateFoodComponent },
  { path: 'basket', component: BasketComponent },
  { path: 'rorders/:restaurantId', component: OrdersComponent },
  { path: 'orders/:userId', component: OrdersComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
