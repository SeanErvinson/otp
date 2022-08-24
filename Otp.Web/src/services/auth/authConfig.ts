import { Configuration, RedirectRequest } from '@azure/msal-browser';

export const authConfig: Configuration = {
	auth: {
		clientId: import.meta.env.VITE_B2C_CONFIG_CLIENT_ID,
		authority: import.meta.env.VITE_B2C_CONFIG_AUTHORITY,
		redirectUri: import.meta.env.VITE_B2C_CONFIG_REDIRECT_URI,
		knownAuthorities: [import.meta.env.VITE_B2C_CONFIG_KNOWN_AUTHORITY],
	},
};

export const loginRequest: RedirectRequest = {
	scopes: ['https://otpdev.onmicrosoft.com/api/access_as_user'],
};
