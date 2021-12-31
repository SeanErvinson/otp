import { MsalProvider } from '@azure/msal-react';
import 'normalize.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { QueryClientProvider } from 'react-query';
import { BrowserRouter } from 'react-router-dom';

import msalClient from '@/services/auth/msalClient';
import queryClient from '@/api/queryClient';

import Root from './Root';
import './global.less';
import './index.css';

ReactDOM.render(
	<React.StrictMode>
		<MsalProvider instance={msalClient}>
			<QueryClientProvider client={queryClient}>
				<BrowserRouter>
					<Root />
				</BrowserRouter>
			</QueryClientProvider>
		</MsalProvider>
	</React.StrictMode>,
	document.getElementById('root'),
);
