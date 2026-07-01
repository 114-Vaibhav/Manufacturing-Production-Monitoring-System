import { HttpInterceptorFn, HttpErrorResponse, HttpRequest, HttpHandlerFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { AuthApiService } from './services/auth.services';

// State variables kept outside the function so they persist across requests
let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthApiService);
  const token = sessionStorage.getItem('accessToken');

  // 1. Attach the token if we have one
  let requestToProcess = req;
  if (token) {
    requestToProcess = addTokenHeader(req, token);
  }

  // 2. Send the request and catch errors
  return next(requestToProcess).pipe(
    catchError((error: HttpErrorResponse) => {
      
      // Prevent infinite loops by making sure this isn't the refresh request itself failing
      const isRefreshRequest = requestToProcess.url.includes('refresh-token');
      
      // If unauthorized and it's a standard API call, attempt to refresh
      if (error.status === 401 && !isRefreshRequest) {
        return handle401Error(requestToProcess, next, authService, router);
      }
      
      return throwError(() => error);
    })
  );
};

// Helper function to cleanly clone and attach the header
function addTokenHeader(request: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
  return request.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });
}

// Logic to handle the refresh process and queue concurrent requests
function handle401Error(
  request: HttpRequest<unknown>, 
  next: HttpHandlerFn, 
  authService: AuthApiService, 
  router: Router
) {
  if (!isRefreshing) {
    // Block other requests from attempting to refresh simultaneously
    isRefreshing = true;
    refreshTokenSubject.next(null); // Reset the subject

    // Call your backend to refresh the token
    return authService.refreshToken().pipe(
      switchMap(() => {
        isRefreshing = false;
        
        // Grab the newly saved token from sessionStorage
        const newToken = sessionStorage.getItem('accessToken');
        if (newToken) {
          refreshTokenSubject.next(newToken); // Release the queued requests
          return next(addTokenHeader(request, newToken)); // Retry the original request
        }
        
        throw new Error('Failed to retrieve new token after refresh');
      }),
      catchError((refreshError) => {
        // If the refresh token is dead/expired, wipe data and kick them to login
        isRefreshing = false;
        authService.logout(); // Use the service logout method to ensure signals are cleared
        router.navigate(['/login']);
        
        return throwError(() => refreshError);
      })
    );
  } else {
    // If a refresh is already in progress, wait for it to finish
    return refreshTokenSubject.pipe(
      filter((token): token is string => token !== null), // Wait until token is not null
      take(1), // Take the newly emitted token and complete this observable
      switchMap((token) => next(addTokenHeader(request, token))) // Retry original request
    );
  }
}