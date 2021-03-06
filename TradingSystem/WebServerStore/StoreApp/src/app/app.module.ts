import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { getBaseUrl } from "../main";
import { AppComponent } from './app.component';
import { HomeComponent } from './views/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatSidenavModule} from "@angular/material/sidenav";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatTabsModule, MatTabGroup, MatTabLabel} from '@angular/material/tabs';
import {MatFormFieldModule} from "@angular/material/form-field";
import { NavigationComponent } from './component/navigation/navigation.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { ProducttableComponent } from './component/producttable/producttable.component';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { ChangePriceDialogComponent } from './component/change-price-dialog/change-price-dialog.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { LowStockTableComponent } from './component/low-stock-table/low-stock-table.component';
import { OrderAmountDialogComponent } from './component/order-amount-dialog/order-amount-dialog.component';
import { ReceiveOrderedProductsComponent } from './component/receive-ordered-products/receive-ordered-products.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavigationComponent,
    ProducttableComponent,
    ChangePriceDialogComponent,
    LowStockTableComponent,
    OrderAmountDialogComponent,
    ReceiveOrderedProductsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, data: {title: 'Home'}, pathMatch: 'full'},
      { path: 'products', component: ProducttableComponent, data: { title: 'Products in stock' } },
      { path: 'low-stock-items', component: LowStockTableComponent, data: { title: 'Products running out of stock' } },
      { path: 'receive-ordered-products', component: ReceiveOrderedProductsComponent, data: { title: 'Receive ordered products' } },
    ]),
    BrowserAnimationsModule,
    MatToolbarModule,
    MatCheckboxModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    LayoutModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatSidenavModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatTabsModule,
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
