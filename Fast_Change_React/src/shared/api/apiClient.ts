import axios, { type AxiosError, type AxiosResponse } from 'axios';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7001/api/v1',
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error: AxiosError) => {
    if (error.response && error.response.data) {
      const problem = error.response.data as { status?: number; title?: string; errors?: unknown };
      console.error(`Backend Error [${problem.status}]: ${problem.title}`, problem.errors);
      return Promise.reject(problem);
    }
    return Promise.reject(error);
  }
);
