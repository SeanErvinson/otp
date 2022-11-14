import 'normalize.css';
import './global.less';
import './index.css';

import { MsalProvider } from '@azure/msal-react';
import { QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';

import { makeServer } from '@/api/mockOtpApi';
import queryClient from '@/api/queryClient';
import msalInstance from '@/services/auth/msalInstance';

import Root from './Root';
import { SubscriptionProvider } from './contexts/SubscriptionContext';

if (import.meta.env.VITE_ENABLE_MOCK_SERVER) {
	makeServer();
}

ReactDOM.render(
	<React.StrictMode>
		<MsalProvider instance={msalInstance}>
			<QueryClientProvider client={queryClient}>
				<SubscriptionProvider>
					<BrowserRouter>
						<Root />
					</BrowserRouter>
				</SubscriptionProvider>
				<ReactQueryDevtools position="bottom-right" initialIsOpen={false} />
			</QueryClientProvider>
		</MsalProvider>
	</React.StrictMode>,
	document.getElementById('root'),
);
