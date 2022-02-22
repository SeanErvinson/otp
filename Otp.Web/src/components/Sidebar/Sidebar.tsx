import { FunctionComponent, ReactChildren, ReactNode } from 'react';
import { NavLink } from 'react-router-dom';

import MenuIcon from '@/components/misc/MenuIcon';

interface Props {
	children: ReactChildren | ReactNode;
}

const Sidebar: FunctionComponent<Props> = props => {
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
				<ul className="menu p-3 text-md overflow-y-auto w-56 bg-base-100 text-base-content">
					<li>
						<NavLink to="/">Home</NavLink>
					</li>
					<li>
						<NavLink to="/apps">Apps</NavLink>
					</li>
					<li>
						<NavLink to="/usage">Usage</NavLink>
					</li>
					<li>
						<NavLink to="/billing">Billing</NavLink>
					</li>
					<div className="grow"></div>
					<li>
						<a>Customer Support</a>
					</li>
					<li>
						<a>Documentation</a>
					</li>
				</ul>
			</div>
		</div>
	);
};

export default Sidebar;
