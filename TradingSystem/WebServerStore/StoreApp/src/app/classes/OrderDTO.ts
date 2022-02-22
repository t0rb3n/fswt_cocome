import { ProductSupplierDTO } from "./ProductSupplierDTO";

export class OrderDTO {
    orderId: number;
    amount : number;
    productSupplier: ProductSupplierDTO;

    constructor(
        orderId: number,
        amount : number,
        productSupplier: ProductSupplierDTO) {
            this.orderId = orderId;
            this.amount = amount;
            this.productSupplier = productSupplier;
    }
}