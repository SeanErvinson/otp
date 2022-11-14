import { useMutation, useQueryClient } from '@tanstack/react-query';

import { oauthInstance, request } from '@/api/https';
import { AppDetail } from '@/types/types';

import appKeys from '../queries/appKeys';

type SaveDescriptorRequest = {
	appId: string;
	name: string;
	description?: string;
	tags: string[];
};

const saveAppDescriptors = (req: SaveDescriptorRequest): Promise<AppDetail> => {
	return request(oauthInstance, {
		method: 'PUT',
		url: `/apps/${req.appId}/descriptor`,
		data: req,
	});
};

const useSaveAppDescriptors = () => {
	const queryClient = useQueryClient();
	return useMutation(saveAppDescriptors, {
		onSuccess(data) {
			queryClient.setQueryData(appKeys.details(data.id), data);
		},
	});
};

export default useSaveAppDescriptors;
