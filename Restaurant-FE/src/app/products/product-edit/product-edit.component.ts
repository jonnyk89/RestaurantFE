import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriesService } from 'src/app/categories/categories.service';
import { Category } from 'src/app/categories/category.model';
import { ProductsService } from '../products.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})
export class ProductEditComponent implements OnInit {
  id: string;
  name: string;
  description: string;
  editProductForm: FormGroup;
  categories: Category[];

  // error messages:
  categoryRequiredError = 'Product category id is required.';

  constructor(private categoriesService: CategoriesService, private productsService: ProductsService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(){
    this.id = this.route.snapshot.params['id'];
    
    this.categoriesService.getAllCategories().subscribe(response => {
      this.categories = response;
      console.log(this.categories);
    });

    this.editProductForm = new FormGroup({
      'name': new FormControl(null),
      'category': new FormControl(null),
      'description': new FormControl(null),
      'price': new FormControl(null),
    });
    this.editProductForm.controls['category'].setValue('select', {onlySelf: true});
  }

  onSubmit(){
    let name = this.editProductForm.get('name').value;
    let category = this.editProductForm.get('category').value;
    let description = this.editProductForm.get('description').value;
    let price = this.editProductForm.get('price').value;

    this.productsService.updateProduct(this.id, name, category, description, price).subscribe(response =>{
      if(response.message === '_Product successfully updated_'){
        this.router.navigate(['/products']);
      }
      else if(response.message === '_Product category is invalid_'){
        alert(this.categoryRequiredError);
      }
    });
  }

}
