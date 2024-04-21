import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import configurl from '../../../assets/config.json';
import { DemoService } from '../../services/demo.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  authService = inject(AuthService);
  config = {
    ApiUrl: configurl.apiServer.url,
  };
  title = 'WebApp';
  response = 'No data loaded, yet';
  constructor(private sharedService: DemoService) {}
  ngOnInit(): void {
    setTimeout(() => {
      this.getData();
    }, 2000);
  }
  getData() {
    this.sharedService.getData().subscribe((data) => {
      console.log(data);
      this.response = data;
    });
  }
  logout() {
    this.authService.logout();
  }
}
