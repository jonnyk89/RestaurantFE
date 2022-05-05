import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from './category.model';

export interface GetCategoriesResponseData {
  id: string,
  name: string,
  subcategories: Category[];
}

export interface CategoryResponseData {
  message: string,
}

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor(private _http: HttpClient) { }

  getAllCategories(){
    return this._http.get<GetCategoriesResponseData[]>("https://localhost:7162/api/Categories", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  getTargetCategory(id: string){
    return this._http.get<GetCategoriesResponseData>("https://localhost:7162/api/Categories/" + id, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  createNewCategory(name: string, parentId: string){
    return this._http.post<CategoryResponseData>("https://localhost:7162/api/Categories", {
      name,
      parentId,
    }, 
    {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    })
  }

  updateCategory(id: string, name: string, parentId: string){
    return this._http.put<CategoryResponseData>("https://localhost:7162/api/Categories/" + id, {
      name,
      parentId,
    }, 
    {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    })
  }

  deleteCategory(id: string){
    return this._http.delete<CategoryResponseData>("https://localhost:7162/api/Categories/" + id, 
    {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    })
  }
}
