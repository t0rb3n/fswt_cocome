import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-stockitem',
  templateUrl: './stockitem.component.html',
})
export class StockItemComponent {
  public stockitems: ProductSupplierStockItemDTO[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string)  {
    http.get<ProductSupplierStockItemDTO[]>(baseUrl + 'storestockitem').subscribe(result => {
      this.stockitems = result;
    }, error => console.error(error));

  }

}

interface ProductSupplierStockItemDTO {
  productId: number;
  barcode: number;
  purchasePrice: number;
  productName: string;
  supplierId: number;
  supplierName: string;
  stockItem: StockItemDTO;
}

interface StockItemDTO {
  itemId: number;
  salesPrice: number;
  amount: number;
  minStock: number;
  maxStock: number;
}


