import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTable} from '@angular/material/table';
import {ProductStockItemDTO} from '../../classes/ProductStockItemDTO';
import {ProductSupplierStockItemDTO} from '../../classes/ProductSupplierStockItemDTO';
import {StoreService} from '../../services/store/store.service';
import {LowStockItemDTODataSource} from './low-stock-table-datasource';
import {OrderAmountDialogComponent} from '../order-amount-dialog/order-amount-dialog.component';
import {OrderRequest, OrderProductDTO, OrderRequestDTO} from '../../classes/OrderProductDTO'
import { ProductSupplierDTO } from 'src/app/classes/ProductSupplierDTO';

@Component({
  selector: 'app-low-stock-table',
  templateUrl: './low-stock-table.component.html',
  styleUrls: ['./low-stock-table.component.css']
})
export class LowStockTableComponent implements AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<OrderProductDTO>;
  dataSource: LowStockItemDTODataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['ProductName', 'Supplier', 'PurchasePrice', 'MinStock', 'MaxStock', 'Amount', 'OrderedAmount',];

  constructor(private storeService: StoreService, public dialog: MatDialog) {
    this.dataSource = new LowStockItemDTODataSource(storeService);
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.table.dataSource = this.dataSource;
  }


  onOrderProductClick() {

    const productsToOrder = this.dataSource.data
    .filter((row) => row.orderAmount && row.orderAmount >= 1)
    .map((row) => {
      return new OrderRequestDTO(
        new ProductSupplierDTO (
          row.productId,
          row.barcode,
          row.purchasePrice,
          row.productName,
          row.supplierId,
          row.supplierName
        ),
        row.orderAmount!!
      );
    });
  
    let request = new OrderRequest(productsToOrder);

    this.storeService.orderProducts(request).subscribe(data => {
      console.log(data);
    }, error => {
      console.log(error);
    });

  }

  openOrderProductDialog(row: OrderProductDTO) {
    this.dialog.open(OrderAmountDialogComponent, {data: row}).afterClosed().subscribe(orderAmount => {

      if (orderAmount) {
        const result = this.dataSource.data.findIndex(item => item.productId === row.productId);

        if (result < 0) return;

        this.dataSource.data[result].orderAmount = orderAmount.data;
      }

    });
  }
}
