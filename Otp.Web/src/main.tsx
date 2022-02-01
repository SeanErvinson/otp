import 'normalize.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { QueryClientProvider } from 'react-query';
import { BrowserRouter } from 'react-router-dom';

import queryClient from '@/api/queryClient';

import Root from './Root';
import './global.less';
import './index.css';

ReactDOM.render(
	<React.StrictMode>
		<QueryClientProvider client={queryClient}>
			<BrowserRouter>
				<Root />
			</BrowserRouter>
		</QueryClientProvider>
	</React.StrictMode>,
	document.getElementById('root'),
);
