import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { TablesComponent } from './tables/tables.component';
import { OrdersComponent } from './orders/orders.component';
import { ProductsComponent } from './products/products.component';
import { CategoriesComponent } from './categories/categories.component';
import { UsersComponent } from './users/users.component';
import { ProfileComponent } from './users/profile/profile.component';
import { AppRoutingModule } from './app-routing.module';
import { DropdownDirective } from './shared/dropdown.directive';
import { LoginComponent } from './login/login.component';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { UserNewComponent } from './users/user-new/user-new.component';
import { UserCurrentEditComponent } from './users/user-current-edit/user-current-edit.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { RecursiveComponent } from './shared/recursive/recursive.component';
import { CategoryNewComponent } from './categories/category-new/category-new.component';
import { CategoriesRecursiveDropdownComponent } from './shared/categories-recursive-dropdown/categories-recursive-dropdown.component';
import { CategoryEditComponent } from './categories/category-edit/category-edit.component';
import { ProductNewComponent } from './products/product-new/product-new.component';
import { ProductEditComponent } from './products/product-edit/product-edit.component';
import { OrderViewComponent } from './orders/order-view/order-view.component';
import { OrderEditComponent } from './orders/order-edit/order-edit.component';
import { TableEditComponent } from './tables/table-edit/table-edit.component';
import { TableFreeComponent } from './tables/table-free/table-free.component';
import { ToastrModule } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {NgbPaginationModule, NgbAlertModule} from '@ng-bootstrap/ng-bootstrap';

export function tokenGetter(){
  return localStorage.getItem('bearer');
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    TablesComponent,
    OrdersComponent,
    ProductsComponent,
    CategoriesComponent,
    UsersComponent,
    ProfileComponent,
    DropdownDirective,
    LoginComponent,
    LoadingSpinnerComponent,
    UserNewComponent,
    UserCurrentEditComponent,
    UserEditComponent,
    RecursiveComponent,
    CategoryNewComponent,
    CategoriesRecursiveDropdownComponent,
    CategoryEditComponent,
    ProductNewComponent,
    ProductEditComponent,
    OrderViewComponent,
    OrderEditComponent,
    TableEditComponent,
    TableFreeComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    ToastrModule.forRoot({
      
    }),
    CommonModule,
    BrowserAnimationsModule,
    NgbPaginationModule,
    NgbAlertModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
