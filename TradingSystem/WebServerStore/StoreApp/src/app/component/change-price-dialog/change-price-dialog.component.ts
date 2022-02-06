import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators  } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ProductSupplierStockItemDTO } from '../../classes/ProductSupplierStockItemDTO';
import { StoreService } from '../../services/store/store.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-change-price-dialog',
  templateUrl: './change-price-dialog.component.html',
  styleUrls: ['./change-price-dialog.component.css']
})
export class ChangePriceDialogComponent implements OnInit {
  //@ts-ignore
  changePriceForm: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ProductSupplierStockItemDTO,
    private dialogRef: MatDialogRef<ChangePriceDialogComponent>,
    private storeService: StoreService,
    private snackBar: MatSnackBar) {
    console.log(data)
  }

  ngOnInit(): void {
    //Todo: Nur zahlen erlaubt in Change Price Field
    this.changePriceForm = new FormGroup({ salesPrice: new FormControl(this.data.stockItem.salesPrice, [Validators.required]) })
  }

  onSubmit(): void {
    if (!this.changePriceForm.valid) {
      return;
    }
    this.snackBar.open("Changed price succesfully!", "✖", {panelClass: ['success']})
    /*this.storeService.changePrice(this.data.stockItem.itemId, this.changePriceForm.value.salesPrice).subscribe(
      () => {
        this.snackbar.open("Changed price succesfully!","✖",{panelClass: ['success']})
      });
    },
      error => {
        this.snackbar.open("Could not change price!","✖",{panelClass: ['failure']})
      }
    );*/

    this.dialogRef.close()
  }

}
