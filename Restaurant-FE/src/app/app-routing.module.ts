import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { JwtModule } from "@auth0/angular-jwt";
import { tokenGetter } from "./app.module";
import { AuthGuard } from "./auth/auth.guard";
import { AuthAdminGuard } from "./auth/authAdmin.guard";
import { AuthLoginGuard } from "./auth/authLogin.guard";
import { CategoriesComponent } from "./categories/categories.component";
import { CategoryEditComponent } from "./categories/category-edit/category-edit.component";
import { CategoryNewComponent } from "./categories/category-new/category-new.component";
import { LoginComponent } from "./login/login.component";
import { OrderEditComponent } from "./orders/order-edit/order-edit.component";
import { OrderViewComponent } from "./orders/order-view/order-view.component";
import { OrdersComponent } from "./orders/orders.component";
import { ProductEditComponent } from "./products/product-edit/product-edit.component";
import { ProductNewComponent } from "./products/product-new/product-new.component";
import { ProductsComponent } from "./products/products.component";
import { TableEditComponent } from "./tables/table-edit/table-edit.component";
import { TableFreeComponent } from "./tables/table-free/table-free.component";
import { TablesComponent } from "./tables/tables.component";
import { UserCurrentEditComponent } from "./users/user-current-edit/user-current-edit.component";
import { UserEditComponent } from "./users/user-edit/user-edit.component";
import { UserNewComponent } from "./users/user-new/user-new.component";
import { UsersComponent } from "./users/users.component";

const appRoutes: Routes = [
    { path: '', redirectTo: '/orders', pathMatch: 'full'},
    { path: 'orders', component: OrdersComponent, canActivate: [AuthGuard] },
    { path: 'orders/view/:orderId/:orderNumber', component: OrderViewComponent, canActivate: [AuthGuard] },
    { path: 'orders/edit/:orderId/:orderNumber', component: OrderEditComponent, canActivate: [AuthGuard] },
    { path: 'tables', component: TablesComponent, canActivate: [AuthGuard] },
    { path: 'tables/edit/:tableId', component: TableEditComponent, canActivate: [AuthGuard] },
    { path: 'tables/free/:tableId', component: TableFreeComponent, canActivate: [AuthGuard] },
    { path: 'products', component: ProductsComponent, canActivate: [AuthAdminGuard] },
    { path: 'products/new', component: ProductNewComponent, canActivate: [AuthAdminGuard] },
    { path: 'products/edit/:id', component: ProductEditComponent, canActivate: [AuthAdminGuard] },
    { path: 'categories', component: CategoriesComponent, canActivate: [AuthAdminGuard] },
    { path: 'categories/new', component: CategoryNewComponent, canActivate: [AuthAdminGuard] },
    { path: 'categories/edit/:id', component: CategoryEditComponent, canActivate: [AuthAdminGuard] },
    { path: 'users', component: UsersComponent, canActivate: [AuthAdminGuard] },
    { path: 'users/new', component: UserNewComponent, canActivate: [AuthAdminGuard]},
    { path: 'users/edit', component: UserCurrentEditComponent, canActivate: [AuthGuard]},
    { path: 'users/edit/:userId', component: UserEditComponent, canActivate: [AuthAdminGuard]},
    { path: 'login', component: LoginComponent },
    { path: '**', component: OrdersComponent, canActivate: [AuthGuard] },
]

@NgModule({
    imports: [RouterModule.forRoot(appRoutes), JwtModule.forRoot({
        config: {
            tokenGetter: tokenGetter,
            allowedDomains: ['localhost:7162'],
            disallowedRoutes: [],
        }
    })],
    exports: [RouterModule],
})
export class AppRoutingModule{

}