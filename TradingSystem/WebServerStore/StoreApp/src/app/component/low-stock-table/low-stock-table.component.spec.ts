import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LowStockTableComponent } from './low-stock-table.component';

describe('LowStockTableComponent', () => {
  let component: LowStockTableComponent;
  let fixture: ComponentFixture<LowStockTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LowStockTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LowStockTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
