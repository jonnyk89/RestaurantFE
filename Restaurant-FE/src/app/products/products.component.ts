import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriesService } from '../categories/categories.service';
import { Category } from '../categories/category.model';
import { Product } from './product.model';
import { ProductsService } from './products.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  productFilter: string;
  categories: Category[];
  products: Product[];
  getProductsForm: FormGroup;
  priorityName = false;
  ascendingName = false;
  priorityCategory = false;
  ascendingCategory = false;
  priorityPrice = false;
  ascendingPrice = false;
  filtered = false;
  page = 1;
  isLoading = false;
  totalProducts = 0;

  // sorting
  nameSortClicks: number = 0;
  categorySortClicks: number = 0;
  categorySortAsc = false;
  categorySortDesc = false;


  constructor(private categoriesService: CategoriesService, private productsService: ProductsService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(){
    this.page = 1;
    this.isLoading = true;
    this.categoriesService.getAllCategories().subscribe(response => {
      this.categories = response;
      console.log(this.categories);
    });
    
    this.productsService.getAllProducts().subscribe(response =>{
      this.products = response;
      this.totalProducts = this.products.length;
      this.filtered = false;
      this.isLoading = false;
    });

    this.getProductsForm = new FormGroup({
      'productNameFilter': new FormControl(null),
      'categoryFilter': new FormControl(null),
    });
    this.getProductsForm.controls['categoryFilter'].setValue('All', {onlySelf: true})
  }

  onFilter(productName: string, categoryId: string){
    if(categoryId === 'All'){
      categoryId = null;
    }
    if(productName === ''){
      productName = null;
    }

    console.log(categoryId);

    if(productName === null && categoryId === null){
      this.productsService.getAllProducts().subscribe(response =>{
        this.products = response;
        this.totalProducts = this.products.length;
        this.filtered = false;
      });
    }
    else{
      this.productsService.getFilteredProducts(productName, categoryId, this.page, 20, null, null).subscribe(response => {
        this.products = response;
        this.productsService.getFilteredProducts(productName, categoryId, 1, null, null, null).subscribe(response => {
          this.totalProducts = response.length;
        });
        this.filtered = true;
      });
    }

  }

  onClickNameSort(){
    this.categorySortClicks = 0;
    let sortBy = "product";
    let sortDirection = null;
    let productName = this.getProductsForm.controls['productNameFilter'].value;
    let categoryName = this.getProductsForm.controls['categoryFilter'].value;
    let categoryId = categoryName === 'All' ? null : this.categories.filter(c => c.name === categoryName)[0].id;
    
    if(this.nameSortClicks === 2){
      this.nameSortClicks = 0;
      this.productsService.getFilteredProducts(productName, categoryId, this.page, 20, null, null).subscribe(response =>{
        this.products = response;
        this.filtered = false;
        console.log(this.products);
      });
    }
    else{
      this.nameSortClicks++;
      if(this.nameSortClicks === 0){
        sortBy = null
        sortDirection = null;
      }
      else if(this.nameSortClicks === 1){
        sortDirection = "Ascending";
      }
      else if(this.nameSortClicks === 2){
        sortDirection = "Descending";
      }
      this.productsService.getFilteredProducts(productName, categoryId, this.page, 20, sortBy, sortDirection).subscribe(response =>{
        this.products = response;
        this.filtered = true;
        console.log(this.products);
      });
    }
  }

  onClickCategorySort(){
    this.nameSortClicks = 0;
    let sortBy = "category";
    let sortDirection = null;
    let productName = this.getProductsForm.controls['productNameFilter'].value;
    let categoryName = this.getProductsForm.controls['categoryFilter'].value;
    let categoryId = categoryName === 'All' ? null : this.categories.filter(c => c.name === categoryName)[0].id;
    
    if(this.categorySortClicks === 2){
      this.categorySortClicks = 0;
      this.productsService.getFilteredProducts(productName, categoryId, this.page, 20, null, null).subscribe(response =>{
        this.products = response;
        this.filtered = false;
        console.log(this.products);
      });
    }
    else{
      this.categorySortClicks++;
      if(this.categorySortClicks === 0){
        sortBy = null
        sortDirection = null;
      }
      else if(this.categorySortClicks === 1){
        sortDirection = "Ascending";
      }
      else if(this.categorySortClicks === 2){
        sortDirection = "Descending";
      }
      this.productsService.getFilteredProducts(productName, categoryId, this.page, 20, sortBy, sortDirection).subscribe(response =>{
        this.products = response;
        this.filtered = true;
        console.log(this.products);
      });
    }
  }

  onPageChanged(page: number){
    let productName = this.getProductsForm.controls['productNameFilter'].value;
    let categoryName = this.getProductsForm.controls['categoryFilter'].value;
    let categoryId = categoryName === 'All' ? null : this.categories.filter(c => c.name === categoryName)[0].id;
    let sortBy = null;
    let sortDirection = null;

    if(this.nameSortClicks === 1){
      sortBy = "product";
      sortDirection = "Ascending";
    }
    else if(this.nameSortClicks === 2){
      sortBy = "product";
      sortDirection = "Descending";
    }
    
    if(this.categorySortClicks === 1){
      sortBy = "category";
      sortDirection = "Ascending";
    }
    else if(this.categorySortClicks === 2){
      sortBy = "category";
      sortDirection = "Descending";
    }
    console.log(this.nameSortClicks);
    console.log(this.categorySortClicks);
    console.log(sortBy);
    console.log(sortDirection);
    this.productsService.getFilteredProducts(productName, categoryId, page, 20, sortBy, sortDirection).subscribe(response =>{
      this.products = response;
      this.filtered = false;
    });
    this.page = page;
  }

  onEditProduct(id: string){
    this.router.navigate(['edit', id], {relativeTo: this.route})
  }

  onDeleteProduct(id: string){
    this.productsService.deleteProduct(id).subscribe(response => {
      if(response.message === '_Product successfully deleted_'){
        let productName = this.getProductsForm.controls['productNameFilter'].value;
        let categoryName = this.getProductsForm.controls['categoryFilter'].value
        this.onFilter(productName, categoryName);
      }
    });
  }
}
