import { Configuration } from '@azure/msal-browser';

export const authConfig: Configuration = {
	auth: {
		clientId: 'f918913f-ac52-4550-9f9f-62de19cd6185',
		authority: 'https://otpdev.b2clogin.com/otpdev.onmicrosoft.com/B2C_1_SignUp_And_SignIn',
		redirectUri: 'http://localhost:3000/',
		knownAuthorities: ['otpdev.b2clogin.com'],
	},
	cache: {
		cacheLocation: 'sessionStorage',
		storeAuthStateInCookie: false,
	},
};

export const loginRequest = {
	scopes: ['https://otpdev.onmicrosoft.com/api/access_as_user'],
};