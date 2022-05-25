import { Component, OnInit } from '@angular/core';
import { Restaurant } from 'src/app/model/restaurant.model';
import { RestaurantService } from 'src/app/services/restaurant.service';

@Component({
  selector: 'app-list-restaurants',
  templateUrl: './list-restaurants.component.html',
  styleUrls: ['./list-restaurants.component.css']
})
export class ListRestaurantsComponent implements OnInit {

  constructor(private restaurantService: RestaurantService) { }
  restaurants: Restaurant[] = []
  ngOnInit(): void {
    console.log('asdf')
    this.restaurantService.GetAllRestaurants()
      .subscribe(result => {this.restaurants = result
        console.log(this.restaurants)
      })
  }

}
