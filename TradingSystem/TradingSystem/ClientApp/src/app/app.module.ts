import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import {getBaseUrl} from "../main";
import { NavMenuComponent } from './component/nav-menu/nav-menu.component';
import {HomeComponent} from "./views/home/home.component";
import {CounterComponent} from "./component/counter/counter.component";
import {StockItemComponent} from "./component/stockitem/stockitem.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    StockItemComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'stockitem', component: StockItemComponent },
    ])
  ],
  providers: [
    { provide: "BASE_API_URL", useFactory: getBaseUrl, deps: [] }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
