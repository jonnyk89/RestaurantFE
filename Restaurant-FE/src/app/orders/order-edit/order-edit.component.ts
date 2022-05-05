import { Component, OnInit, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { CategoriesService } from 'src/app/categories/categories.service';
import { Category } from 'src/app/categories/category.model';
import { Product } from 'src/app/products/product.model';
import { ProductsService } from 'src/app/products/products.service';
import { TableGeneral } from 'src/app/tables/tableGeneral.model';
import { TablesService } from 'src/app/tables/tables.service';
import { User } from 'src/app/users/user.model';
import { UsersService } from 'src/app/users/users.service';
import { Order } from '../order.model';
import { OrderProduct } from '../orderProduct.model';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.css']
})
export class OrderEditComponent implements OnInit {
  id: string;
  orderNumber: number;
  isAdmin = false;
  waiter: string;
  editOrderForm: FormGroup;
  users: User[];
  tables: TableGeneral[];
  categories: Category[];
  filteredCategories: Category[];
  products: Product[];
  subProducts: Product[] = [];
  order: Order;
  tableCapacity: number;
  breadcrumbs = ['All'];
  selectedCategory: Category;
  allProducts: Product[];
  isCategorySelected = false;
  isLoading = false;
  page = 1;
  totalProducts = 20;
  


  constructor(private authService: AuthService,
              private usersService: UsersService, 
              private categoriesService: CategoriesService,
              private productsService: ProductsService,
              private ordersService: OrdersService,
              private tablesService: TablesService,
              private route: ActivatedRoute,
              private router: Router) { }

  ngOnInit(): void {
    const userData: {
      email: string;
      name: string;
      role: string;
      _token: string;
      _tokenExpirationDate: string;
    } = JSON.parse(localStorage.getItem('userData'));
    this.isAdmin = userData.role === "Admin" ? true : false;
    this.isLoading = true;
    this.initForm();
  }

  valueChanged(newValue, productName: string){
    this.order.products.filter(p => p.name === productName)[0].quantity = newValue;
    let newPrice = 0;
    for(let prod of this.order.products){
      newPrice = +(newPrice + (prod.price * prod.quantity)).toFixed(2);
    }
    this.order.price = newPrice;
  }

  private initForm(){
    let currentWaiter: string
    let currentTableId: number;
    this.id = this.route.snapshot.params['orderId'];
    this.orderNumber = this.route.snapshot.params['orderNumber'];

    this.editOrderForm = new FormGroup({
      'user': new FormControl(null),
      'table': new FormControl(null),
      'orderProductQuantity': new FormControl(null),
    });

    this.categoriesService.getAllCategories().subscribe(response => {
      this.categories = response;
      this.filteredCategories = this.categories.slice();
      this.isCategorySelected = false;
      console.log(this.categories);
    });

    this.productsService.getAllProducts().subscribe(response => {
      this.products = response;
      this.allProducts = response;
      console.log(this.products);
    });

    this.ordersService.getOrder(this.id).subscribe(response => {
      this.order = response;
      console.log(this.order);
      this.tablesService.getAllTables().subscribe(response => {
        this.tables = response.filter(t => t.status === "Free" || t.id === this.order.tableId);
        
        console.log(this.tables);
      });
      if(this.isAdmin === true){

        this.usersService.getAllUsers().subscribe(response => {
          this.users = response;
          console.log(this.users);
          currentWaiter = this.users.filter(obj => obj.name === this.order.waiter)[0].id;
          currentTableId = this.order.tableId;
          this.editOrderForm.controls['user'].setValue(currentWaiter + '', {onlySelf: true});
          this.editOrderForm.controls['table'].setValue(currentTableId + '', {onlySelf: true});
        });
      }
      else{
        this.tableCapacity = this.tables.filter(t => t.id === this.order.tableId)[0].capacity;
      }
      this.isLoading = false;
    });

    console.log(this.breadcrumbs);
  }

  onCategorySelect(id: string, name: string){
    this.breadcrumbs.push(name);
    this.selectedCategory = this.categoryDFS(this.filteredCategories, name);
    this.categories = this.selectedCategory.subcategories.length > 0 ? this.selectedCategory.subcategories : [this.selectedCategory];
    this.productsService.getAllProducts().subscribe(response => {
        this.getAllSubProducts(this.categories, response);
        this.products = this.subProducts.slice();
        this.subProducts.length = 0;
        console.log(this.products);
      });
    this.isCategorySelected = true;
  }

