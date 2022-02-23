
export class ProductDTO {
    productId!: number;
    barcode!: number;
    purchasePrice!: number;
    productName!: string;
}

export class ProductSupplierDTO extends ProductDTO{

    supplierId: number;
    supplierName: string;

    constructor(productId: number, barcode: number, purchasePrice: number, productName: string, supplierId: number, supplierName: string){
        super();

        this.productId = productId;
        this.barcode = barcode; 
        this.purchasePrice = purchasePrice;
        this.productName = productName;
        this.supplierId = supplierId;
        this.supplierName = supplierName;
    }
}