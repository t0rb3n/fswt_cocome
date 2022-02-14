import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {EnterpriseReportDTO} from "../classes/EnterpriseReportDTO";
import {MeanTimeReportDTO} from "../classes/MeanTimeReportDTO";

@Injectable({
  providedIn: 'root'
})
export class EnterpriseService {

  constructor(private http: HttpClient) { }

  getEnterpriseReport(): Observable<EnterpriseReportDTO> {
    return this.http.get<EnterpriseReportDTO>(`report`);
  }
  getStatistics(): Observable<MeanTimeReportDTO[]> {
    return this.http.get<MeanTimeReportDTO[]>('statistics');
  }
}
