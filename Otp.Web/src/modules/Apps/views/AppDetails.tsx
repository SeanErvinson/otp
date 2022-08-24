import { AxiosError } from 'axios';
import { useAtom } from 'jotai';
import { useEffect } from 'react';
import { useQuery } from 'react-query';
import { NavLink, Outlet, useNavigate, useParams } from 'react-router-dom';

import { OtpApi } from '@/api/otpApi';
import LoadingIndicator from '@/components/LoadingIndicator/LoadingIndicator';
import { CustomError } from '@/types/types';

import AppDetailForm from '../components/AppDetailForm';
import { selectedAppAtom } from '../states/SelectedAppAtom';

const AppDetails = () => {
	const { appId } = useParams();
	const navigate = useNavigate();
	const [, setSelectedApp] = useAtom(selectedAppAtom);

	const query = useQuery(['getApp', appId], () => OtpApi.getApp(appId!), {
		enabled: !!appId,
		onError: (error: AxiosError<CustomError>) => {
			if (
				error.response &&
				(error.response.status === 404 || error.response.status === 410)
			) {
				navigate('/404');
				return;
			}
		},
		onSuccess: data => {
			setSelectedApp(data);
		},
	});

	useEffect(() => {
		if (!appId) {
			navigate('/404');
			return;
		}
	}, []);

	return (
		<main id="app" className="h-full mx-auto">
			{query.isLoading && <LoadingIndicator />}
			{query.data && (
				<>
					<div className="flex flex-row justify-between mb-4">
						<AppDetailForm />
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