  onBreadcrumbSelect(name: string){
    let breadcrumb: string = this.breadcrumbSearch(name);
    if(breadcrumb === 'All'){
      this.categoriesService.getAllCategories().subscribe(response => {
        this.categories = response;
        this.filteredCategories = this.categories.slice();
        this.isCategorySelected = false;
        console.log(this.categories);
        this.productsService.getAllProducts().subscribe(response => {
          this.products = response;
          this.subProducts.length = 0;
          console.log(this.subProducts);
          console.log(this.products);
        });
      });
    }
    else{
      this.selectedCategory = this.categoryDFS(this.filteredCategories, name);
      this.categories = this.selectedCategory.subcategories;
      this.productsService.getAllProducts().subscribe(response => {
        this.getAllSubProducts(this.categories, response);
        console.log(this.subProducts);
        this.products = this.subProducts.slice();
        this.subProducts.length = 0;
        console.log(this.products);
        console.log(this.subProducts);
      });
      this.isCategorySelected = true;
    }
  }

  // onPageChanged(page: number){
  //   let productName = this.getProductsForm.controls['productNameFilter'].value;
  //   let categoryName = this.getProductsForm.controls['categoryFilter'].value;
  //   let categoryId = categoryName === 'All' ? null : this.categories.filter(c => c.name === categoryName)[0].id;
  //   this.productsService.getFilteredProducts(productName, categoryId, page, 20, null, null).subscribe(response =>{
  //     this.products = response;
  //     this.filtered = false;
  //   });
  //   this.page = page;
  // }

  private categoryDFS(categories: Category[], target: string){
    if(categories.length > 0){
      for(let category of categories){
        if(category.name === target){
          return category;
        }
        else {
          let found = this.categoryDFS(category.subcategories, target);
          if(found != null){
            return found
          }
        }
      }
    }
    return null;
  }

  private getAllSubProducts(categories: Category[], products: Product[]){
    if(categories.length > 0 && products.length > 0){
      for(let category of categories){
        let targetProducts = products.filter(obj => obj.categoryId === category.id);
        if(targetProducts.length > 0){
          for(let product of targetProducts){
            this.subProducts.push(product);
            products.filter(obj => obj !== product);
          }
        }
        this.getAllSubProducts(category.subcategories, products);
      }
    }
    return null;
  }

  private breadcrumbSearch(target: string){
    for(let i = this.breadcrumbs.length - 1; i >=0; i--){
      if(this.breadcrumbs[i] != target){
        if(this.breadcrumbs[i] != 'All'){
          this.breadcrumbs.pop();
        }
        else{
          return this.breadcrumbs[i];
        }
      }
      else{
        return this.breadcrumbs[i];
      }
    }
  }

  onAddProductToOrder(id: string){
    let selectedProduct: Product;
    this.productsService.getProduct(id).subscribe(response => {
      selectedProduct = response;
      console.log(selectedProduct);
      let selectedOrderProduct = new OrderProduct(
        selectedProduct.name,
        1,
        selectedProduct.price,
        selectedProduct.price,
      );
      console.log(selectedOrderProduct);
      this.order.products.push(selectedOrderProduct);
      this.order.price = +(this.order.price + selectedProduct.price).toFixed(2);
    });
  }

  onRemoveProductFromOrder(name: string){
    let targetProduct = this.order.products.filter(p => p.name === name)[0];
    this.order.price = +(this.order.price - targetProduct.price * targetProduct.quantity).toFixed(2);
    this.order.products.forEach((value,index)=>{
      if(value === targetProduct){
        this.order.products.splice(index,1);
      }
  });
    
  }

  onOrderSave(){
    let waiterId: string;
    let tableId: number;
    if(this.isAdmin === true){
      waiterId = this.editOrderForm.controls['user'].value;
      tableId = this.editOrderForm.controls['table'].value;
    }
    else {
      this.authService.getCurrentUser().subscribe(response => {
        waiterId = response.id;
      });
      tableId = this.order.tableId;
    }
    
    let products: {productId: string, quantity: number}[] = [];
    
    for(let prod of this.order.products){
      console.log(prod);
      console.log(this.products);
      let productId = this.allProducts.filter(p => p.name === prod.name)[0].productId;
      let quantity = +prod.quantity;
      products.push({
        productId,
        quantity
      })
    }
    console.log(products);
    this.ordersService.updateOrder(this.id, waiterId, tableId, products).subscribe(response => {
      if(response.message === '_Order successfully updated_'){
        this.router.navigate(['/orders']);
      }
    });
  }

  onCompleteOrder(){
    this.ordersService.completeOrder(this.id).subscribe(response => {
      if(response.message === "_Order successfully completed_"){
        this.router.navigate(['/orders']);
      }
    });
  }
}
