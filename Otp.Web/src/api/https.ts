import axios, { AxiosError, AxiosInstance, AxiosRequestConfig } from 'axios';

import { MsalService } from '@/services/auth/msalService';

export const otpInstance = (key: string) => {
	return axios.create({
		baseURL: import.meta.env.VITE_OTP_API_BASE_URL,
		headers: {
			OTP_KEY: key,
		},
	});
};

export const oauthInstance = axios.create({
	baseURL: import.meta.env.VITE_OTP_API_BASE_URL,
});

oauthInstance.interceptors.request.use(async config => {
	var token = await MsalService.acquireAccessToken();

	if (!!token) {
		config.headers = {
			Authorization: `Bearer ${token}`,
			Accept: 'application/json',
			'Content-Type': 'application/json;charset=UTF-8',
		};
	}

	return config;
});

export const request = async <T>(client: AxiosInstance, options: AxiosRequestConfig) => {
	return client
		.request<T>(options)
		.then(response => response.data)
		.catch((error: AxiosError) => Promise.reject(error.response));
};
