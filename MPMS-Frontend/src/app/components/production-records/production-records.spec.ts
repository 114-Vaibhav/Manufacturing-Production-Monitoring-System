import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ProductionRecordsComponent } from './production-records';

describe('ProductionRecordsComponent', () => {
  let component: ProductionRecordsComponent;
  let fixture: ComponentFixture<ProductionRecordsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductionRecordsComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductionRecordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should keep productionDate server-managed', () => {
    expect(component.config.formFields.some(field => field.key === 'productionDate')).toBe(false);
  });
});
