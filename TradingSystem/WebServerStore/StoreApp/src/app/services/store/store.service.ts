import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProductSupplierStockItemDTO} from "../../classes/ProductSupplierStockItemDTO";
import {Observable} from "rxjs";
import { StockItemDTO } from '../../classes/StockItemDTO';

@Injectable({
  providedIn: 'root'
})
export class StoreService {

  constructor(
    private http: HttpClient,
  ) {
  }

  getStockItems(): Observable<ProductSupplierStockItemDTO[]> {
    return this.http.get<ProductSupplierStockItemDTO[]>('storestockitem');
  }

  changeStockItemPrice(stockItemId: number, newPrice: number): Observable<any> {
    return this.http.patch(`storestockitem/${stockItemId}`,
      [{
      "op": "replace",
      "path": "/salesPrice",
      "value": newPrice
      }]);
  }
}

class StockItemPrice {
  constructor(public price: number) {
  }
}
