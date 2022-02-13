import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { ProductStockItemDTO } from '../../classes/ProductStockItemDTO';
import { StoreService } from '../../services/store/store.service';
import { LowStockItemDTODataSource } from './low-stock-table-datasource';
import { OrderAmountDialogComponent } from '../order-amount-dialog/order-amount-dialog.component';
import {OrderProductDTO} from '../../classes/OrderProductDTO'



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

  onOrderProductClick(){
    const productsToOrder: Array<OrderProductDTO> = []


    this.dataSource.data.forEach( (row) => {
      if(!row.orderAmount || row.orderAmount < 1){
        return;
      }

      productsToOrder.push(row)
    });

    this.storeService.orderProducts(productsToOrder).subscribe(data => {
      console.log(data);
    }, error => {
      console.log(error);
    });

  }

  openOrderProductDialog(row: OrderProductDTO){
    this.dialog.open(OrderAmountDialogComponent, { data: row }).afterClosed().subscribe(orderamount => {


      if(orderamount){
        const result = this.dataSource.data.findIndex( item => item.productId === row.productId);

        if(result < 0) return;

        this.dataSource.data[result].orderAmount = orderamount.data;
      }

    });
  }
}
