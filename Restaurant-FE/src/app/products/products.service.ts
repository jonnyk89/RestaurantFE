import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

export interface GetProductsResponseData {
  productId: string,
  name: string,
  description: string,
  categoryId: string,
  categoryName: string,
  price: number,
}

export interface ProductResponseData {
  message: string,
}

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(private _http: HttpClient) { }

  getAllProducts(){
    return this._http.get<GetProductsResponseData[]>("https://localhost:7162/api/Products", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  getFilteredProducts(productName: string, categoryId: string, page: number, pageSize: number, sortBy: string, sortDirection: string){
    let params = new HttpParams();
    if(productName != null){
      params = params.append('product', productName);
    }
    if(categoryId != null){
      params = params.append('categoryId', categoryId);
    }
    params = params.append('page', page === null ? 0 : page);
    params = params.append('pageSize', pageSize === null ? 0 : pageSize);
    if(sortBy != null){
      params = params.append('sortBy', sortBy);
    }
    if(sortDirection != null){
      params = params.append('sortDirection', sortDirection);
    }
    return this._http.get<GetProductsResponseData[]>("https://localhost:7162/api/Products", {
      params: params
    });
  }

  getProduct(id: string){
    return this._http.get<GetProductsResponseData>("https://localhost:7162/api/Products/" + id);
  }

  createNewProduct(name: string, categoryId: string, description: string, price: number){
    if(description === null){
      description = '';
    }

    return this._http.post<ProductResponseData>("https://localhost:7162/api/Products", {
      name,
      description,
      price,
      categoryId,
    })
  }

  updateProduct(id: string, name: string, categoryId: string, description: string, price: number){
    if(description === null){
      description = '';
    }

    return this._http.put<ProductResponseData>("https://localhost:7162/api/Products/" + id, {
      name,
      description,
      price,
      categoryId,
    })
  }

  deleteProduct(id: string){
    return this._http.delete<ProductResponseData>("https://localhost:7162/api/Products/" + id);
  }
}
