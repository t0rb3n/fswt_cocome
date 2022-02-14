import { ProductSupplierStockItemDTO } from "./ProductSupplierStockItemDTO";

export class OrderProductDTO extends ProductSupplierStockItemDTO{
    orderAmount?: number;

    constructor(dto: ProductSupplierStockItemDTO, orderAmount: number){
        super(dto.productId, dto.barcode, dto.purchasePrice, dto.productName, dto.supplierId, dto.supplierName, dto.stockItem);
        this.orderAmount = orderAmount
    }
}