import { oauthInstance, request } from '@/api/https';
import { AppDetail } from '@/types/types';
import { useQuery } from 'react-query';
import appKeys from './appKeys';

const fetchApp = (id: string | undefined): Promise<AppDetail> => {
	return typeof id === 'undefined'
		? Promise.reject(new Error('Invalid id'))
		: request(oauthInstance, {
				method: 'GET',
				url: `/apps/${id}`,
		  });
};

const useAppDetails = (appId: string | undefined) => {
	return useQuery(appKeys.details(appId!), () => fetchApp(appId), {
		enabled: Boolean(appId),
	});
};

export default useAppDetails;
