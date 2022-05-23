import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ImageService } from 'src/app/services/image.service';
import { RestaurantService } from 'src/app/services/restaurant.service';
import { AccountInfo  } from '@azure/msal-browser';
import { CreateFood, Food } from 'src/app/model/food.model';

@Component({
  selector: 'app-create-food',
  templateUrl: './create-food.component.html',
  styleUrls: ['./create-food.component.css']
})
export class CreateFoodComponent implements OnInit {

  form!: FormGroup
  constructor(
    public route: ActivatedRoute,
    public authService: AuthService,
    private router: Router,
    private imageService: ImageService,
    private restaurantService: RestaurantService
  ) {}

  mode = 'create'
  categories: Array<any> = ['asdf', 'dfasdf', 'asdfasdf']
  foodId = ''
  imagePreview = ""
  currentUser: AccountInfo | null = null
  ngOnInit() {
    this.createForm()
    this.authService.getCurrentUserListener()
      .subscribe(user => {
        this.currentUser = user
        this.parseFoodFromUrl()
      })
  }

  parseFoodFromUrl(){
    this.route.paramMap.subscribe((paramMap: ParamMap) => {
      if (paramMap.has('foodId')) {
        this.foodId =  paramMap.get('foodId')!;
        this.mode = 'edit'
        this.restaurantService.GetFoodById(this.currentUser?.idTokenClaims?.oid!, this.foodId)
          .subscribe(food => {
            this.loadItemToForm(food)
          })
      }
    })
  }

  onSaveRestaurant(){
    let food = this.getNewFoodFromForm()
    this.imageService.uploadImage(this.form.value.image, 
      (responseData: any) => {
        // console.log('res', responseData)
        food.imagePath = responseData.data.url
        // console.log('rd.data.url', responseData.data.url)
        // console.log(' restaurant.imagePath',  restaurant.imagePath)
        // console.log('restaurant', restaurant)
        this.restaurantService.AddFoodToRestaurant(food)
        .subscribe(result => {
          console.log('added')
          this.router.navigate([''])
        })
      },
      () => {}
      )
    // if(this.mode == 'edit'){
    //   let product = this.getExistingProductFromForm()
    //   this.backendService.editProduct(product).subscribe(res => {
    //     this.router.navigate([''])
    //   })
    // } else {
    //   let product = this.getNewProductFromForm()
    //   this.backendService.createProduct(product).subscribe(res => {
    //     this.router.navigate([''])
    //   })
    // }
  }

  getNewFoodFromForm(): CreateFood{
    return {
      name: this.form.value.name,
      description: this.form.value.description,
      price: this.form.value.price,
      calories: this.form.value.calories,
      imagePath: ''
    }
  }

  // getExistingProductFromForm(): Product{
  //   return {
  //     id: this.productId!,
  //     name: this.form.value.name,
  //     description: this.form.value.description,
  //     price: this.form.value.price,
  //     quantity: this.form.value.quantity,
  //     category: this.form.value.category
  //   }
  // }

  loadItemToForm(product: Food){
  //   this.form.setValue({
  //     name: product.name,
  //     description: product.description ?? '',
  //     price: product.price,
  //     quantity: product.quantity,
  //     category: product.category,
  //   });
  }

  createForm(){
    this.form = new FormGroup({
      name: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(3)],
      }),
      description: new FormControl(null, { validators: [Validators.required] }),
      zipcode: new FormControl(null, {validators: [Validators.required, Validators.minLength(4), Validators.maxLength(4)],}),
      city: new FormControl(null, { validators: [Validators.required] }),
      address: new FormControl(null, { validators: [Validators.required] }),
      country: new FormControl(null, { validators: [Validators.required] }),
      image: new FormControl(null),
    });
  }

  onCancelClicked(){
    this.router.navigate([''])
  }

  onImagePicked(event: Event) {
    const file = (event.target as HTMLInputElement).files![0];
    this.form.patchValue({ image: file });
    this.form.get('image')!.updateValueAndValidity();
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result as string;
    };
    reader.readAsDataURL(file);
  }
}