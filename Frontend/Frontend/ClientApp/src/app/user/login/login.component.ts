import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AlertService } from '../../shared/alert.service';
import { DataService } from '../../shared/data.service';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, AfterViewInit {
  isLoginError: boolean = false;
  public title: any;
  public loginForm: FormGroup;
  public _saveUrl: string = 'api/login/signin';
  public _updateUrl: string = 'api/signup/update/';
  public loginStatus: string = 'online';

  constructor(private dataService: DataService,
    private alertService: AlertService,
    private titleService: Title,
    private router: Router,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.titleService.setTitle("Login");
    this.createForm();
  }

  ngAfterViewInit() { }

  get f() {
    return this.loginForm.controls;
  }

  createForm() {
    this.loginForm = this.formBuilder.group({
      userName: new FormControl('', Validators.required),
      userPassword: new FormControl('', Validators.required)
    });
    //$("#userName").focus();
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }
    this.alertService.clear();

    this.dataService.save(this.loginForm.value, this._saveUrl)
      .subscribe(response => {
        console.log("Login Status : ", response);
        var loggedUser = this.loginForm.get('userName').value;
        console.log(loggedUser);
        if (response.resdata != null) {
          localStorage.setItem('logged', JSON.stringify(loggedUser));
          this.dataService.updateUser(this.loginForm.value, loggedUser, this.loginStatus, this._updateUrl)
            .subscribe(response => {
              console.log("Updated : ", response);
            });
          this.router.navigate(['/load']);
          setTimeout(() => {
            this.alertService.success(response.resdata, true);
            this.router.navigate(['/chat']);
            }, 3000);
          setTimeout(() => {
            this.alertService.clear();
          }, 5000);
        }
        else {
          this.isLoginError = true;
        }
      },
        error => {
          this.alertService.error(error,true);
        });
  }
}
