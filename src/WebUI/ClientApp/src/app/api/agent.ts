/* eslint-disable no-debugger */
import axios, { AxiosError, AxiosResponse } from 'axios';
import { toast } from 'react-toastify';
import { store } from '../store';
import { LoginQuery, RegisterCommand, UserDto, ResetPasswordCommand } from './web-api-dtos';
//  import { toast } from 'react-toastify';

axios.defaults.baseURL = process.env.REACT_APP_API_URL;
axios.defaults.withCredentials = true;

axios.interceptors.request.use((config) => {
    const token = store.getState().account.user?.token;
    if (token && config.headers) config.headers.Authorization = `Bearer ${token}`;
    return config;
});

axios.interceptors.response.use(
    (response: AxiosResponse) => {
        return response;
    },
    (error: AxiosError) => {
        if (error.response) {
            const { data, status } = error.response;
            switch (status) {
                case 400:
                case 401:
                    toast.error(data.title);
                    break;
                case 500:
                    toast.error('An error happened');
                    break;
                default:
                    break;
            }
        }
        return Promise.reject(error.response);
    },
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: unknown) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: unknown) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Account = {
    login: (loginDto: LoginQuery): Promise<UserDto> => requests.post<UserDto>('account/login', loginDto),
    register: (registerDto: RegisterCommand): Promise<UserDto> =>
        requests.post<UserDto>('account/register', registerDto),
    currentUser: (): Promise<UserDto> => requests.get<UserDto>('account'),
    verifyEmail: (token: string, email: string): Promise<void> =>
        requests.post<void>(`account/verifyEmail?token=${token}&email=${email}`, {}),
    resendEmailConfirmation: (email: string): Promise<void> =>
        requests.get<void>(`account/resendEmailConfirmationLink?email=${email}`),
    forgotPassword: (email: string): Promise<void> => requests.post<void>(`account/forgotPassword?email=${email}`, {}),
    resetPassword: (command: ResetPasswordCommand): Promise<void> =>
        requests.post<void>(`account/resetPassword`, command),
};

const Event = {
    get: (): Promise<Event[]> => requests.get<Event[]>('event'),
};

const agent = {
    Account,
    Event,
};

export default agent;
