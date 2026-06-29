import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { DefectsComponent } from './defects';

describe('DefectsComponent', () => {
  let component: DefectsComponent;
  let fixture: ComponentFixture<DefectsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DefectsComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(DefectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should use the Defects API endpoint', () => {
    expect(component.config.endpoint).toBe('/api/Defects');
  });
});
