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
    private restaurantService: RestaurantService,
  ) {}

  mode = 'create'
  foodId = ''
  imagePreview = ""
  currentUser: AccountInfo | null = null
  //for edit mode
  oldFood: Food | null = null

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
            console.log('cufood', food)
            this.oldFood = food
            this.loadItemToForm(food)
          })
      }
    })
  }

  onSaveRestaurant(){
    if(this.mode == 'create'){
      let food = this.getNewFoodFromForm()
      this.imageService.uploadImage(this.form.value.image, 
        (responseData: any) => {
          food.imagePath = responseData.data.url
          this.restaurantService.AddFoodToRestaurant(food)
          .subscribe(result => {
            console.log('added')
            this.router.navigate([''])
          })
        },
        () => {}
        )
    } else {
      let food = this.getExistingProductFromForm()
      if(this.oldFood?.imagePath != this.form.value.image){
        this.imageService.uploadImage(this.form.value.image,
          (result: any) => {
            food.imagePath = result.data.url
            this.restaurantService.UpdateFood(food)
            .subscribe(restul => {
              this.router.navigate([''])
            })
          },
          () => {})
      } else {
        this.restaurantService.UpdateFood(food)
        .subscribe(restul => {
          this.router.navigate([''])
        })
      }
    }
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

  getExistingProductFromForm(): Food{
    return {
      id: this.foodId!,
      name: this.form.value.name,
      description: this.form.value.description,
      price: this.form.value.price,
      calories: this.form.value.calories,
      imagePath: ''
    }
  }

  loadItemToForm(product: Food){
    this.imagePreview = product.imagePath
    this.form.setValue({
      name: product.name,
      description: product.description ?? '',
      price: product.price,
      calories: product.calories,
      image: product.imagePath
    });
  }

  createForm(){
    this.form = new FormGroup({
      name: new FormControl(null, {
        validators: [Validators.required, Validators.minLength(3)],
      }),
      description: new FormControl(null, { validators: [Validators.required] }),
      price: new FormControl(null, {validators: [Validators.required, Validators.minLength(4), Validators.maxLength(4)],}),
      calories: new FormControl(null, { validators: [Validators.required] }),
      image: new FormControl(null),
    });
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