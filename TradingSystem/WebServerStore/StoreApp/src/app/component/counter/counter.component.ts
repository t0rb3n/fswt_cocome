import { Component } from '@angular/core';
import {Title} from "../../app.config";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public currentCount = 0;

  constructor(private route: ActivatedRoute) {
    Title.set(this.route.snapshot.data.title);
  }

  public incrementCounter() {
    this.currentCount++;
  }
}
