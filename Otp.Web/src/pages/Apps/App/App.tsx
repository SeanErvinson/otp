import { useQuery } from 'react-query';
import { NavLink, Outlet, useParams } from 'react-router-dom';

import { getApp } from '@/api/otpApi';

import DeleteAppButton from './DeleteAppButton';
import Spinner from '@/components/misc/Spinner';

const App = () => {
	const params = useParams();

	const query = useQuery(['getApp', params.appId], () => getApp(params.appId ?? ''));

	return (
		<main id="app" className="h-full w-4/5 mx-auto pt-5">
			{query.isLoading && (
				<div className="flex flex-col gap-3 items-center h-full w-full justify-center">
					<Spinner />
				</div>
			)}
			{query.isSuccess && (
				<>
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
								Settings
							</NavLink>
						</div>

						<Outlet context={query.data} />
					</div>
				</>
			)}
		</main>
	);
};

export default App;
