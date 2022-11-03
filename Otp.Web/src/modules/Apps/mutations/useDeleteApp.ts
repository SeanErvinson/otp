import { oauthInstance, request } from '@/api/https';
import { useMutation, useQueryClient } from 'react-query';
import appKeys from '../queries/appKeys';

const deleteApp = ({ appId }: { appId: string }): Promise<void> => {
	return request(oauthInstance, {
		method: 'DELETE',
		url: `/apps/${appId}`,
	});
};

const useDeleteApp = () => {
	const queryClient = useQueryClient();

	return useMutation(deleteApp, {
		onSuccess: () => {
			queryClient.invalidateQueries(appKeys.all);
		},
	});
};

export default useDeleteApp;
