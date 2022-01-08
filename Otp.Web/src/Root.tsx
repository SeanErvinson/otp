import { InteractionType } from '@azure/msal-browser';
import { useMsalAuthentication } from '@azure/msal-react';
import { useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';

import { Sidebar } from '@/components/Sidebar';
import { Home } from '@/pages/Home';
import { Billing } from '@/pages/Billing';
import { Usage } from '@/pages/Usage';
import { Apps } from '@/pages/Apps';
import { App } from '@/pages/Apps/App';
import { Settings } from '@/pages/Apps/App/Settings';
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
					<Route path="" element={<Settings />} />
				</Route>
				<Route path="/billing" element={<Billing />} />
				<Route path="/usage" element={<Usage />} />
			</Routes>
		</Sidebar>
	);
};

export default Root;
