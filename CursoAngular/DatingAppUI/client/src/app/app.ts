import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom, Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private http = inject(HttpClient);
  protected readonly title = 'Dating app';
  protected members = signal<any>([]);

  async ngOnInit() {
    this.members.set(await this.getMembers());

  }


  async getMembers() {
    try {
      return await lastValueFrom(this.http.get('https://localhost:5001/api/members'));
    }
    catch (error) {
      console.error('Error in getMembers:', error);
      throw error;
    }

  }
}
