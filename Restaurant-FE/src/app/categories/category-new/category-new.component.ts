import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CategoriesService } from '../categories.service';
import { Category } from '../category.model';

@Component({
  selector: 'app-category-new',
  templateUrl: './category-new.component.html',
  styleUrls: ['./category-new.component.css']
})
export class CategoryNewComponent implements OnInit {
  name: string;
  categories: Category[];
  newCategoryForm: FormGroup;
  category: Category;
  subcategory: Category;
  
  // error messages
  nameError = 'Name is required and must be less than 100 characters.'
  categoryError = 'Please select a category.';
  constructor(private categoriesService: CategoriesService, private router: Router) { }


  ngOnInit(): void {
    this.initForm();
  }

  private initForm(){
    this.categoriesService.getAllCategories().subscribe(response => {
      this.categories = response;
      console.log(this.categories);
    });

    this.newCategoryForm = new FormGroup({
      'name': new FormControl(null, Validators.required),
      'parentCategory': new FormControl(null, Validators.required)
    });
    this.newCategoryForm.controls['parentCategory'].setValue('None', {onlySelf: true});
  }

  onSubmit(){
    let name = this.newCategoryForm.get('name').value;
    let parentId = this.newCategoryForm.get('parentCategory').value;
    if(parentId === 'None'){
      parentId = null;
    }
    this.categoriesService.createNewCategory(name, parentId).subscribe(response => {
      if(response.message === '_Category already exists_'){
        this.nameError = 'Category already exists.'
        alert(this.nameError);
        console.log(this.nameError);
      }
      else if(response.message === '_Category successfully created_'){
        this.router.navigate(['/categories']);
      }
    });
  }

}
