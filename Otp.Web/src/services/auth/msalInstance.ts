import { PublicClientApplication } from '@azure/msal-browser';

import { authConfig } from './authConfig';

const msalInstance = new PublicClientApplication(authConfig);

export default msalInstance;
