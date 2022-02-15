import {DataSource} from '@angular/cdk/collections';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {catchError, map, startWith, switchMap} from 'rxjs/operators';
import {Observable, of as observableOf, merge} from 'rxjs';
import {StoreService} from "../../services/store/store.service";
import {Injectable} from "@angular/core";
import {OrderProductDTO} from 'src/app/classes/OrderProductDTO';


/**
 * Data source for the lowStockTable view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
@Injectable()
export class LowStockItemDTODataSource extends DataSource<OrderProductDTO> {
  data: OrderProductDTO[] = [];
  paginator: MatPaginator | undefined;
  sort: MatSort | undefined;
  length: number = 0;

  constructor(private storeService: StoreService) {
    super();
  }

  /**
   * Connect this data source to the table. The table will only update when
   * the returned stream emits new items.
   * @returns A stream of the items to be rendered.
   */
  connect(): Observable<OrderProductDTO[]> {
    if (this.paginator && this.sort) {
      // Combine everything that affects the rendered data into one update
      // stream for the data-table to consume.
      return merge(this.paginator.page, this.sort.sortChange)
        .pipe(
          startWith({}),
          switchMap(() => {
            return this.storeService.getLowStockItems(
            ).pipe(catchError(() => observableOf(null)));
          }),
          map(data => {
              // hacky way to do it but works
              this.length = data!!.length;
              if (data === null) {
                return [];
              }
              this.data = this.getPagedData(this.getSortedData([...data]));

              return this.data;
            }
          ),
        )
    } else {
      throw Error('Please set the paginator and sort on the data source before connecting.');
    }
  }

  /**
   *  Called when the table is being destroyed. Use this function, to clean up
   * any open connections or free any held resources that were set up during connect.
   */
  disconnect(): void {
  }

  /**
   * Paginate the data (client-side). If you're using server-side pagination,
   * this would be replaced by requesting the appropriate data from the server.
   */

  private getPagedData(data: OrderProductDTO[]): OrderProductDTO[] {
    if (this.paginator) {
      const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
      return data.splice(startIndex, this.paginator.pageSize);
    } else {
      return data;
    }
  }

  /**
   * Sort the data (client-side). If you're using server-side sorting,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getSortedData(data: OrderProductDTO[]): OrderProductDTO[] {
    if (!this.sort || !this.sort.active || this.sort.direction === '') {
      return data;
    }

    return data.sort((a, b) => {
      const isAsc = this.sort?.direction === 'asc';
      switch (this.sort?.active) {
        case 'ProductName':
          return compare(a.productName, b.productName, isAsc);
        case 'Supplier':
          return compare(a.supplierName, b.supplierName, isAsc);
        case 'PurchasePrice':
          return compare(+a.purchasePrice, +b.purchasePrice, isAsc);
        case 'MinStock':
          return compare(+a.stockItem.minStock, +b.stockItem.minStock, isAsc);
        case 'MaxStock':
          return compare(+a.stockItem.maxStock, +b.stockItem.maxStock, isAsc);
        case 'Amount':
          return compare(+a.stockItem.amount, +b.stockItem.amount, isAsc);
        default:
          return 0;
      }
    });
  }
}

/** Simple sort comparator for example ID/Name columns (for client-side sorting). */
function compare(a: string | number, b: string | number, isAsc: boolean): number {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
