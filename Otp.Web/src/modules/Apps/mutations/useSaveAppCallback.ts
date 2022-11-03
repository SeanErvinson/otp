import { useMutation, useQueryClient } from 'react-query';

import { oauthInstance, request } from '@/api/https';
import { AppDetail } from '@/types/types';

import appKeys from '../queries/appKeys';

type SaveCallbackRequest = {
	appId: string;
	callbackUrl: string;
	endpointSecret: string;
};

const saveAppCallback = (req: SaveCallbackRequest): Promise<AppDetail> => {
	return request(oauthInstance, {
		method: 'PUT',
		url: `/apps/${req.appId}/callback`,
		data: req,
	});
};

const useSaveAppCallback = () => {
	const queryClient = useQueryClient();
	return useMutation(saveAppCallback, {
		onSuccess(data) {
			queryClient.setQueryData(appKeys.details(data.id), data);
		},
	});
};

export default useSaveAppCallback;
