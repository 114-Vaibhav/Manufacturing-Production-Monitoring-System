import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { MaintenanceLogsComponent } from './maintenance-logs';

describe('MaintenanceLogsComponent', () => {
  let component: MaintenanceLogsComponent;
  let fixture: ComponentFixture<MaintenanceLogsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MaintenanceLogsComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(MaintenanceLogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should use DTO fields for maintenance writes', () => {
    expect(component.config.formFields.map(field => field.key)).toEqual([
      'machineId',
      'issueDescription',
      'resolution',
    ]);
  });
});
