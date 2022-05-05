export class Product {
    public productId: string;
    public name: string;
    public description: string
    public categoryId: string;
    public categoryName: string;
    public price: number;

    constructor(id: string, name: string, description: string, categoryId: string, categoryName: string, price: number){
        this.productId = id;
        this.name = name;
        this.description = description;
        this.categoryId = categoryId;
        this.categoryName = categoryName;
        this.price = price;
    }
}