import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HeaderComponent } from "./component/header/header.component";
import { NavMenuComponent } from './component/nav-menu/nav-menu.component';
import { HomeComponent } from './views/home/home.component';
import { CounterComponent } from './component/counter/counter.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, data: { title: 'Home' }, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent, data: { title: 'Counter' } },
    ])
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule {
}
