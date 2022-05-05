export class User {
    public id: string;
    public name: string;
    public email: string;
    public role: string;
    public image?: File;

    constructor(id: string, name: string, email: string, role: string, image: File){
        this.id = id;
        this.name = name;
        this.email = email;
        this.role = role;
        this.image = image;
    }
}