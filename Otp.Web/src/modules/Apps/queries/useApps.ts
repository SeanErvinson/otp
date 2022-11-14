import { oauthInstance, request } from '@/api/https';
import { PagedResult, App } from '@/types/types';
import { useQuery } from '@tanstack/react-query';
import appKeys from './appKeys';

const fetchApps = (pageIndex: number): Promise<PagedResult<App>> => {
	const pageSize = 12;
	return request(oauthInstance, {
		method: 'GET',
		url: '/apps',
		params: {
			pageIndex: pageIndex,
			pageSize: pageSize,
		},
	});
};

const useApps = (pageIndex: number) =>
	useQuery(appKeys.lists(pageIndex), () => fetchApps(pageIndex), {
		keepPreviousData: true,
	});

export default useApps;
