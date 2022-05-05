import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TableGeneral } from './tableGeneral.model';
import { TablesService } from './tables.service';

@Component({
  selector: 'app-tables',
  templateUrl: './tables.component.html',
  styleUrls: ['./tables.component.css']
})
export class TablesComponent implements OnInit {
  tables: TableGeneral[];
  isLoading = false;
  constructor(private tablesService: TablesService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.tablesService.getAllTables().subscribe(response => {
      this.tables = response;
      console.log(this.tables);
      this.isLoading = false;
    });
  }

  onClickTable(id: string){
    let targetTable = this.tables.filter(t => t.id === +id)[0];
    if(targetTable.status === "Active"){
      this.router.navigate(['edit', id], {relativeTo: this.route});
    }
    else{
      this.router.navigate(['free', id], {relativeTo: this.route})
    }
  }

}
