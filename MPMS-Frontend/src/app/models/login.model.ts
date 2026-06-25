// 1. Move the enum outside the class

export class LoginModel {

    constructor(
        public UserName: string = "", 
        public Password: string = "", 
        public Role: number = -1
    ) {}
}