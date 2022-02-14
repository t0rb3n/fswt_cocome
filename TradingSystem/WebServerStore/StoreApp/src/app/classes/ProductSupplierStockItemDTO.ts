import {StockItemDTO} from "./StockItemDTO";

export class ProductSupplierStockItemDTO {
  productId!: number;
  barcode!: number;
  purchasePrice!: number;
  productName!: string;
  supplierId!: number;
  supplierName!: string;
  stockItem!: StockItemDTO;

  constructor(
    productId: number, 
    barcode: number, 
    purchasePrice: number, 
    productName: string, 
    supplierId: number, 
    supplierName: string, 
    stockItem: StockItemDTO
  )
  {
    this.productId = productId;
    this.barcode = barcode;
    this.purchasePrice = purchasePrice;
    this.productName = productName;
    this.supplierId = supplierId;
    this.supplierName = supplierName;
    this.stockItem = stockItem;
  }

}

