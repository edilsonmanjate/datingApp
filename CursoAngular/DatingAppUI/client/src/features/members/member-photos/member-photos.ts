import { Component, inject } from '@angular/core';
import { MemberService } from '../../../core/services/member-service';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Photo } from '../../../types/member';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-member-photos',
  imports: [AsyncPipe],
  templateUrl: './member-photos.html',
  styleUrl: './member-photos.css'
})
export class MemberPhotos {
  private memberService = inject(MemberService);
  private route = inject(ActivatedRoute);

  protected memberPhotos$: Observable<Photo[]>;

  constructor() {
    const memberId = this.route.parent?.snapshot.paramMap.get('id');
    if (memberId) {
      this.memberPhotos$ = this.memberService.getMemberPhotos(memberId);
    } else {
      this.memberPhotos$ = new Observable<Photo[]>(observer => {
        observer.next([]);
        observer.complete();
      });
    }
  }

  get photoMocks(){
    return Array.from({ length: 10 }, (_, i) => ({
      url: '/user.png'
    }));

  }

  

}
