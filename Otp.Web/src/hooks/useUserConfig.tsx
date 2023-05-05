import { useAccount } from '@azure/msal-react';
import { useQuery } from '@tanstack/react-query';

import { oauthInstance, request } from '@/api/https';
import { SubscriptionType } from '@/types/types';

export const userConfigKeys = {
	config: ['user', 'config'],
};

export type UserConfig = {
	email: string;
	name: string;
	subscription: SubscriptionType;
};

const fetchUserConfig = (): Promise<UserConfig> => {
	return request(oauthInstance, {
		method: 'GET',
		url: '/me',
	});
};

const useUserConfig = () => {
	const account = useAccount();

	const defaultConfig: UserConfig = {
		subscription: 'Free',
		email: '',
		name: '',
	};

	const query = useQuery(userConfigKeys.config, fetchUserConfig, {
		cacheTime: Infinity,
		staleTime: Infinity,
		enabled: !!account,
	});

	return { userConfigQuery: query, userConfig: query.data ?? defaultConfig };
};

export default useUserConfig;
