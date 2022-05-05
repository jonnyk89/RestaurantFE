import { Component, Input, OnInit } from '@angular/core';
import { Category } from '../recursive/recursive.component';

@Component({
  selector: 'app-categories-recursive-dropdown',
  templateUrl: './categories-recursive-dropdown.component.html',
  styleUrls: ['./categories-recursive-dropdown.component.css']
})
export class CategoriesRecursiveDropdownComponent implements OnInit {
  @Input() categories: Category;
  constructor() { }

  ngOnInit(): void {
    console.log(this.categories);
  }

}
