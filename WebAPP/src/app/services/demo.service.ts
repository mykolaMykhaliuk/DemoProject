import { Injectable } from '@angular/core';
import * as configurl from '../../assets/config.json';
import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class DemoService {
  config = {
    ApiUrl: configurl.apiServer.url,
  };
  constructor(private http: HttpClient) {
    this.getJSON().subscribe((data) => {
      this.config.ApiUrl = data.apiServer.url;
    });
  }
  public getJSON(): Observable<any> {
    return this.http.get('./assets/config.json');
  }
  getData(): Observable<any> {
    return this.http.get(this.config.ApiUrl + '/Demo', {
      responseType: 'text',
    });
  }
}
