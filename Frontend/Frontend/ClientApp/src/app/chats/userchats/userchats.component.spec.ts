import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserchatsComponent } from './userchats.component';

describe('UserchatsComponent', () => {
  let component: UserchatsComponent;
  let fixture: ComponentFixture<UserchatsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserchatsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserchatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
