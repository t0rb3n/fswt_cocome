import {Component} from '@angular/core';
import {EnterpriseService} from "../../services/enterprise.service";
import {Observable} from "rxjs";
import {EnterpriseReportDTO, StoreReportDTO} from "../../classes/EnterpriseReportDTO";
import {MatOption} from "@angular/material/core";
import {MeanTimeReportDTO} from "../../classes/MeanTimeReportDTO";

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent {
  displayedColumns: string[] = ['productName', 'supplierName', 'purchasePrice', 'minStock', 'maxStock', 'amount'];
  statsColumns: string[] = ['supplierName', 'meanTime'];

  enterprise: Observable<EnterpriseReportDTO>;
  stores!: StoreReportDTO[];
  selectedStore!: number;
  store!: StoreReportDTO;
  stats!: MeanTimeReportDTO[];

  constructor(private enterpriseService: EnterpriseService) {
    this.enterprise = this.enterpriseService.getEnterpriseReport();
    this.enterprise.subscribe(data => {
      this.stores = data.storeReports;
    });
    this.enterpriseService.getStatistics().subscribe(data => this.stats = data);
  }

  changeStore(store: StoreReportDTO) {
    this.store = store;
  }


}

