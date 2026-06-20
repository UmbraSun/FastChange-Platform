import { apiClient } from '../../../shared/api/apiClient';
import { RegisterRequest, RegisterResponse } from '../model/dto';

export const registerUser = async (data: RegisterRequest): Promise<RegisterResponse> => {
  const response = await apiClient.post<RegisterResponse>('/auth/register', data);
  return response.data;
};