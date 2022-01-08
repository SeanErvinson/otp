import axios from 'axios';

import msalClient from '@/services/auth/msalClient';
import { InteractionRequiredAuthError } from '@azure/msal-browser';

const instance = axios.create({
	baseURL: import.meta.env.VITE_OTP_API_BASE_URL,
});

instance.interceptors.request.use(async config => {
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

export default instance;

const acquireAccessToken = async (): Promise<string | null> => {
	const activeAccount = msalClient.getActiveAccount();
	const accounts = msalClient.getAllAccounts();
	if (!activeAccount && accounts.length === 0) {
		console.log('No account');
	}

	const request = {
		scopes: ['https://otpdev.onmicrosoft.com/api/access_as_user'],
		account: activeAccount || accounts[0],
	};
	try {
		const authResult = await msalClient.acquireTokenSilent(request);
		return authResult.accessToken;
	} catch (error) {
		if (error instanceof InteractionRequiredAuthError) {
			msalClient.acquireTokenRedirect(request);
		}
	}
	return null;
};
