import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ApiResourceComponent, ResourceConfig } from './api-resource';

describe('ApiResourceComponent', () => {
  let component: ApiResourceComponent;
  let fixture: ComponentFixture<ApiResourceComponent>;

  const config: ResourceConfig = {
    title: 'Products',
    description: 'Manage products.',
    endpoint: '/api/Products',
    idKey: 'productId',
    columns: [{ key: 'productName', label: 'Name', type: 'text' }],
    formFields: [{ key: 'productName', label: 'Name', type: 'text', required: true }],
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApiResourceComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ApiResourceComponent);
    component = fixture.componentInstance;
    component.config = config;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form values from configured fields', () => {
    expect(component.form['productName']).toBe('');
  });
});
