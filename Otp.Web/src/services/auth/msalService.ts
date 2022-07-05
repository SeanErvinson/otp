import { InteractionRequiredAuthError } from '@azure/msal-browser';

import { loginRequest } from './authConfig';
import msalInstance from './msalInstance';

export class MsalService {
	static logout = (): void => {
		const activeAccount = msalInstance.getActiveAccount();

		msalInstance.logoutRedirect({
			account: activeAccount,
		});
	};

	static acquireAccessToken = async (): Promise<string | null> => {
		const account = msalInstance.getActiveAccount();
		if (!account) {
			throw Error(
				'No active account! Verify a user has been signed in and setActiveAccount has been called.',
			);
		}

		const request = {
			...loginRequest,
			account: account,
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
