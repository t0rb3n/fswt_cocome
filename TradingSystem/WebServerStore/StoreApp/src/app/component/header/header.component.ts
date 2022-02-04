import { Component } from '@angular/core';
import { GlobalConfig } from "../../app.config";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  constructor(public config: GlobalConfig) {}
}
