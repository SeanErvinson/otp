import { oauthInstance, request } from '@/api/https';
import { OtpRequest } from '@/types/types';
import { useQuery } from '@tanstack/react-query';

const fetchOtpDetail = (id: string | undefined): Promise<OtpRequest> => {
	return typeof id === 'undefined'
		? Promise.reject(new Error('Invalid id'))
		: request(oauthInstance, {
				method: 'GET',
				url: `/otp/${id}`,
		  });
};

const refreshInterval = 5000;

const useOtpDetails = (otpId: string | undefined) => {
	return useQuery(['otps', 'details', otpId], () => fetchOtpDetail(otpId), {
		enabled: Boolean(otpId),
		refetchIntervalInBackground: false,
		refetchInterval: refreshInterval,
	});
};

export default useOtpDetails;
