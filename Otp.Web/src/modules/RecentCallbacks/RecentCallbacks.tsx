import { nanoid } from 'nanoid';
import { useQuery } from 'react-query';
import { useNavigate, useOutletContext } from 'react-router-dom';

import { OtpApi } from '@/api/otpApi';
import SpinnerIcon from '@/components/misc/SpinnerIcon';
import RoundCheckIcon from '@/components/misc/RoundCheckIcon';
import { AppDetail } from '@/types/types';
import { Fragment, useEffect } from 'react';

const RecentCallbacks = () => {
	const app = useOutletContext<AppDetail | null>();
	const navigate = useNavigate();
	const query = useQuery(['getAppRecentCallbacks', app?.id], () =>
		OtpApi.getAppRecentCallbacks(app?.id),
	);

	useEffect(() => {
		if (!app?.callbackUrl) {
			navigate(`/apps/${app?.id}`);
		}
	}, []);

	return (
		<article id="recent-callbacks">
			<h2 className="text-lg font-semibold mb-2">Recent delivers</h2>
			<span className="text-sm font-light mb-2">Last 30 callbacks</span>
			{query.isLoading && <SpinnerIcon />}
			{query.data &&
				query.data.map((callback, index) => {
					return (
						<Fragment key={nanoid()}>
							<div
								tabIndex={0}
								className="collapse border border-base-300 bg-base-100 rounded-box collapse-arrow">
								<div className="collapse-title text-sm font-medium flex flex-row justify-between">
									<div className="flex flex-row items-center gap-2">
										{(callback.statusCode <= 300 ||
											callback.statusCode <= 200) && <RoundCheckIcon />}
										<span>{callback.requestId}</span>
									</div>
									<span>{callback.createdAt}</span>
								</div>
								<div className="collapse-content">
									<p>
										<span>{callback.responseMessage}</span>
									</p>
								</div>
							</div>
							{index !== query.data!.length - 1 && <div className="divider"></div>}
						</Fragment>
					);
				})}
		</article>
	);
};

export default RecentCallbacks;
