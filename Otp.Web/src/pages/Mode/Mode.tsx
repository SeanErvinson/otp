import { getOtpRequest, verifyOtp } from '@/api/otpApi';
import Spinner from '@/components/misc/Spinner';
import { isValidGuid } from '@/utils/stringUtils';
import { AxiosError } from 'axios';
import React, { FormEvent } from 'react';
import { useEffect, useState } from 'react';
import { useMutation, useQuery } from 'react-query';
import { useLocation, useNavigate, useParams } from 'react-router-dom';

import { OtpInputGroup } from './OtpInputGroup';

const otpInputLength = 6;

const Mode = () => {
	const { requestId } = useParams();
	const navigate = useNavigate();
	const { hash, pathname } = useLocation();
	const [mode, setMode] = useState<string>();
	const [value, setValue] = useState<string>('');
	const [isValidOtp, setIsValidOtp] = useState(false);
	const query = useQuery(
		['getApps', requestId, hash],
		() => getOtpRequest(requestId!, hash.substring(1, hash.length - 1)),
		{
			keepPreviousData: true,
			enabled: !!requestId,
			refetchIntervalInBackground: false,
			refetchOnMount: false,
			retry: false,
			retryOnMount: false,
			staleTime: Infinity,
			onError: (error: AxiosError) => {
				if (error.code && error.code === '404') {
					navigate('/404');
					return;
				}
			},
		},
	);
	const mutation = useMutation(verifyOtp);

	const onSubmit = (event: FormEvent) => {
		event.preventDefault();
		mutation.mutate(
			{
				id: requestId!,
				secret: hash.substring(1, hash.length - 1),
				code: value,
			},
			{
				onSuccess: response => {
					window.location.replace(response.successUrl);
				},
			},
		);
	};

	useEffect(() => {
		value?.length === otpInputLength ? setIsValidOtp(true) : setIsValidOtp(false);
	}, [value]);

	useEffect(() => {
		if (!requestId || !isValidGuid(requestId) || !hash) {
			navigate('/404');
			return;
		}

		setMode(pathname.split('/')[1]);
	}, []);

	return (
		<main id="mode" className="h-full w-full bg-base-300">
			<section className="h-full w-full flex flex-col items-center">
				<div className="card shadow-lg bg-base-200 my-auto mx-4">
					<div className="card-body">
						<h2 className="card-title text-center">One-Time Passowrd</h2>
						{query.isLoading && <Spinner size={'lg'} height={0} />}
						{query.isSuccess && (
							<>
								<p>A One-Time Password has been sent to {query.data.contact}</p>
								<div className="my-4 mx-auto text-center">
									<form onSubmit={onSubmit}>
										<div className="flex justify-between">
											<OtpInputGroup onSetValue={setValue} />
										</div>
										<button
											type="submit"
											disabled={!isValidOtp}
											className="btn btn-primary mx-auto block w-full  p-2 mt-2">
											Verify OTP!
										</button>
									</form>
									<p className="text-xs">
										Did not receive an OTP?{' '}
										<span className="btn btn-link text-xs px-0">
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
//{mode === OtpMode.SMS && <h1>SMS</h1>}
enum OtpMode {
	SMS = 'sms',
	Email = 'email',
}

export default React.memo(Mode);
