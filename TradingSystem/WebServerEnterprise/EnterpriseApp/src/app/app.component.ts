import {Component, Inject, OnInit} from '@angular/core';
import {GlobalConfig, EnterpriseConfig} from "./app.config";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})

export class AppComponent {
  constructor(public config:GlobalConfig, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

}
