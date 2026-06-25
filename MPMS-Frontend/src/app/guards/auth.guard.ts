import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { isLoggedIn } from "../rxjs/auth.operator";

export const authGuard: CanActivateFn = () => {
    const router = inject(Router);

    const userStatus = isLoggedIn();
    
    // If the token exists, let them pass
    if (userStatus) {
        return true;
    }

    // If no token, bounce them to the login page
    router.navigate(["/login"]);
    return false;
}