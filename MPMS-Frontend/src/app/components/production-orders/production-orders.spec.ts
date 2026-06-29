import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ProductionOrdersComponent } from './production-orders';

describe('ProductionOrdersComponent', () => {
  let component: ProductionOrdersComponent;
  let fixture: ComponentFixture<ProductionOrdersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductionOrdersComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductionOrdersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should use orderId as the route id key', () => {
    expect(component.config.idKey).toBe('orderId');
  });
});
