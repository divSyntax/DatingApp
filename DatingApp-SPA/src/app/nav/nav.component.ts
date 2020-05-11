import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authServcie: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authServcie.login(this.model).subscribe(next =>
      {
          console.log('Signed in successful.');
      }, error => {
        console.log("Failed to login.");
      });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
    console.log('logged out');
  }

}
