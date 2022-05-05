import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

export class Category {
  id: string;
  name: string;
  subCategories: Category[];
}

@Component({
  selector: 'app-recursive',
  templateUrl: './recursive.component.html',
  styleUrls: ['./recursive.component.css']
})
export class RecursiveComponent implements OnInit {
  @Input() categories: Category;

  constructor(private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    //debugger
    console.log(this.categories);
  }

  onEditCategory(id: string){
    this.router.navigate(['edit', id], {relativeTo: this.route});
  }

}
