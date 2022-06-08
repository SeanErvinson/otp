import { InteractionType } from '@azure/msal-browser';
import { useIsAuthenticated, useMsalAuthentication } from '@azure/msal-react';
import { useEffect } from 'react';
import { Outlet, Route, Routes } from 'react-router-dom';

import { Sidebar } from '@/components/Sidebar';
import { Home } from '@/pages/Home';
import { Billing } from '@/pages/Billing';
import { Channel } from '@/pages/Channel';
import { Logs } from '@/pages/Logs';
import { Apps } from '@/pages/Apps';
import { NotFound } from '@/pages/NotFound';
import { App } from '@/pages/Apps/App';
import { Settings } from '@/pages/Apps/App/Settings';
import { RecentCallbacks } from '@/pages/Apps/App/RecentCallbacks';
import { loginRequest } from '@/services/auth/authConfig';
import { Loader } from './components/Loader';

const SidebarLayout = () => {
	const isAuthenticated = useIsAuthenticated();
	const { login, error } = useMsalAuthentication(InteractionType.Redirect, loginRequest);

	useEffect(() => {
		if (error) {
			login();
		}
	}, []);

	return isAuthenticated ? (
		<Sidebar>
			<Outlet />
		</Sidebar>
	) : (
		<Loader />
	);
};

const Root = () => {
	return (
		<Routes>
			<Route path="/" element={<SidebarLayout />}>
				<Route path="" element={<Home />} />
				<Route path="apps" element={<Apps />} />
				<Route path="apps/:appId" element={<App />}>
					<Route path="" element={<Settings />} />
					<Route path="recent-callbacks" element={<RecentCallbacks />} />
				</Route>
				<Route path="billing" element={<Billing />} />
				<Route path="logs" element={<Logs />} />
			</Route>
			<Route path="/sms/:requestId" element={<Channel />} />
			<Route path="/email/:requestId" element={<Channel />} />
			<Route path="*" element={<NotFound />} />
		</Routes>
	);
};

export default Root;
