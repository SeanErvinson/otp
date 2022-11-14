import { useMutation } from '@tanstack/react-query';

import { oauthInstance, request } from '@/api/https';

export type RegenerateApiKeyResponse = {
	apiKey: string;
};

const regenerateAppApiKey = (appId: string | undefined): Promise<RegenerateApiKeyResponse> => {
	return request(oauthInstance, {
		method: 'POST',
		url: `/apps/${appId}/regenerate-api-key`,
	});
};

const useRegenerateAppApiKey = () => {
	return useMutation(regenerateAppApiKey);
};

export default useRegenerateAppApiKey;
