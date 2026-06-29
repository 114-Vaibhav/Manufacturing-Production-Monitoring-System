import { HttpErrorResponse } from '@angular/common/http';
import { ApiErrorResponse } from '../models/api-error';

export function getApiErrorMessage(error: unknown, fallback: string): string {
  if (error instanceof HttpErrorResponse) {
    if (typeof error.error === 'string') {
      return error.error;
    }

    const apiError = error.error as ApiErrorResponse | null;
    return apiError?.message ?? error.message ?? fallback;
  }

  return fallback;
}
