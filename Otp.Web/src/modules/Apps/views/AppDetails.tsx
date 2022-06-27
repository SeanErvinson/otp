import { AxiosError } from 'axios';
import { useAtom } from 'jotai';
import { useEffect } from 'react';
import { useQuery } from 'react-query';
import { NavLink, Outlet, useNavigate, useParams } from 'react-router-dom';

import { OtpApi } from '@/api/otpApi';
import { CustomError } from '@/types/types';

import AppDetailForm from '../components/AppDetailForm';
import { appIdAtom } from '../states/AppIdAtom';
import LoadingIndicator from '@/components/LoadingIndicator/LoadingIndicator';

const AppDetails = () => {
	const { appId } = useParams();
	const navigate = useNavigate();
	const [applicationId, setAppId] = useAtom(appIdAtom);

	const query = useQuery(['getApp', applicationId], () => OtpApi.getApp(applicationId), {
		enabled: !!applicationId,
		onError: (error: AxiosError<CustomError>) => {
			if (
				error.response &&
				(error.response.status === 404 || error.response.status === 410)
			) {
				navigate('/404');
				return;
			}
		},
	});

	useEffect(() => {
		if (!appId) {
			navigate('/404');
			return;
		}
		setAppId(appId);
	}, []);

	return (
		<main id="app" className="h-full mx-auto">
			{query.isLoading && <LoadingIndicator />}
			{query.data && (
				<>
					<div className="flex flex-row justify-between mb-4">
						<AppDetailForm data={query.data} />
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
							{query.data?.callbackUrl && (
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

						<Outlet context={query.data} />
					</div>
				</>
			)}
		</main>
	);
};

export default AppDetails;
