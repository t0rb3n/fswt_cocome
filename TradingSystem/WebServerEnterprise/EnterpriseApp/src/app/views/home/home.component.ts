import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GlobalConfig, Title } from "../../app.config";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(public config: GlobalConfig, private route: ActivatedRoute) {
    Title.set(this.route.snapshot.data.title);
  }
}
