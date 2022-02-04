import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HeaderComponent } from "./header/header.component";
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { StockItemComponent } from "./stockitem/stockitem.component";

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
  bootstrap: [
    AppComponent
  ]
})
export class AppModule {
}
