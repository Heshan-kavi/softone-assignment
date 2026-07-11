import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const basicAuthInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const authorization = auth.authorizationHeader;

  if (!authorization) {
    return next(req);
  }

  return next(req.clone({
    setHeaders: { Authorization: authorization }
  }));
};
