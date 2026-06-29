import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { MachinesComponent } from './machines';

describe('MachinesComponent', () => {
  let component: MachinesComponent;
  let fixture: ComponentFixture<MachinesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MachinesComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(MachinesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should point to the Machines API', () => {
    expect(component.config.endpoint).toBe('/api/Machines');
  });
});
