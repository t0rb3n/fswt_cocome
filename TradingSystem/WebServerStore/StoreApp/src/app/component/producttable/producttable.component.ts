import {AfterViewInit, Component, Inject, ViewChild} from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTable} from '@angular/material/table';
//import { ProductSupplierStockItemDTODataSource } from './producttable-datasource';
import {ProductSupplierStockItemDTO} from "../../classes/ProductSupplierStockItemDTO";
import {StoreService} from "../../services/store/store.service";
import {Observable, of as observableOf, merge} from 'rxjs';
import {catchError, map, startWith, switchMap} from "rxjs/operators";
import {ProductSupplierStockItemDTODataSource} from "./producttable-datasource";

@Component({
  selector: 'app-producttable',
  templateUrl: './producttable.component.html',
  styleUrls: ['./producttable.component.css']
})
export class ProducttableComponent implements AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<ProductSupplierStockItemDTO>;
  dataSource: ProductSupplierStockItemDTODataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['ProductName', 'Supplier', 'MinStock', 'MaxStock', 'Amount', 'PurchasePrice', 'SalePrice',];

  constructor(private storeService: StoreService) {
    this.dataSource = new ProductSupplierStockItemDTODataSource(storeService);
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;
  }
}
