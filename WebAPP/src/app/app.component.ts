import { Component } from '@angular/core';
import configurl from '../assets/config.json';
import { DemoService } from './services/demo.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  config = {
    ApiUrl: configurl.apiServer.url,
  };
  title = 'WebApp';
  response = 'No data loaded, yet';
  constructor(private sharedService: DemoService) {}
  ngOnInit(): void {
    setTimeout(() => {
      this.refreshList();
    }, 2000);
  }
  refreshList() {
    this.sharedService.getData().subscribe((data) => {
      console.log(data);
      this.response = data;
    });
  }
}
