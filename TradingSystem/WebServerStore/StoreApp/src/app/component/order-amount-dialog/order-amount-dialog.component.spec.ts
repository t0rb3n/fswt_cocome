import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderAmountDialogComponent } from './order-amount-dialog.component';

describe('OrderAmountDialogComponent', () => {
  let component: OrderAmountDialogComponent;
  let fixture: ComponentFixture<OrderAmountDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderAmountDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderAmountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
