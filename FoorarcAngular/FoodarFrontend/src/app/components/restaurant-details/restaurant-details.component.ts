import { Component, OnInit } from '@angular/core';
import { Restaurant } from 'src/app/model/restaurant.model';
import { AuthService } from 'src/app/services/auth.service';
import { AccountInfo } from '@azure/msal-browser'
import { RestaurantService } from 'src/app/services/restaurant.service';
@Component({
  selector: 'app-restaurant-details',
  templateUrl: './restaurant-details.component.html',
  styleUrls: ['./restaurant-details.component.css']
})
export class RestaurantDetailsComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private restaurantService: RestaurantService
  ) { }

  currentUser: AccountInfo | null = null
  restaurant: Restaurant | null = null
  role = ''
  ngOnInit(): void {
    this.authService.getCurrentUserListener()
      .subscribe(user => {
        this.currentUser = user 
        if(user != null){
          this.role = user?.idTokenClaims!['external_Position'] as string ?? ''
        } else {
          this.role = ''
        }
      })
    this.restaurantService.GetRestaurantById()
      .subscribe(result => this.restaurant = result)
      
  }

}
