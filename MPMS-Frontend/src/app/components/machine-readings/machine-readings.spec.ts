import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { MachineReadingsComponent } from './machine-readings';

describe('MachineReadingsComponent', () => {
  let component: MachineReadingsComponent;
  let fixture: ComponentFixture<MachineReadingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MachineReadingsComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(MachineReadingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not create machine readings because backend POST is disabled', () => {
    expect(component.config.allowCreate).toBe(false);
  });
});
