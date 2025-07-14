import { HttpEvent, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { BusyService } from '../services/busy-service';
import { delay, finalize, of, tap } from 'rxjs';

const cache = new Map<string, HttpEvent<unknown>>();

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyService);

  if(req.method === 'GET' && cache.has(req.url)) {
    const cachedResponse = cache.get(req.url);
    if (cachedResponse) {
      return of(cachedResponse);
    }
  }

  busyService.busy();


  return next(req).pipe(
    delay(500), // Simulate a delay for demonstration purposes
    tap(response => {
      cache.set(req.url, response);
      console.log(`Response cached for ${req.url}`);
    }),
    finalize(() => {
      busyService.idle(); 
    })
  );
};
