import { useQuery } from 'react-query';
import { NavLink, Outlet, useParams } from 'react-router-dom';

import { getApp } from '@/api/otpApi';

import DeleteAppButton from './DeleteAppButton';

const App = () => {
	const params = useParams();

	const query = useQuery(['getApp', params.appId], () => getApp(params.appId ?? ''));

	return (
		<main id="app">
			<div className="flex flex-row justify-between">
				<div>
					<h1 className="text-3xl font-bold">{query.data?.name}</h1>
					<h2 className="text-2xl font-semibold">{query.data?.description}</h2>
					<h3>{query.data?.tags}</h3>
				</div>

				<DeleteAppButton appId={params.appId ?? ''} />
			</div>
			<div>
				<div className="tabs">
					<NavLink
						to=""
						end
						className={({ isActive }) =>
							`tab tab-lifted ${isActive ? 'tab-active' : ''}`
						}>
						Home
					</NavLink>
					<NavLink
						to="key"
						className={({ isActive }) =>
							`tab tab-lifted ${isActive ? 'tab-active' : ''}`
						}>
						Key
					</NavLink>
					<NavLink
						to="branding"
						className={({ isActive }) =>
							`tab tab-lifted ${isActive ? 'tab-active' : ''}`
						}>
						Branding
					</NavLink>
				</div>

				<Outlet context={query.data} />
			</div>
		</main>
	);
};

export default App;
