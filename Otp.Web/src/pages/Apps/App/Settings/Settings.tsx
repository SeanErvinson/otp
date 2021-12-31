import { useOutletContext } from 'react-router-dom';

import { GetAppResponse } from '@/api/otpApi';
import Spinner from '@/components/misc/Spinner';
import WarningIcon from '@/components/misc/WarningIcon';

import RegenerateApiButton from './RegenerateApiButton';
import { useState } from 'react';

const Settings = () => {
	const [enableChangeSecret, setEnableChangeSecret] = useState(false);
	const app = useOutletContext<GetAppResponse | null>();

	return (
		<article id="settings">
			{!app && <Spinner />}
			{app && (
				<>
					<section className="my-4">
						<h2 className="text-lg font-semibold mb-4">API Key</h2>
						<hr className="mx-4 my-3" />
						<RegenerateApiButton appId={app!.id} />
						<div>
							<span>Never used</span>
						</div>
					</section>
					<section className="my-4">
						<h2 className="text-lg font-semibold mb-2">Callback</h2>
						<p className="text-xs mb-2">
							Callback URL functions like a notification for your application. Every
							time a user submits an OTP authentication we use this callback URL to
							send relevant information to you.
						</p>
						<hr className="mx-4 my-3" />
						<div className="w-1/2 mb-2">
							<div className="form-control">
								<label className="label">
									<span className="label-text">Callback URL</span>
								</label>
								<input
									type="text"
									defaultValue={app.callbackUrl}
									className="input"
								/>
							</div>

							<div className="form-control">
								<label className="label">
									<span className="label-text">Secret</span>
								</label>
								{!app.callbackUrl || enableChangeSecret ? (
									<>
										<input type="text" className="input" />
										{enableChangeSecret ? (
											<button
												className="btn btn-ghost"
												type="button"
												onClick={() =>
													setEnableChangeSecret(!enableChangeSecret)
												}>
												Cancel
											</button>
										) : (
											<></>
										)}
									</>
								) : (
									<div className="alert alert-warning">
										<div className="flex-1">
											<WarningIcon />
											<p>
												If you've lost or forgotten this secret, you can
												change it, but be aware that any integrations using
												this secret will need to be updated. —
												<span>&nbsp;</span>
												<span
													className="link"
													onClick={() =>
														setEnableChangeSecret(!enableChangeSecret)
													}>
													Change Secret
												</span>
											</p>
										</div>
									</div>
								)}
							</div>
						</div>
					</section>

					<section className="my-4">
						<h2 className="text-lg font-semibold mb-2">Branding</h2>
						<p className="text-xs mb-2">
							Let your customer feel special with a custom background and logo.
						</p>
						<hr className="mx-4 my-3" />
						<div className="flex flex-row gap-8">
							<div className="flex flex-col justify-center gap-4">
								<div className="form-control">
									<label className="label">
										<span className="label-text">Background Image</span>
									</label>
									<input type="file" className="input" />
								</div>
								<div className="form-control">
									<label className="label">
										<span className="label-text">Logo</span>
									</label>
									<input type="file" className="input" />
								</div>
							</div>
							<div className="mockup-window bg-base-300 flex-1">
								<div className="flex justify-center px-4 py-16 bg-base-200 h-full">
									Hello!
								</div>
							</div>
						</div>
					</section>
					<button className="btn btn-success" type="button">
						Update App
					</button>
				</>
			)}
		</article>
	);
};

export default Settings;
