import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
  categories: Array<any> = ['asdf', 'dfasdf', 'asdfasdf']
  productId: Number | null = null
  imagePreview = ""
  
  ngOnInit() {
     this.createForm()
    // this.backendService.getAllCategories()
    //   .subscribe(categories => this.categories = categories)
    // this.route.paramMap.subscribe((paramMap: ParamMap) => {
    //   if (paramMap.has('productId')) {
    //     this.productId =  Number.parseInt(paramMap.get('productId')!);
    //     this.mode = 'edit'
    //     this.backendService.getProductById(this.productId)
    //       .subscribe(product => {
    //         this.loadItemToForm(product)
    //       })
    //   }
    // })
  }

  onSaveRestaurant(){
    let restaurant = this.getNewRestaurantFromForm()
    this.imageService.uploadImage(this.form.value.image, 
      (responseData: any) => {
        console.log('res', responseData)
        restaurant.imagePath = responseData.data.url
        console.log('rd.data.url', responseData.data.url)
        console.log(' restaurant.imagePath',  restaurant.imagePath)
        console.log('restaurant', restaurant)
        this.restaurantService.CreateRestaurant(restaurant)
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

  getNewRestaurantFromForm(): CreateRestaurant{
    return {
      name: this.form.value.name,
      description: this.form.value.description,
      address: `${this.form.value.country} ${this.form.value.zipcode} ${this.form.value.city} ${this.form.value.address}`,
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

  // loadItemToForm(product: Product){
  //   this.form.setValue({
  //     name: product.name,
  //     description: product.description ?? '',
  //     price: product.price,
  //     quantity: product.quantity,
  //     category: product.category,
  //   });
  // }

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
