import { MsalProvider } from '@azure/msal-react';
import 'normalize.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { QueryClientProvider } from 'react-query';
import { BrowserRouter } from 'react-router-dom';
import { ReactQueryDevtools } from 'react-query/devtools';

import { makeServer } from '@/api/mockOtpApi';
import queryClient from '@/api/queryClient';
import msalInstance from '@/services/auth/msalInstance';

import Root from './Root';
import './global.less';
import './index.css';

if (import.meta.env.VITE_ENABLE_MOCK_SERVER) {
	makeServer();
}

ReactDOM.render(
	<React.StrictMode>
		<MsalProvider instance={msalInstance}>
			<QueryClientProvider client={queryClient}>
				<BrowserRouter>
					<Root />
				</BrowserRouter>
				<ReactQueryDevtools initialIsOpen={false} />
			</QueryClientProvider>
		</MsalProvider>
	</React.StrictMode>,
	document.getElementById('root'),
);
