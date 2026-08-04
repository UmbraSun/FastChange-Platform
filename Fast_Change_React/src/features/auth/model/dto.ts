export interface RegisterRequest {
  email: string;
  password: string;
}

export interface RegisterResponse {
  userId: string;
  message: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken?: string;
}
