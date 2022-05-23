import { HttpClient } from "@angular/common/http"
import { Injectable } from "@angular/core"
import { environment } from "src/environments/environment"

@Injectable({
    providedIn: 'root'
})
export class ImageService {

    constructor(private http:HttpClient ) {}

    uploadImage(file: File, resolve: any, reject: any){
        const url = `https://api.imgbb.com/1/upload?key=${environment.imgbbApiKey}`
        const form = new FormData()
        form.append("image", file)
        this.http
          .post(url, form)
          .forEach(res => {
              resolve(res)
          })
          .catch((err) => {
              reject(err)
          })
    }
}