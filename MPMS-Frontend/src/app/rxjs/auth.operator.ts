import { Subject } from "rxjs";

export const usernameSubject = new Subject<string|undefined>();

export const logout = () => {
    // Make sure we remove the exact names we saved in auth.services.ts
    sessionStorage.removeItem("accessToken");
    sessionStorage.removeItem("refreshToken");
    sessionStorage.removeItem("currentUser");
    usernameSubject.next(undefined);
}

export const isLoggedIn = () => {
    // Look for the new 'accessToken' key
    const token = sessionStorage.getItem("accessToken");
    return token ? true : false;
}

export const changeUsername = () => {
    // Look for the new 'accessToken' key
    const token = sessionStorage.getItem("accessToken");
    if (token) {
        const payload = JSON.parse(atob(token.split(".")[1]));
         const name = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"];
         if (name) {
            usernameSubject.next(name);
         }
    }
}
