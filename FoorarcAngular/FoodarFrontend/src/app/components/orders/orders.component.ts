import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Order } from 'src/app/model/order.model';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  constructor(
    private orderService: OrderService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  //mode = 'restaurant'
  currentOrders: Order | null = null
  ngOnInit(): void {
    this.route.paramMap.subscribe(paramMap => 
      {
        if(paramMap.has('restaurantId')){
          var id = paramMap.get('restaurantId')
          this.orderService.GetRestaurantOrders()
            .subscribe(order => this.currentOrders = order)
        } else if(paramMap.has('userId')){
          this.orderService.GetUserOrders()
            .subscribe(order => this.currentOrders = order)
        } else {
          this.router.navigate([''])
        }
      })
  }

}
