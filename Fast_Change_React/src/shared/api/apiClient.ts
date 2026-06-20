import axios from 'axios';

export const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7001/api/v1',
    headers: {
      'Content-Type': 'application/json',
    },
  });
  
  apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response && error.response.data) {
        // Standard backend Problem Details format
        const problem = error.response.data;
        console.error(`Backend Error [${problem.status}]: ${problem.title}`, problem.errors);
        return Promise.reject(problem);
      }
      return Promise.reject(error);
    }
  );