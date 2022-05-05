import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriesService } from '../categories.service';
import { Category } from '../category.model';

@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  styleUrls: ['./category-edit.component.css']
})
export class CategoryEditComponent implements OnInit {
  id: string;
  name: string;
  categories: Category[];
  editcategoryForm: FormGroup;
  currentCategoryName: string;
  oldName: string;

  // error messages
  nameError = 'Name is required and must be less than 100 characters.'

  constructor(private categoriesService: CategoriesService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(){
    this.id = this.route.snapshot.params['id'];

    this.categoriesService.getAllCategories().subscribe(response => {
      this.categories = response;
      console.log(this.categories);
    });

    this.categoriesService.getTargetCategory(this.id).subscribe(category => {
      this.oldName = category['name'];
      console.log(category);
      console.log(category['name']);
    });

    this.editcategoryForm = new FormGroup({
      'name': new FormControl(null),
      'parentCategory': new FormControl(null)
    });
    this.editcategoryForm.controls['parentCategory'].setValue('None', {onlySelf: true});
  }

  onSubmit(){
    let name = this.editcategoryForm.get('name').value;
    let parentId = this.editcategoryForm.get('parentCategory').value;
    if(parentId === 'None'){
      parentId = null;
    }
    this.categoriesService.updateCategory(this.id, name, parentId).subscribe(response => {
      if(response.message === '_Category already exists_'){
        this.nameError = 'Category already exists.'
        alert(this.nameError);
        console.log(this.nameError);
      }
      else if(response.message === '_Category successfully updated_'){
        this.router.navigate(['/categories']);
      }
    });
  }

}
