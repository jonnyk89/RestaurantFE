import { DatePipe } from '@angular/common';
import { Component, OnInit, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { CategoriesService } from 'src/app/categories/categories.service';
import { Category } from 'src/app/categories/category.model';
import { Order } from 'src/app/orders/order.model';
import { OrderProduct } from 'src/app/orders/orderProduct.model';
import { OrdersService } from 'src/app/orders/orders.service';
import { Product } from 'src/app/products/product.model';
import { ProductsService } from 'src/app/products/products.service';
import { TableGeneral } from 'src/app/tables/tableGeneral.model';
import { TablesService } from 'src/app/tables/tables.service';

@Component({
  selector: 'app-table-free',
  templateUrl: './table-free.component.html',
  styleUrls: ['./table-free.component.css'],
  providers: [DatePipe]
})
export class TableFreeComponent implements OnInit {
  tableId: number;
  orderId: string;
  orderNumber: number;
  isAdmin = false;
  editTableForm: FormGroup;
  tables: TableGeneral[];
  table: TableGeneral;
  categories: Category[];
  filteredCategories: Category[];
  products: Product[];
  allProducts: Product[];
  subProducts: Product[] = [];
  order: Order;
  tableCapacity: number;
  breadcrumbs = ['All'];
  selectedCategory: Category;
  isCategorySelected = false;
  isInEditMode = false;
  currentPage = 1;
  waiter = "";
  


  constructor(private authService: AuthService,
              private categoriesService: CategoriesService,
              private productsService: ProductsService,
              private ordersService: OrdersService,
              private tablesService: TablesService,
              private route: ActivatedRoute,
              private datePipe: DatePipe,
              private router: Router) { }

  ngOnInit(): void {
    const userData: {
      email: string;
      name: string;
      role: string;
      _token: string;
      _tokenExpirationDate: string;
    } = JSON.parse(localStorage.getItem('userData'));
    this.waiter = userData.name;
    this.isAdmin = userData.role === "Admin" ? true : false;
    this.isInEditMode = false;
    this.tableId = this.route.snapshot.params['tableId'];
    this.initForm();
  }

  private initForm(){

    this.editTableForm = new FormGroup({
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

    if(this.isInEditMode){
      this.tablesService.getTable(this.tableId).subscribe(response => {
        this.table = response;
        this.orderId = response.orders.filter(o => o.status === "Active")[0].id;
        this.ordersService.getOrder(this.orderId).subscribe(response => {
          this.order = response;
          console.log(this.order);
          this.tablesService.getAllTables().subscribe(response => {
            this.tables = response.filter(t => t.status === "Free" || t.id === this.order.tableId);
            
            console.log(this.tables);
          });
          this.tableCapacity = this.tables.filter(t => t.id === this.order.tableId)[0].capacity;
        });
      });
    }
    else{
      this.tablesService.getTable(this.tableId).subscribe(response => {
        this.table = response;
      });
    }

    

    console.log(this.breadcrumbs);
  }

  createNewOrder(){
    this.isInEditMode = true;
    let date = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
    let products: OrderProduct[] = [];

    this.order = new Order("id", this.tableId, this.waiter, date, "Active", 0, products);
  }

  quantityChanged(newValue, productName: string){
    this.order.products.filter(p => p.name === productName)[0].quantity = newValue;
    let newPrice = 0;
    for(let prod of this.order.products){
      newPrice = +(newPrice + (prod.price * prod.quantity)).toFixed(2);
    }
    this.order.price = newPrice;
  }

  onCategorySelect(id: string, name: string){
    this.breadcrumbs.push(name);
    this.selectedCategory = this.categoryDFS(this.filteredCategories, name);
    this.categories = this.selectedCategory.subcategories.length > 0 ? this.selectedCategory.subcategories : [this.selectedCategory];
    this.productsService.getAllProducts().subscribe(response => {
        this.getAllSubProducts(this.categories, response);
        this.products = this.subProducts.slice();
        this.subProducts.length = 0;
        console.log(this.subProducts);
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
    console.log(categories);
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
      console.log(this.subProducts);
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
    this.authService.getCurrentUser().subscribe(response => {
      waiterId = response.id;
    });
    tableId = this.order.tableId;
    
    let products: {productId: string, quantity: number}[] = [];

    for(let prod of this.order.products){
      console.log(prod);
      let productId = this.allProducts.filter(p => p.name === prod.name)[0].productId;
      let quantity = +prod.quantity;
      products.push({
        productId,
        quantity
      })
    }
    console.log(products);
    this.ordersService.createOrder(tableId, products).subscribe(response => {
      if(response.message === '_Order successfully created_'){
        this.router.navigate(['/tables']);
      }
    });
  }

  onCompleteOrder(){
    this.ordersService.completeOrder(this.orderId).subscribe(response => {
      if(response.message === "_Order successfully completed_"){
        this.router.navigate(['/tables']);
      }
    });
  }
}
