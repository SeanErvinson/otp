import { useIsAuthenticated } from '@azure/msal-react';
import { ReactChildren, ReactNode } from 'react';
import { NavLink } from 'react-router-dom';

import MenuIcon from '@/components/misc/MenuIcon';

import LogoutButton from './LogoutButton';
import DashboardIcon from '../misc/DashboardIcon';
import AppsIcon from '../misc/AppsIcon';
import BillingIcon from '../misc/BillingIcon';
import LogsIcon from '../misc/LogsIcon';

interface Props {
	children: ReactChildren | ReactNode;
}

const Sidebar = (props: Props) => {
	const isAuthenticated = useIsAuthenticated();

	return (
		<div id="sidebar" className="shadow bg-base-200 h-full drawer drawer-mobile w-full">
			<input id="drawer" type="checkbox" className="drawer-toggle" />
			<div className="drawer-content h-full w-full">
				<div className="w-full navbar lg:hidden">
					<label htmlFor="drawer" className="btn btn-square btn-ghost">
						<MenuIcon className="stroke-none fill-neutral h-10" />
					</label>
				</div>
				<div className="px-4 lg:p-6">{props.children}</div>
			</div>
			<div className="drawer-side">
				<label htmlFor="drawer" className="drawer-overlay"></label>
				<ul className="menu p-3 text-md overflow-y-auto w-64 bg-base-100 text-base-content">
					<li>
						<NavLink to="/">
							<DashboardIcon className="h-6 w-6 fill-current" /> Dashboard
						</NavLink>
					</li>
					<li>
						<NavLink to="/apps">
							<AppsIcon className="h-6 w-6 fill-current" />
							Apps
						</NavLink>
					</li>
					<li>
						<NavLink to="/logs">
							<LogsIcon className="h-6 w-6 fill-current" />
							Logs
						</NavLink>
					</li>

					<li>
						<NavLink to="/billing">
							<BillingIcon className="h-6 w-6 fill-current" />
							Billing
						</NavLink>
					</li>
					<div className="grow"></div>
					<li>
						<a
							href={`${
								new URL('contact', import.meta.env.VITE_OTP_SITE_BASE_URL).href
							}`}
							target="_blank">
							Customer Support
						</a>
					</li>
					<li>
						<a
							href={`${
								new URL('docs/intro', import.meta.env.VITE_OTP_SITE_BASE_URL).href
							}`}
							target="_blank">
							Documentation
						</a>
					</li>
					{isAuthenticated && (
						<li>
							<LogoutButton />
						</li>
					)}
				</ul>
			</div>
		</div>
	);
};

export default Sidebar;
