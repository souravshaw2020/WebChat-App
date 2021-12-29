import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertService } from '../../shared/alert.service';
import { DataService } from '../../shared/data.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  public emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";
  public signupForm: FormGroup;
  public _saveUrl: string = 'api/signup/register';

  constructor(private alertService: AlertService,
    private dataService: DataService,
    private formBuilder: FormBuilder,
    private router: Router
  ) { }

  ngOnInit() {
    this.resetForm();
  }

  get f() {
    return this.signupForm.controls;
  }

  resetForm() {
    this.signupForm = this.formBuilder.group({
      userName: new FormControl(''),
      userPassword: new FormControl(''),
      userEmail: new FormControl('')
    });
  }

  onSubmit() {
    if (this.signupForm.invalid)
      return;
    this.alertService.clear();
    this.dataService.save(this.signupForm.value, this._saveUrl)
      .subscribe(response => {
        console.log(response);
        if (response.resdata != null) {
          this.router.navigate(['/load']);
          this.alertService.success(response.resdata, true);
          setTimeout(() => {
            this.alertService.clear();
            this.router.navigate(['/login']);
          }, 5000);
        }
      },
        error => {
          this.alertService.error(error,true);
        });
  }
}
