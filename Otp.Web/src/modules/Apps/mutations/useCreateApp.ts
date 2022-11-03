import { oauthInstance, request } from '@/api/https';
import { useMutation, useQueryClient } from 'react-query';
import appKeys from '../queries/appKeys';

type CreateAppRequest = {
	name: string;
	tags: string[];
	description: string;
};

type CreateAppResponse = {
	id: string;
	apiKey: string;
};

const createApp = (req: CreateAppRequest): Promise<CreateAppResponse> => {
	return request(oauthInstance, {
		method: 'POST',
		url: '/apps',
		data: req,
	});
};

const useCreateApp = () => {
	const queryClient = useQueryClient();

	return useMutation(createApp, {
		onSuccess: () => {
			queryClient.invalidateQueries(appKeys.all);
		},
	});
};

export default useCreateApp;
