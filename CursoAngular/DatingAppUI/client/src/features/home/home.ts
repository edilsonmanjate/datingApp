import { Component, signal } from '@angular/core';
import { Register } from "../../features/account/register/register";

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  //@Input({required: true}) membesFromApp:  User[] = [];
  protected registerMode = signal(false);

  showRegister(value: boolean ) {
    this.registerMode.set(value);
  }


}
