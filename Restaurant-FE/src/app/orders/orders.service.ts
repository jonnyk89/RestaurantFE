import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Order } from './order.model';
import { OrderProduct } from './orderProduct.model';

export interface GetOrdersResponseData{
  id: string,
  tableId: number,
  waiter: string,
  dateTime: string,
  status: string,
  price: number,
  products: OrderProduct[];
}

export interface OrderResponseData {
  message: string,
}

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  constructor(private _http: HttpClient) { }

  getFilteredOrders(userName: string, tableId: number, status: string, page: number, pageSize: number, sortBy: string, sortDirection: string){
    let params = new HttpParams();
    if(userName != null || userName === ''){
      params = params.append('userName', userName);
    }
    params = params.append('tableId', tableId === null ? 0 : tableId);
    if(status != null){
      params = params.append('status', status);
    }
    params = params.append('page', page === null ? 0 : page);
    params = params.append('pageSize', 20);
    if(sortBy != null){
      params = params.append('sortBy', sortBy);
    }
    if(sortDirection != null){
      params = params.append('sortDirection', sortDirection);
    }

    return this._http.get<GetOrdersResponseData[]>("https://localhost:7162/api/Orders", {
      params: params
    });
  }

  getOrder(orderId: string){
    return this._http.get<GetOrdersResponseData>("https://localhost:7162/api/Orders/" + orderId, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  createOrder(tableId: number, products: {productId: string, quantity: number}[]){
    return this._http.post<OrderResponseData>("https://localhost:7162/api/Orders", {
      tableId,
      products
    })
  }

  updateOrder(orderId: string, userId : string, tableId: number, products: {productId: string, quantity: number}[]){
    return this._http.put<OrderResponseData>("https://localhost:7162/api/Orders/" + orderId, {
      userId,
      tableId,
      products
    })
  }

  deleteOrder(orderId: string){
    return this._http.delete<OrderResponseData>("https://localhost:7162/api/Orders/" + orderId);
  }

  completeOrder(orderId: string){
    return this._http.put<OrderResponseData>("https://localhost:7162/api/Orders/" + orderId + "/Complete", {
      
    })
  }
}
