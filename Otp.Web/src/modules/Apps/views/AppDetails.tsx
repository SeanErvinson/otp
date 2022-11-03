import axios from 'axios';
import { useEffect } from 'react';
import { NavLink, Outlet, useNavigate, useParams } from 'react-router-dom';

import { notFoundRoute } from '@/consts/endpoints';
import LoadingIndicator from '@/components/LoadingIndicator/LoadingIndicator';
import { ProblemDetails } from '@/types/types';

import AppDetailForm from '../components/AppDetailForm/AppDetailForm';
import useAppDetails from '../queries/useAppDetails';
import MainContainer from '@/components/MainContainer/MainContainer';

const AppDetails = () => {
	const { appId } = useParams();
	const navigate = useNavigate();
	const appQuery = useAppDetails(appId);

	useEffect(() => {
		if (!appQuery.error) return;
		if (axios.isAxiosError<ProblemDetails>(appQuery.error)) {
			if (
				appQuery.error.response &&
				(appQuery.error.response.status === 404 || appQuery.error.response.status === 410)
			) {
				navigate(notFoundRoute);
				return;
			}
		} else {
			navigate(notFoundRoute);
			return;
		}
	}, [appQuery.error]);

	useEffect(() => {
		if (!appId) {
			navigate(notFoundRoute);
			return;
		}
	}, []);

	return (
		<MainContainer id="app">
			{appQuery.isLoading && <LoadingIndicator />}
			{appQuery.data && (
				<>
					<div className="flex flex-row justify-between mb-4">
						<AppDetailForm id={appQuery.data.id} />
					</div>

					<div>
						<div className="tabs">
							<NavLink
								to=""
								end
								className={({ isActive }) =>
									`tab tab-lifted ${isActive && 'tab-active'}`
								}>
								Settings
							</NavLink>
							{appQuery.data?.callbackUrl && (
								<NavLink
									to="recent-callbacks"
									end
									className={({ isActive }) =>
										`tab tab-lifted ${isActive && 'tab-active'}`
									}>
									Recent Callbacks
								</NavLink>
							)}
						</div>

						<Outlet context={appQuery.data} />
					</div>
				</>
			)}
		</MainContainer>
	);
};

export default AppDetails;
