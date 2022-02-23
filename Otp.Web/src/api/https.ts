import axios from 'axios';

import msalInstance from '@/services/auth/msalInstance';
import { InteractionRequiredAuthError } from '@azure/msal-browser';

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
	var token = await acquireAccessToken();

	if (!!token) {
		config.headers = {
			Authorization: `Bearer ${token}`,
			Accept: 'application/json',
			'Content-Type': 'application/json;charset=UTF-8',
		};
	}

	return config;
});

const acquireAccessToken = async (): Promise<string | null> => {
	const activeAccount = msalInstance.getActiveAccount();
	const accounts = msalInstance.getAllAccounts();
	if (!activeAccount && accounts.length === 0) {
		console.log('No account');
	}

	const request = {
		scopes: ['https://otpdev.onmicrosoft.com/api/access_as_user'],
		account: activeAccount || accounts[0],
	};
	try {
		const authResult = await msalInstance.acquireTokenSilent(request);
		return authResult.accessToken;
	} catch (error) {
		if (error instanceof InteractionRequiredAuthError) {
			msalInstance.acquireTokenRedirect(request);
		}
	}
	return null;
};
