export class Category {
    public id: string;
    public name: string;
    public subcategories: Category[];

    constructor(id: string, name: string, subcategories: Category[]){
        this.id = id;
        this.name = name;
        this.subcategories = subcategories;
    }
}