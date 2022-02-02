import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProductSupplierStockItemDTO} from "../../classes/ProductSupplierStockItemDTO";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StoreService {



  constructor(
    private http: HttpClient,
  ) {
  }

  getStockItems() : Observable<ProductSupplierStockItemDTO[]>{
    return this.http.get<ProductSupplierStockItemDTO[]>('storestockitem');
  }
}

