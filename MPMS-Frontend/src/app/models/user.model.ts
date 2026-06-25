import { UserRole, UserStatus } from './enum';
export interface User {
    id: number;
    username: string;
    fullName: string;
    role: UserRole;
    email: string;
    status: UserStatus;
    // token: string;
    accessToken: string;
    refreshToken: string;
}

