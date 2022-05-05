import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CategoriesService } from 'src/app/categories/categories.service';
import { Category } from 'src/app/categories/category.model';
import { ProductsService } from '../products.service';

@Component({
  selector: 'app-product-new',
  templateUrl: './product-new.component.html',
  styleUrls: ['./product-new.component.css']
})
export class ProductNewComponent implements OnInit {
  name: string;
  description: string;
  newProductForm: FormGroup;
  categories: Category[];

  // error messages:
  nameError = 'Name is required and must be less than 100 characters.';
  categoryError = 'Please select a category';
  priceError = 'Price is required';
  constructor(private categoriesService: CategoriesService, private productsService: ProductsService, private router: Router) { }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(){
    this.categoriesService.getAllCategories().subscribe(response => {
      this.categories = response;
      console.log(this.categories);
    });

    this.newProductForm = new FormGroup({
      'name': new FormControl(null, Validators.required),
      'category': new FormControl(null, Validators.required),
      'description': new FormControl(null),
      'price': new FormControl(null, Validators.required),
    });
    this.newProductForm.controls['category'].setValue('select', {onlySelf: true});
  }

  onSubmit(){
    if(this.newProductForm.valid === true){

      let name = this.newProductForm.get('name').value;
      let category = this.newProductForm.get('category').value;
      let description = this.newProductForm.get('description').value;
      let price = this.newProductForm.get('price').value;
      
      this.productsService.createNewProduct(name, category, description, price).subscribe(response =>{
        if(response.message === '_Product successfully created_'){
          this.router.navigate(['/products']);
        }
      });
    }
  }
}
