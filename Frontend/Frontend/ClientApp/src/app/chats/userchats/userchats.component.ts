import { Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { DataService } from 'src/app/shared/data.service';
import { Message } from 'src/app/shared/models/message';
import * as signalR from '@aspnet/signalr';
import { jsonpFactory } from '@angular/http/src/http_module';

@Component({
  selector: 'app-userchats',
  templateUrl: './userchats.component.html',
  styleUrls: ['./userchats.component.css'],
  providers: [DataService]
})
export class UserchatsComponent implements OnInit, OnDestroy {
  public title: any;
  public res: any;
  public resMessage: string;
  public loggedUsername: string;
  public options: any = [];
  public search = '';

  //API
  public _chatUrl: string = 'api/chat/getchat';
  public _getUrl: string = 'api/signup/getuser';

  //Chat
  public onlineUser: any = [];
  public chatUsername: string = null;
  public chatConnection: string;
  public chatMessages: any = [];
  public chatMessage: Message = null;
public _hubConnection;

  constructor(private titleService: Title,
    private _dataService: DataService) { 
      var loggedUser = JSON.parse(localStorage.getItem('logged'));
      this.loggedUsername = loggedUser;
      this._dataService.get(this._getUrl)
    .subscribe(response => {
      console.log("Signup Data : ", response);
      response.resdata.forEach(element => {
        if(this.loggedUsername!=element.userName) {
          this.options.push(element.userName);
        }
      });
      console.log("Registered User : ", this.options);
    });
    }

  ngOnInit() {
    this.titleService.setTitle("Chat");
    this.signalrConn();
  }

  selectedStatic(result) {
    this.search = result;
  }

  searchUser(value) {
    console.log("Searched : ", value);
    this.chatUsername = value;
    this.chatLog();
  }

  signalrConn() {
    //Init Connection
    this._hubConnection = new signalR.HubConnectionBuilder().withUrl("http://localhost:55474/chathub?user=" + this.loggedUsername).build();

    //Call client methods from hub to update user
    this._hubConnection.on('UpdateUserList', (onlineuser) => {
      var users = JSON.parse(onlineuser);
      console.log("User Data : ", users);
      this.onlineUser = [];
      for(var key in users) {
        if(users.hasOwnProperty(key)) {
          if(key!== this.loggedUsername) {
            this.onlineUser.push({
              userName: key,
              connection: users[key]
            });
          }
        }
      }
      console.log("Online User : ", this.onlineUser);
    });

    //Call client methods from hub to update user
    this._hubConnection.on('ReceiveMessage', (message: Message) => {
      this.chatUsername = message.senderId;
      this.chatLog();
    });

    //Start Connection
    this._hubConnection.start().then(function () {
      console.log("Connected");
    }).catch(function (err) {
      return console.error(err.toString());
    });
  }

  sendMessage(message) {
    //SendMessage
    if(message!='') {
      this.chatMessage = new Message();
      this.chatMessage.senderId = this.loggedUsername;
      this.chatMessage.receiverId = this.chatUsername;
      this.chatMessage.message = message;
      this.chatMessage.messageStatus = "sent";
      this.chatMessages.push(this.chatMessage);
      this._hubConnection.invoke('SendMessage', this.chatMessage);
    }
  }

  chooseUser(user) {
    this.chatUsername = user.userName;
    this.chatLog();
  }

  chatLog() {
    //ChatLog
    var param = { senderId: this.loggedUsername, receiverId: this.chatUsername };
    var getchatUrl = this._chatUrl + '?param=' + JSON.stringify(param);
    this._dataService.get(getchatUrl)
    .subscribe(response => {
      this.res = response;
      if(this.res!=null) {
        var chatLog = this.res.resdata;
        this.chatMessages = [];
        if(chatLog.length > 0) {
          for(var i=0;i<chatLog.length;i++) {
            if(this.loggedUsername===chatLog[i].senderId) {
              chatLog[i].messageStatus = "sent";
            }
            else {
              chatLog[i].messageStatus = "received";
            }
            this.chatMessages.push(chatLog[i]);
          }
        }
      }
    }, error => {
      console.log(error);
    });
  }

  ngOnDestroy() {
    //Stop Connection
    this._hubConnection.stop().then(function () {
      console.log("Stopped Connection");
    }).catch(function (err) {
      return console.error(err.toString());
    });
  }
}