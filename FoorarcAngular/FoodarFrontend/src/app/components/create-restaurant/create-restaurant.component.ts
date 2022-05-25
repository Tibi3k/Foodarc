import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { CreateRestaurant, Restaurant } from 'src/app/model/restaurant.model';
import { AuthService } from 'src/app/services/auth.service';
import { ImageService } from 'src/app/services/image.service';
import { RestaurantService } from 'src/app/services/restaurant.service';

@Component({
  selector: 'app-create-restaurant',
  templateUrl: './create-restaurant.component.html',
  styleUrls: ['./create-restaurant.component.css']
})
export class CreateRestaurantComponent implements OnInit {
  form!: FormGroup
  constructor(
    public route: ActivatedRoute,
    public authService: AuthService,
    private router: Router,
    private imageService: ImageService,
    private restaurantService: RestaurantService
  ) {}

  mode = 'create'
  restaurantId: string | null = null
  imagePreview = ""
  oldRestaurant: Restaurant | null = null

  ngOnInit() {
    this.createForm()
    this.route.paramMap.subscribe((paramMap: ParamMap) => {
      if (paramMap.has('restaurantId')) {
        this.restaurantId =  paramMap.get('restaurantId')!;
        this.mode = 'edit'
        this.restaurantService.GetRestaurantById(this.restaurantId)
          .subscribe(restaurant => {
            this.oldRestaurant = restaurant
            this.loadItemToForm(restaurant!)
          })
      }
    })
  }

  onSaveRestaurant(){
    if(this.mode == 'create'){
      let restaurant = this.getNewRestaurantFromForm()
      this.imageService.uploadImage(this.form.value.image, 
        (responseData: any) => {
          restaurant.imagePath = responseData.data.url
          this.restaurantService.CreateRestaurant(restaurant)
          .subscribe(result => {
            console.log('added')
            this.router.navigate([''])
          })
        },
        () => {}
      )
    } else {
      //update
      let restaurant = this.getExistingRestaurantFromForm()
      if(restaurant.imagePath != this.oldRestaurant?.imagePath){
        this.imageService.uploadImage(this.form.value.image,
          (result: any) => {
            restaurant.imagePath = result.data.url
            this.restaurantService.UpdateRestaurant(restaurant)
              .subscribe(result => {
                this.router.navigate([''])
              })
          },
          () => {}
          )
      } else {
        console.log('ressssssss', restaurant)
        this.restaurantService.UpdateRestaurant(restaurant)
          .subscribe(result => {
            this.router.navigate([''])
          })
      }
    }
  }

  getNewRestaurantFromForm(): CreateRestaurant{
    return {
      name: this.form.value.name,
      description: this.form.value.description,
      address: this.form.value.address,
      imagePath: '',
      country: this.form.value.country,
      zipCode: this.form.value.zipcode,
      city: this.form.value.city,
    }
  }

  getExistingRestaurantFromForm(): Restaurant{
    return {
      id: this.restaurantId!,
      name: this.form.value.name,
      description: this.form.value.description,
      address: this.form.value.address,
      imagePath: this.form.value.image,
      ownerId: this.restaurantId!,
      availableFoods: this.oldRestaurant!.availableFoods,
      country: this.form.value.country,
      zipCode: this.form.value.zipcode,
      city: this.form.value.city,
      orders: this.oldRestaurant!.orders
    }
  }

  loadItemToForm(restaurant: Restaurant){
    this.imagePreview = restaurant.imagePath
    this.form.setValue({
      name: restaurant.name,
      description: restaurant.description ?? '',
      address: restaurant.address,
      image: restaurant.imagePath,
      country: restaurant.country,
      zipcode: restaurant.zipCode,
      city: restaurant.city
    });
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
