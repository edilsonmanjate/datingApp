import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterCreds, User } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  //membersFromHome = input.required<User[]>();
  private accountService = inject(AccountService) // Replace with actual account service type
  //protected registerMode = input<boolean>(false);
  cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;

  register() {
    console.log('Registering user with credentials:', this.creds);
    this.accountService.register(this.creds).subscribe({
      next: (user: User) => {
        console.log('User registered successfully:', user);
        this.cancel();
      },
      error: (error) => {
        console.error('Registration failed:', error);
      },
      complete: () => {
        console.log('Registration process completed');
      }
    });
 
  }

  cancel() {
    console.log('Registration cancelled');
    this.cancelRegister.emit(false);
  }

}
