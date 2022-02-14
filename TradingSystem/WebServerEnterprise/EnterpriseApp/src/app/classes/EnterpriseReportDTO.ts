import {ProductSupplierStockItemDTO} from "./ProductSupplierStockItemDTO";

export class EnterpriseReportDTO{
  storeReports!: StoreReportDTO[];
  enterpriseId!: number;
  enterpriseName!: string;
}

export class StoreReportDTO {
  storeId!: number;
  storeName!: string;
  location!: string;
  stockItems!: ProductSupplierStockItemDTO[];
}
