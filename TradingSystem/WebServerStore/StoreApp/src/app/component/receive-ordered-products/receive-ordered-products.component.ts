import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTable } from '@angular/material/table';
import { Observable } from 'rxjs';
import { ProductOrderDTO } from 'src/app/classes/ProductOrderDTO';
import { StoreService } from 'src/app/services/store/store.service';

@Component({
  selector: 'app-receive-ordered-products',
  templateUrl: './receive-ordered-products.component.html',
  styleUrls: ['./receive-ordered-products.component.css']
})
export class ReceiveOrderedProductsComponent {
  @ViewChild(MatTable) table!: MatTable<ProductOrderDTO>;
  storeOrders: ProductOrderDTO[] = [];
  $storeOrders: Observable<ProductOrderDTO[]>;
  
  displayedColumns = ['ProductName', 'Supplier', 'PurchasePrice', 'OrderedAmount', 'Barcode'];

  constructor(private storeService: StoreService, private snackBar: MatSnackBar) { 
    this.$storeOrders = this.storeService.getAllOpenOrders();
    this.$storeOrders.subscribe( data => {
      this.storeOrders = data;
    })
  }

  onReceiveOrderClick(productOrderId: number) {
    this.storeService.acceptOrder(productOrderId).subscribe(
      () => {
        this.snackBar.open("Received order succesfully!", "✖", { panelClass: ['success'] });
        this.storeOrders = this.storeOrders.filter(order => order.productOrderId !== productOrderId);
      },
      error => {
        this.snackBar.open("Something went wrong!","✖",{panelClass: ['failure']})
      }
    );
  }

}
