import { ProductSupplierStockItemDTO } from "./ProductSupplierStockItemDTO";
import {ProductSupplierDTO}  from "./ProductSupplierDTO"

export class OrderProductDTO extends ProductSupplierStockItemDTO{
    orderAmount?: number;

    constructor(dto: ProductSupplierStockItemDTO, orderAmount: number){
        super(dto.productId, dto.barcode, dto.purchasePrice, dto.productName, dto.supplierId, dto.supplierName, dto.stockItem);
        this.orderAmount = orderAmount
    }
}


export class OrderRequest {
    orders: OrderRequestDTO[];

    constructor(orders : OrderRequestDTO[]){
        this.orders = orders;
    }
}
export class OrderRequestDTO{
    amount: number;
    productSupplier: ProductSupplierDTO

    constructor(dto: ProductSupplierDTO, amount: number){
        this.productSupplier = dto;
        this.amount = amount
    }
}
