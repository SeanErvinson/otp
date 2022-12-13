import {
	AuthenticationResult,
	EventMessage,
	EventType,
	PublicClientApplication,
} from '@azure/msal-browser';

import { authConfig } from './authConfig';

const msalInstance = new PublicClientApplication(authConfig);

const accounts = msalInstance.getAllAccounts();

if (!msalInstance.getActiveAccount() && msalInstance.getAllAccounts().length > 0) {
	msalInstance.setActiveAccount(accounts[0]);
}

msalInstance.enableAccountStorageEvents();

msalInstance.addEventCallback((event: EventMessage) => {
	if (event.eventType === EventType.LOGIN_SUCCESS && event.payload) {
		const payload = event.payload as AuthenticationResult;
		const account = payload.account;
		msalInstance.setActiveAccount(account);
	}
});

export default msalInstance;
