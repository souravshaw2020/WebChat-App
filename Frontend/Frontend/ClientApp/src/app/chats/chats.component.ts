import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { AlertService } from '../shared/alert.service';
import { DataService } from '../shared/data.service';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.css'],
  providers: [DataService]
})
export class ChatsComponent implements OnInit, AfterViewInit {
  public title: any;
  public signupData: any;
  public loggedUsername: string;
  public _getUrl: string = 'api/signup/getuser';
  public _updateUrl: string = 'api/signup/update/';
  public _deleteUrl: string = 'api/login/deleteduser/';
  public loginStatus: string = 'offline';

  constructor(private titleService: Title,
    private router: Router,
    private alertService: AlertService,
    private _dataService: DataService) {
    var logged = JSON.parse(localStorage.getItem('logged'));
    this.loggedUsername = logged;
  }

  ngOnInit() {
    this.titleService.setTitle("Chat Application");
  }

  ngAfterViewInit() { }

  logout() {
    localStorage.removeItem('logged');
    this._dataService.get(this._getUrl)
      .subscribe(response => {
        console.log("Signup Data : ", response);
        this.signupData = response.resdata;
      });

    this._dataService.updateUser(this.signupData, this.loggedUsername, this.loginStatus, this._updateUrl)
      .subscribe(response => {
        console.log("Updated Data : ", response);
      });

    this._dataService.delete(this.loggedUsername, this._deleteUrl)
      .subscribe(response => {
        console.log("Deleted Data : ", response);
      });
      this.router.navigate(['/load']);
      this.alertService.success("Logged Out Successfully!", true);
      setTimeout(() => {
        this.alertService.clear();
        this.router.navigate(['/login']);
        }, 5000);
  }
}
