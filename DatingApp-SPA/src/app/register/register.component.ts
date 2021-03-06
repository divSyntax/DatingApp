import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;
  @Output() cancelReg = new EventEmitter();

  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
  }

  register()
  {
    this.authService.register(this.model).subscribe(() =>
    {
        console.log("Registration successful");
    },error =>
    {
      console.log(error);
    });
  }

  cancel()
  {
    this.cancelReg.emit(false);
    console.log("Cancelled");
  }

}
