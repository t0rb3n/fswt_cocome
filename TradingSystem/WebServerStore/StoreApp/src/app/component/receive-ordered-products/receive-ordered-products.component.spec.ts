import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiveOrderedProductsComponent } from './receive-ordered-products.component';

describe('ReceiveOrderedProductsComponent', () => {
  let component: ReceiveOrderedProductsComponent;
  let fixture: ComponentFixture<ReceiveOrderedProductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceiveOrderedProductsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceiveOrderedProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
