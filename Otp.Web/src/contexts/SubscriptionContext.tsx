import useUserConfig from '@/hooks/useUserConfig';
import { SubscriptionType } from '@/types/types';
import { createContext, ReactNode } from 'react';

type SubscriptionContextType = {
	isAllowedTo: (subscription: SubscriptionType) => boolean;
};

const defaultBehaviour: SubscriptionContextType = {
	isAllowedTo: () => false,
};

const SubscriptionContext = createContext(defaultBehaviour);

type Props = {
	children: ReactNode;
};

const SubscriptionProvider = (props: Props) => {
	const { userConfig } = useUserConfig();

	const isAllowedTo = (subscription: SubscriptionType) =>
		userConfig.subscription === subscription;

	return (
		<SubscriptionContext.Provider value={{ isAllowedTo }}>
			{props.children}
		</SubscriptionContext.Provider>
	);
};

export { SubscriptionContext, SubscriptionProvider };
