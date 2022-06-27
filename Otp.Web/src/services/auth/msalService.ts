import { InteractionRequiredAuthError } from '@azure/msal-browser';
import msalInstance from './msalInstance';

export class MsalService {
	static getActiveAccount = () => {
		const activeAccount = msalInstance.getActiveAccount();
		const accounts = msalInstance.getAllAccounts();
		if (!activeAccount && accounts.length === 0) {
			console.log('No account');
		}
		return activeAccount || accounts[0];
	};

	static acquireAccessToken = async (): Promise<string | null> => {
		const activeAccount = MsalService.getActiveAccount();

		const request = {
			scopes: ['https://otpdev.onmicrosoft.com/api/access_as_user'],
			account: activeAccount,
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
}
