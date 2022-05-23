import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { AccountInfo } from '@azure/msal-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  public userIsAuthenticated = false;
  currentUser: AccountInfo | null = null
  authenticatedUserSubscription: Subscription | undefined
  userRole = ''
  constructor(public authService: AuthService, private router: Router){}

  ngOnInit(){
    this.authenticatedUserSubscription =  this.authService.getCurrentUserListener()
    .subscribe(user => {
      console.log("header user:",user)
      if(user != null)
        this.userRole = user.idTokenClaims?.['extension_Position'] as string ?? ''
      this.currentUser = user
      console.log(this.currentUser)
    })
  }

  onLoginClicked(){
    console.log('login')
    if(this.currentUser == null)
      this.authService.login()
    else{
      this.authService.logOut()
       .then(_ => {
        this.router.navigate([''])
       })
    }
    
  }
}
