import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { map, Observable, take, tap } from "rxjs";
import { AuthService } from "./auth.service";
import { User } from "./user.model";

@Injectable({
    providedIn: 'root'
})
export class AuthAdminGuard implements CanActivate {
    constructor(private authService: AuthService, private router: Router){
        
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Promise<boolean | UrlTree> | Observable<boolean | UrlTree>{
        return this.authService.user.pipe(
            take(1),
            map(user => {
            if(user === null){
                const userData: {
                    email: string;
                    name: string;
                    role: string;
                    _token: string;
                    _tokenExpirationDate: string;
                } = JSON.parse(localStorage.getItem('userData'));
                user = new User(
                    userData.email,
                    userData.name,
                    userData.role,
                    userData._token,
                    new Date(userData._tokenExpirationDate)
                );
            }
            const isAuth = !!user;
            if(isAuth && user.role === "Admin"){
                return true;
            }
            this.router.navigate(['/login']);
            return false;
        }), 
        )
        // tap(isAuth => {
        //     if(!isAuth){
        //         this.router.navigate(['/auth']);
        //     }
        // }));
    }
}