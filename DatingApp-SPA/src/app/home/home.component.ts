import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from "../../environments/environment";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  values:any;

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getValues();
  }

  registerToggle()
  {
    this.registerMode = true;
  }

  getValues()
  {
    this.http.get(environment.valueURL).subscribe(
      response => {
        this.values = response;
      },
      error => {
        console.log(error);
      }
    );
  }

  cancelRegistrationMode(registerMode: boolean)
  {
      this.registerMode = registerMode;
  }

}
