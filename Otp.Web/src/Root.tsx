import { InteractionType } from '@azure/msal-browser';
import { useMsalAuthentication } from '@azure/msal-react';
import { useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';

import Sidebar from '@/components/Sidebar';
import { Home } from '@/pages/Home';
import { Billing } from '@/pages/Billing';
import { Apps } from '@/pages/Apps';
import { App } from '@/pages/Apps/App';
import { Key } from '@/pages/Apps/App/Key';
import { Branding } from '@/pages/Apps/App/Branding';
import { loginRequest } from '@/services/auth/authConfig';

const Root = () => {
	const { login, result, error } = useMsalAuthentication(InteractionType.Redirect, loginRequest);

	useEffect(() => {
		if (error) {
			login();
		}
	}, []);

	return (
		<Sidebar>
			<Routes>
				<Route path="/" element={<Home />} />
				<Route path="/apps" element={<Apps />} />
				<Route path="/apps/:appId" element={<App />}>
					<Route path="key" element={<Key />} />
					<Route path="branding" element={<Branding />} />
				</Route>
				<Route path="/billing" element={<Billing />} />
			</Routes>
		</Sidebar>
	);
};

export default Root;
