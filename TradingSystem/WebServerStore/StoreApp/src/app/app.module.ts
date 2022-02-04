import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { getBaseUrl } from "../main";
import { AppComponent } from './app.component';
import { HeaderComponent } from "./component/header/header.component";
import { NavMenuComponent } from './component/nav-menu/nav-menu.component';
import { HomeComponent } from './views/home/home.component';
import { CounterComponent } from './component/counter/counter.component';
import { StockItemComponent } from "./component/stockitem/stockitem.component";

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    StockItemComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, data: { title: 'Home' }, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent, data: { title: 'Counter' } },
      { path: 'stockitem', component: StockItemComponent, data: { title: 'StockItem' } },
    ])
  ],
  providers: [
    { provide: "BASE_API_URL", useFactory: getBaseUrl, deps: [] }
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule {
}
