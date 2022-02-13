import { StockItemDTO } from "./StockItemDTO";

export class ProductStockItemDTO {
  productId!: number;
  barcode!: number;
  purchasePrice!: number;
  productName!: string;
  stockItem!: StockItemDTO;
}
