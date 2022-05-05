import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from '../order.model';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-view',
  templateUrl: './order-view.component.html',
  styleUrls: ['./order-view.component.css']
})
export class OrderViewComponent implements OnInit {
  @Input() orderNumber: number
  order: Order;
  id: string;

  constructor(private route: ActivatedRoute, private ordersService: OrdersService) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['orderId'];
    this.ordersService.getOrder(this.id).subscribe(response => {
      this.order = response;
    });
  }

}
