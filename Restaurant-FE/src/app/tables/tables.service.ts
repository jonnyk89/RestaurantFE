import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Order } from '../orders/order.model';

export interface GetTablesResponseData{
  id: number,
  status: string,
  capacity: number,
  waiter: string,
  bill: string,
}

export interface GetTableResponseData{
  id: number,
  status: string,
  capacity: number,
  waiter: string,
  bill: string,
  orders: Order[],
}

@Injectable({
  providedIn: 'root'
})
export class TablesService {

  constructor(private _http: HttpClient) { }

  getAllTables(){
    return this._http.get<GetTablesResponseData[]>("https://localhost:7162/api/Tables", {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  getTable(id: number){
    return this._http.get<GetTableResponseData>("https://localhost:7162/api/Tables/" + id, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }
}
