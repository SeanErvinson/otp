import { FunctionComponent, ReactChild, ReactChildren } from 'react';
import { NavLink } from 'react-router-dom';

interface Props {
	children: ReactChildren | ReactChild;
}

const Sidebar: FunctionComponent<Props> = props => {
	return (
		<div id="sidebar" className="shadow bg-base-200 drawer drawer-mobile h-full">
			<input id="my-drawer-2" type="checkbox" className="drawer-toggle" />{' '}
			{/* TODO: Rename id and add a hamburger when in mobile */}
			<div className="drawer-content h-full w-full p-8">{props.children}</div>
			<div className="drawer-side">
				<label htmlFor="my-drawer-2" className="drawer-overlay"></label>
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
