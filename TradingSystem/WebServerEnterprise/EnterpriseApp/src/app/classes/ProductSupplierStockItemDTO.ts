import {StockItemDTO} from "./StockItemDTO";

export class ProductSupplierStockItemDTO {
  productId!: number;
  barcode!: number;
  purchasePrice!: number;
  productName!: string;
  supplierId!: number;
  supplierName!: string;
  stockItem!: StockItemDTO;
}

