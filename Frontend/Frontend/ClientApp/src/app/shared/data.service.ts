import { Inject, Injectable, Component } from '@angular/core';
import { HttpModule, Http, Request, RequestMethod, Response, RequestOptions, Headers } from '@angular/http';
import { Observable, Subject, ReplaySubject } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';

@Component({
  providers: [Http]
})

@Injectable({
  providedIn: 'root'
})
export class DataService {
  public apiUrl: string = "http://localhost:55474/";
  constructor(
    private _http: Http,
    @Inject(DOCUMENT) private documet: any) { }
  //Get
  get(_getUrl: string): Observable<any> {
    var getUrl = this.apiUrl + _getUrl;
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    return this._http.get(getUrl, options)
      .pipe(map(res => res.json()))
      .pipe(catchError(this.handleError));
  }
  //Update the Login Status
  updateUser(model: any, userName: string, loginStatus: string, _updateByUserUrl: string): Observable<any> {
    var updateUserUrl = this.apiUrl + _updateByUserUrl + userName + '=' + loginStatus;
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let body = JSON.stringify(model);
    return this._http.put(updateUserUrl, body, options)
      .pipe(map(res => <any>res.json()))
      .pipe(catchError(this.handleError));
  }
  //Post
  save(model: any, _saveUrl: string): Observable<any> {
    var saveUrl = this.apiUrl + _saveUrl;
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    let body = JSON.stringify(model);
    return this._http.post(saveUrl, body, options)
      .pipe(map(res => res.json()))
      .pipe(catchError(this.handleError));
  }
  //Post Form Data
  saveForm(model: any, _saveUrl: string): Observable<any> {
    var saveUrl = this.apiUrl + _saveUrl;
    return this._http.post(saveUrl, model)
      .pipe(map(res => res.json()))
      .pipe(catchError(this.handleError));
  }
  //Delete the Logged User
  delete(userName: string, _deleteByNameUrl: string): Observable<any> {
    var deleteByNameUrl = this.apiUrl + _deleteByNameUrl + userName;
    return this._http.delete(deleteByNameUrl, userName)
      .pipe(map(res => res.json()))
      .pipe(catchError(this.handleError));
  }
  private handleError(error: Response) {
    return Observable.throw(error.json().error || 'Server Error');
  }
}
