import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ProductionPlansComponent } from './production-plans';

describe('ProductionPlansComponent', () => {
  let component: ProductionPlansComponent;
  let fixture: ComponentFixture<ProductionPlansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductionPlansComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductionPlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not expose createdBy in the request form', () => {
    expect(component.config.formFields.some(field => field.key === 'createdBy')).toBe(false);
  });
});
