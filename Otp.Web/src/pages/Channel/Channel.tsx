import { AxiosError } from 'axios';
import React, { FormEvent, useEffect, useState } from 'react';
import { useMutation, useQuery } from 'react-query';
import { useLocation, useNavigate, useParams } from 'react-router-dom';

import { cancelOtp, getOtpRequest, resendOtp, verifyOtp, VerifyOtpResponse } from '@/api/otpApi';
import SpinnerIcon from '@/components/misc/SpinnerIcon';
import { isValidGuid } from '@/utils/stringUtils';

import { OtpInputGroup } from './OtpInputGroup';
import { Loader } from '@/components/Loader';
import { CustomError } from '@/common/types';

const otpInputLength = 6;

const Channel = () => {
	const { requestId } = useParams();
	const navigate = useNavigate();
	const { hash, pathname } = useLocation();
	const [channel] = useState<string>(pathname.split('/')[1]);
	const [key] = useState<string>(hash.substring(1, hash.length - 1));
	const [code, setCode] = useState<string>('');
	const [isValidOtp, setIsValidOtp] = useState(false);
	const query = useQuery(
		['getOtpRequest', requestId, hash],
		() => getOtpRequest(requestId!, key),
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

	const verifyMutation = useMutation<VerifyOtpResponse, CustomError>(
		['verifyApp', requestId, key, code],
		() => verifyOtp(requestId!, key, code),
		{
			onSuccess: response => {
				window.location.replace(response.successUrl);
			},
		},
	);

	const resendMutation = useMutation(['resendOtp', requestId, key], () =>
		resendOtp(requestId!, key),
	);

	const cancelMutation = useMutation(
		['cancelOtp', requestId, key],
		() => cancelOtp(requestId!, key),
		{
			// onSuccess: response => {
			// 	window.location.replace(response.cancelUrl);
			// },
		},
	);

	const handleOnSubmmit = (event: FormEvent) => {
		event.preventDefault();
		verifyMutation.mutate();
	};

	const handleOnResend = (event: React.MouseEvent<HTMLSpanElement>) => {
		event.preventDefault();
		resendMutation.mutate();
	};

	const handleOnCancel = (event: React.MouseEvent<HTMLSpanElement>) => {
		event.preventDefault();
		cancelMutation.mutate();
	};

	useEffect(() => {
		code?.length === otpInputLength ? setIsValidOtp(true) : setIsValidOtp(false);
	}, [code]);

	useEffect(() => {
		if (!requestId || !isValidGuid(requestId) || !hash) {
			navigate('/404', {
				replace: true,
			});
			return;
		}
	}, []);

	return query.isLoading ? (
		<Loader />
	) : (
		<main
			id="channel"
			className="h-full w-full bg-base-300 bg-repeat bg-cover bg-center"
			style={{ backgroundImage: `url("${query.data?.backgroundUrl ?? '/bg.svg'}")` }}>
			<section className="h-full w-full flex flex-col items-center">
				<div className="card shadow-lg bg-base-200 my-auto mx-4">
					<div className="card-body">
						<h2 className="card-title text-center">One-Time Password</h2>
						{query.isLoading && <SpinnerIcon />}
						{query.isSuccess && (
							<>
								<p>A One-Time Password has been sent to {query.data.contact}</p>
								<div className="my-4 mx-auto text-center">
									<form onSubmit={handleOnSubmmit}>
										<div className="flex justify-between">
											<OtpInputGroup onSetValue={setCode} />
										</div>
										{verifyMutation.isError && (
											<span className="label-text-alt text-sm text-error">
												{verifyMutation.error.detail}
											</span>
										)}
										<button
											type="submit"
											disabled={!isValidOtp}
											className={`btn btn-primary px-4 w-full mt-2 ${
												verifyMutation.isLoading && 'loading'
											}`}>
											{!verifyMutation.isLoading
												? 'Verify OTP!'
												: 'Verifying OTP'}
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
