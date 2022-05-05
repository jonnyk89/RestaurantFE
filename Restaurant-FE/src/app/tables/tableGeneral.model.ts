export class TableGeneral{
    public id: number;
    public status: string;
    public capacity: number;
    public waiter: string;
    public bill: string;

    constructor(id: number, status: string, capacity: number, waiter: string, bill: string){
        this.id = id;
        this.status = status;
        this.capacity = capacity;
        this.waiter = waiter;
        this.bill = bill;
    }
}