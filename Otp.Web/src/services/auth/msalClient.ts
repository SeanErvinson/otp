import { PublicClientApplication } from '@azure/msal-browser';

import { authConfig } from './authConfig';

const msalClient = new PublicClientApplication(authConfig);

export default msalClient;
