import { InteractionType } from '@azure/msal-browser';
import { MsalProvider, useMsalAuthentication } from '@azure/msal-react';
import { useEffect } from 'react';
import { Outlet, Route, Routes } from 'react-router-dom';

import { Sidebar } from '@/components/Sidebar';
import { Home } from '@/pages/Home';
import { Billing } from '@/pages/Billing';
import { Channel } from '@/pages/Channel';
import { Usage } from '@/pages/Usage';
import { Apps } from '@/pages/Apps';
import { NotFound } from '@/pages/NotFound';
import { App } from '@/pages/Apps/App';
import { Settings } from '@/pages/Apps/App/Settings';
import { loginRequest } from '@/services/auth/authConfig';
import msalClient from './services/auth/msalClient';

const SidebarLayout = () => {
	const { login, result, error } = useMsalAuthentication(InteractionType.Redirect, loginRequest);

	useEffect(() => {
		if (error) {
			login();
		}
	}, []);

	return (
		<Sidebar>
			<Outlet />
		</Sidebar>
	);
};

const Root = () => {
	return (
		<>
			<MsalProvider instance={msalClient}>
				<Routes>
					<Route path="/" element={<SidebarLayout />}>
						<Route path="" element={<Home />} />
						<Route path="apps" element={<Apps />} />
						<Route path="apps/:appId" element={<App />}>
							<Route path="" element={<Settings />} />
						</Route>
						<Route path="billing" element={<Billing />} />
						<Route path="usage" element={<Usage />} />
					</Route>
				</Routes>
			</MsalProvider>
			<Routes>
				<Route path="*" element={<NotFound />} />
				<Route path="/sms/:requestId" element={<Channel />} />
				<Route path="/email/:requestId" element={<Channel />} />
			</Routes>
		</>
	);
};

export default Root;
