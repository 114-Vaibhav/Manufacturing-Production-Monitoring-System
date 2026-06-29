import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetLogs } from './get-logs';

describe('GetLogs', () => {
  let component: GetLogs;
  let fixture: ComponentFixture<GetLogs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetLogs],
    }).compileComponents();

    fixture = TestBed.createComponent(GetLogs);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
