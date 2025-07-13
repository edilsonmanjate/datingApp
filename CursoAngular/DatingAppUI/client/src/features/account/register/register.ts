import { Component, output } from '@angular/core';

@Component({
  selector: 'app-register',
  imports: [],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
    cancelRegister = output<boolean>();

   cancel() {
    this.cancelRegister.emit(false);
  }

}
