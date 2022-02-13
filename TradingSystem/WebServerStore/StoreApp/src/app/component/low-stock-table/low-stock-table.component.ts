import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { ProductStockItemDTO } from '../../classes/ProductStockItemDTO';
import { ProductSupplierStockItemDTO } from '../../classes/ProductSupplierStockItemDTO';
import { StoreService } from '../../services/store/store.service';
import { LowStockItemDTODataSource } from './low-stock-table-datasource';

@Component({
  selector: 'app-low-stock-table',
  templateUrl: './low-stock-table.component.html',
  styleUrls: ['./low-stock-table.component.css']
})
export class LowStockTableComponent implements AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<ProductStockItemDTO>;
  dataSource: LowStockItemDTODataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['ProductName', 'Supplier', 'PurchasePrice', 'MinStock', 'MaxStock', 'Amount', 'OrderAmount',];

  constructor(private storeService: StoreService, public dialog: MatDialog) {
    this.dataSource = new LowStockItemDTODataSource(storeService);
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;
  }

}
