import { InteractionType } from '@azure/msal-browser';
import { useIsAuthenticated, useMsalAuthentication } from '@azure/msal-react';
import { useEffect } from 'react';
import { Outlet, Route, Routes } from 'react-router-dom';

import { Sidebar } from '@/components/Sidebar';
import Dashboard from '@/modules/Dashboard/views/Dashboard';
import Billing from '@/modules/Billing/views/Billing';
import Channel from '@/modules/Channel/views/Channel';
import Logs from '@/modules/Logs/views/Logs';
import Apps from '@/modules/Apps/views/Apps';
import AppDetails from '@/modules/Apps/views/AppDetails';
import Settings from '@/modules/Apps/views/Settings/Settings';
import NotFound from '@/modules/common/views/NotFound';
import { RecentCallbacks } from '@/modules/RecentCallbacks';

import { Loader } from './components/Loader';
import useUserConfig from './hooks/useUserConfig';
import { loginRequest } from './libs/azureB2C/authConfig';

const SidebarLayout = () => {
	const isAuthenticated = useIsAuthenticated();
	const { userConfigQuery } = useUserConfig();
	const { login, error } = useMsalAuthentication(InteractionType.Redirect, loginRequest);

	useEffect(() => {
		if (error) {
			login();
		}
	}, []);

	return isAuthenticated && userConfigQuery.isSuccess ? (
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
				<Route path="" element={<Dashboard />} />
				<Route path="apps">
					<Route path="" element={<Apps />} />
					<Route path=":appId" element={<AppDetails />}>
						<Route path="" element={<Settings />} />
						{/* <Route path="recent-callbacks" element={<RecentCallbacks />} /> */}
					</Route>
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
