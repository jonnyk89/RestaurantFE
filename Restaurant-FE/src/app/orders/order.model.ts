import { OrderProduct } from "./orderProduct.model";

export class Order {
    public id: string;
    public tableId: number;
    public waiter: string
    public dateTime: string;
    public status: string;
    public price: number;
    public products: OrderProduct[];

    constructor(id: string, tableId: number, waiter: string, dateTime: string, status: string, price: number, products: OrderProduct[]){
        this.id = id;
        this.tableId = tableId;
        this.waiter = waiter;
        this.dateTime = dateTime;
        this.status = status;
        this.price = price;
        this.products = products;
    }
}