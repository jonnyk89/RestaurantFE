import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TableGeneral } from '../tables/tableGeneral.model';
import { TablesService } from '../tables/tables.service';
import { Order } from './order.model';
import { OrdersService } from './orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: Order[];
  tables: TableGeneral[];
  getOrdersForm: FormGroup;
  orderFilter = false;
  tableFilter = false;
  waiterFilter = false;
  isLoading = false;
  page = 1;
  totalOrders = 20;

  orderSortClicks = 0;
  tableSortClicks = 0;
  waiterSortClicks = 0;
  dateSortClicks = 0;


  constructor(private ordersService: OrdersService, private tablesService: TablesService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.initForm();
  }

  private initForm(){
    this.page = 1;
    this.isLoading = true;
    this.ordersService.getFilteredOrders(null, 0, null, 0, 0, null, null).subscribe(response => {
      this.orders = response;
      console.log(this.orders);
      this.isLoading = false;
      this.totalOrders = this.orders.length > 20 ? 20 : this.orders.length;
    });

    this.tablesService.getAllTables().subscribe(response => {
      this.tables = response;
      console.log(this.tables);
    });

    this.getOrdersForm = new FormGroup({
      'waiterFilter': new FormControl(null),
      'tableFilter': new FormControl(null),
      'statusFilter': new FormControl(null),
    });
    this.getOrdersForm.controls['tableFilter'].setValue('all', {onlySelf: true})
    this.getOrdersForm.controls['statusFilter'].setValue('all', {onlySelf: true})
  }

  onClickOrderSort(){
    this.tableSortClicks = 0;
    this.waiterSortClicks = 0;
    this.dateSortClicks = 0;
    let sortBy = "order";
    let sortDirection = null;

    if(this.orderSortClicks === 2){
      this.orderSortClicks = 0;
      
    }
    else{
      this.orderSortClicks++;
      if(this.orderSortClicks === 0){
        sortBy = null
        sortDirection = null;
      }
      else if(this.orderSortClicks === 1){
        sortDirection = "Ascending";
      }
      else if(this.orderSortClicks === 2){
        sortDirection = "Descending";
      }
    }
  }

  onClickTableSort(){
    this.orderSortClicks = 0;
    this.waiterSortClicks = 0;
    this.dateSortClicks = 0;
    let sortBy = "table";
    let sortDirection = null;

    if(this.tableSortClicks === 2){
      this.tableSortClicks = 0;
      
    }
    else{
      this.tableSortClicks++;
      if(this.tableSortClicks === 0){
        sortBy = null
        sortDirection = null;
      }
      else if(this.tableSortClicks === 1){
        sortDirection = "Ascending";
      }
      else if(this.tableSortClicks === 2){
        sortDirection = "Descending";
      }
    }
  }

  onClickWaiterSort(){
    this.orderSortClicks = 0;
    this.tableSortClicks = 0;
    this.dateSortClicks = 0;
    let sortBy = "username";
    let sortDirection = null;

    if(this.waiterSortClicks === 2){
      this.waiterSortClicks = 0;
    }
    else{
      this.waiterSortClicks++;
      if(this.waiterSortClicks === 0){
        sortBy = null
        sortDirection = null;
      }
      else if(this.waiterSortClicks === 1){
        sortDirection = "Ascending";
      }
      else if(this.waiterSortClicks === 2){
        sortDirection = "Descending";
      }
    }
  }

  onClickDateSort(){
    this.orderSortClicks = 0;
    this.tableSortClicks = 0;
    this.waiterSortClicks = 0;
    let sortBy = "date";
    let sortDirection = null;

    if(this.dateSortClicks === 2){
      this.dateSortClicks = 0;
    }
    else{
      this.dateSortClicks++;
      if(this.dateSortClicks === 0){
        sortBy = null
        sortDirection = null;
      }
      else if(this.dateSortClicks === 1){
        sortDirection = "Ascending";
      }
      else if(this.dateSortClicks === 2){
        sortDirection = "Descending";
      }
    }
  }
  
  onPageChanged(page: number){
    this.page = page;
  }


  onViewOrder(orderId: string, orderNumber: number){
    this.router.navigate(['view', orderId, orderNumber], {relativeTo: this.route});
  }

  onEditOrder(orderId: string, orderNumber: number){
    this.router.navigate(['edit', orderId, orderNumber], {relativeTo: this.route});
  }

  onDeleteOrder(orderId: string){
    this.ordersService.deleteOrder(orderId).subscribe(response => {
      if(response.message === "_Order successfully deleted_"){
        let waiter = this.getOrdersForm.controls['waiterFilter'].value;
        let table = this.getOrdersForm.controls['tableFilter'].value;
        let status = this.getOrdersForm.controls['statusFilter'].value;
        this.onFilter(waiter, table, status);
      }
    });
  }

  onFilter(waiter: string, table: string, status: string){
    let tValue = table === 'all' ? null : Number(table);
    status = status === 'all' ? null : status;
    this.ordersService.getFilteredOrders(waiter, tValue, status, 0, 0, null, null).subscribe(response => {
      this.orders = response;
      this.totalOrders = this.orders.length > 20 ? 20 : this.orders.length;
    });
  }

}
