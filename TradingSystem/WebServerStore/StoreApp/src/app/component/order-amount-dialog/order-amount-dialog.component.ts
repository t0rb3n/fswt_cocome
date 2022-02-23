import { Component, Inject, OnInit } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import {
  FormControl,
  Validators,
  FormGroupDirective,
  NgForm,
} from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";
import { ProductSupplierStockItemDTO } from "../../classes/ProductSupplierStockItemDTO";

@Component({
  templateUrl: "./order-amount-dialog.component.html",
  styleUrls: ["./order-amount-dialog.component.css"],
})
export class OrderAmountDialogComponent implements OnInit {

  amountForm = new FormControl('', [Validators.required, Validators.min(1)]);
  matcher = new OrderAmountErrorMatcher();
  max: number = 0;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ProductSupplierStockItemDTO,
    private dialogRef: MatDialogRef<OrderAmountDialogComponent>
  ) {}

  ngOnInit(): void {
    this.max = this.data.stockItem.maxStock-this.data.stockItem.amount
    this.amountForm.addValidators(Validators.max(this.max))
  }

  onSubmit() {
    if (!this.amountForm.valid) return;
    
    this.dialogRef.close({ data: this.amountForm.value });
  }
}

export class OrderAmountErrorMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return !!(
      control &&
      control.invalid &&
      (control.dirty || control.touched || isSubmitted)
    );
  }
}
