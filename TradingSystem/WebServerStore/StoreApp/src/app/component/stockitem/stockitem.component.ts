import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {ProductSupplierStockItemDTO} from "../../classes/ProductSupplierStockItemDTO";
import {StoreService} from "../../services/store/store.service";

@Component({
  selector: 'app-stockitem',
  templateUrl: './stockitem.component.html',
})
export class StockItemComponent implements OnInit {
  public stockitems: ProductSupplierStockItemDTO[] = [];

  constructor(private storeService: StoreService)  {}

  ngOnInit(){
    this.storeService.getStockItems().subscribe(
      fetchedStockItems =>
      {this.stockitems = fetchedStockItems }, error => console.error(error));
  }
}
