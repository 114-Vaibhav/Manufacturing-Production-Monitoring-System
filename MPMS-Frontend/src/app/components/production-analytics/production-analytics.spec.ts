import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ProductionAnalyticsComponent } from './production-analytics';

describe('ProductionAnalyticsComponent', () => {
  let component: ProductionAnalyticsComponent;
  let fixture: ComponentFixture<ProductionAnalyticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductionAnalyticsComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductionAnalyticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should be read-only because the backend exposes GET only', () => {
    expect(component.config.allowCreate).toBe(false);
    expect(component.config.allowEdit).toBe(false);
    expect(component.config.allowDelete).toBe(false);
  });
});
