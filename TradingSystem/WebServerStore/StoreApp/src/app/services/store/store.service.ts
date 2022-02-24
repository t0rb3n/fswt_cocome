import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProductSupplierStockItemDTO} from "../../classes/ProductSupplierStockItemDTO";
import {Observable} from "rxjs";
import { OrderRequest } from 'src/app/classes/OrderProductDTO';
import { ProductOrderDTO } from 'src/app/classes/ProductOrderDTO';


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

  //ToDo: change any to appropriate type
  orderProducts(orderProducts: OrderRequest): Observable<any> {
    return this.http.post('orderstockitem', orderProducts);
  }

  getAllOpenOrders(): Observable<ProductOrderDTO[]>{
    return this.http.get<ProductOrderDTO[]>('receiveorders');
  }

  acceptOrder(productOrderId: number): Observable<any> {
    console.log(productOrderId)
    return this.http.post('acceptorder', productOrderId);
  }
}

