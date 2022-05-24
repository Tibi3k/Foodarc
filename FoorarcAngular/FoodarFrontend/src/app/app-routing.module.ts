import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateFoodComponent } from './components/create-food/create-food.component';
import { CreateRestaurantComponent } from './components/create-restaurant/create-restaurant.component';
import { ListRestaurantsComponent } from './components/list-restaurants/list-restaurants.component';
import { RestaurantDetailsComponent } from './components/restaurant-details/restaurant-details.component';

const routes: Routes = [
  {path: '', component: ListRestaurantsComponent},
  {path: 'restaurant/:restaurantId', component: RestaurantDetailsComponent},
  {path: 'createrestaurant', component: CreateRestaurantComponent},
  {path: 'editrestaurant/:restaurantId', component: CreateRestaurantComponent},
  {path: 'createfood', component: CreateFoodComponent},
  {path: 'editfood/:foodId', component: CreateFoodComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
