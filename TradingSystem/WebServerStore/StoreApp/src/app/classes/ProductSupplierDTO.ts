
export class ProductDTO {
    productId!: number;
    barcode!: number;
    purchasePrice!: number;
    productName!: string;
}

export class ProductSupplierDTO extends ProductDTO{

    supplierId!: number;
    supplierName!: string;
}