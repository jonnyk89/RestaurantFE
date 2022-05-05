export class OrderProduct{
    public name: string;
    public quantity: number;
    public price: number;
    public totalPrice: number;

    constructor(name: string, quantity: number, price: number, totalPrice: number){
        this.name = name;
        this.quantity = quantity;
        this.price = price;
        this.totalPrice = totalPrice;
    }
}