import { AxiosError } from 'axios';
import React, { FormEvent, useEffect, useState } from 'react';
import { useMutation, useQuery } from 'react-query';
import { useLocation, useNavigate, useParams } from 'react-router-dom';

import { cancelOtp, getOtpRequest, resendOtp, verifyOtp } from '@/api/otpApi';
import Spinner from '@/components/misc/Spinner';
import { isValidGuid } from '@/utils/stringUtils';

import { OtpInputGroup } from './OtpInputGroup';

const otpInputLength = 6;

const Channel = () => {
	const { requestId } = useParams();
	const navigate = useNavigate();
	const { hash, pathname } = useLocation();
	const [channel] = useState<string>(pathname.split('/')[1]);
	const [secret] = useState<string>(hash.substring(1, hash.length - 1));
	const [value, setValue] = useState<string>('');
	const [isValidOtp, setIsValidOtp] = useState(false);
	const query = useQuery(
		['getOtpRequest', requestId, hash],
		() => getOtpRequest(requestId!, secret),
		{
			keepPreviousData: true,
			enabled: !!requestId,
			refetchIntervalInBackground: false,
			refetchOnMount: false,
			retry: false,
			retryOnMount: false,
			refetchOnWindowFocus: false,
			staleTime: Infinity,
			onError: (error: AxiosError) => {
				if (
					error.response &&
					(error.response.status === 404 || error.response.status === 410)
				) {
					navigate('/404', {
						replace: true,
					});
					return;
				}
			},
		},
	);
	const verifyMutation = useMutation(verifyOtp);
	const resendMutation = useMutation(resendOtp);
	const cancelMutation = useMutation(cancelOtp);

	const handleOnSubmmit = (event: FormEvent) => {
		event.preventDefault();
		verifyMutation.mutate(
			{
				id: requestId!,
				secret: secret,
				code: value,
			},
			{
				onSuccess: response => {
					window.location.replace(response.successUrl);
				},
			},
		);
	};

	const handleOnResend = (event: React.MouseEvent<HTMLSpanElement>) => {
		event.preventDefault();

		resendMutation.mutate({
			id: requestId!,
			secret: secret,
		});
	};

	const handleOnCancel = (event: React.MouseEvent<HTMLSpanElement>) => {
		event.preventDefault();

		cancelMutation.mutate(
			{
				id: requestId!,
				secret: secret,
			},
			{
				// onSuccess: response => {
				// 	window.location.replace(response.cancelUrl);
				// },
			},
		);
	};

	useEffect(() => {
		value?.length === otpInputLength ? setIsValidOtp(true) : setIsValidOtp(false);
	}, [value]);

	useEffect(() => {
		if (!requestId || !isValidGuid(requestId) || !hash) {
			// navigate('/404', {
			// 	replace: true,
			// });
			// return;
		}
	}, []);

	return (
		<main id="channel" className="h-full w-full bg-base-300">
			<section className="h-full w-full flex flex-col items-center">
				<div className="card shadow-lg bg-base-200 my-auto mx-4">
					<div className="card-body">
						<h2 className="card-title text-center">One-Time Passowrd</h2>
						{query.isLoading && <Spinner size={'lg'} height={0} />}
						{query.isSuccess && (
							<>
								<p>A One-Time Password has been sent to {query.data.contact}</p>
								<div className="my-4 mx-auto text-center">
									<form onSubmit={handleOnSubmmit}>
										<div className="flex justify-between">
											<OtpInputGroup onSetValue={setValue} />
										</div>
										{verifyMutation.isError && (
											<span className="label-text-alt text-sm text-error">
												{
													(verifyMutation.error as AxiosError).response
														?.data.detail
												}
											</span>
										)}
										<button
											type="submit"
											disabled={!isValidOtp}
											className="btn btn-primary px-4 w-full mt-2">
											Verify OTP!
										</button>
										<button
											type="button"
											onClick={handleOnCancel}
											className="btn btn-ghost px-4 w-full mt-2">
											Cancel
										</button>
									</form>
									<p className="text-xs">
										Did not receive an OTP?{' '}
										<span
											onClick={handleOnResend}
											className="btn btn-link text-xs px-0">
											Resend OTP after X
										</span>
									</p>
								</div>
							</>
						)}
					</div>
				</div>
			</section>
		</main>
	);
};

export default React.memo(Channel);
