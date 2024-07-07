import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FirstpageComponent } from './firstpage.component';

describe('FirstpageComponent', () => {
  let component: FirstpageComponent;
  let fixture: ComponentFixture<FirstpageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FirstpageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FirstpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
