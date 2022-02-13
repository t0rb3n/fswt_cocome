import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProductSupplierStockItemDTO} from "../../classes/ProductSupplierStockItemDTO";
import {Observable} from "rxjs";
import { StockItemDTO } from '../../classes/StockItemDTO';
import { ProductStockItemDTO } from '../../classes/ProductStockItemDTO';

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

  getLowStockItems(): Observable<ProductSupplierStockItemDTO[]> {
    return this.http.get<ProductSupplierStockItemDTO[]>('lowstorestockitem');
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

