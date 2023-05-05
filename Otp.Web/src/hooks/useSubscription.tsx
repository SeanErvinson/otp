import { useContext } from 'react';

import { SubscriptionContext } from '@/contexts/SubscriptionContext';
import { SubscriptionType } from '@/types/types';

const useSubscription = (subscription: SubscriptionType) => {
	const { isAllowedTo } = useContext(SubscriptionContext);
	return isAllowedTo(subscription);
};

export default useSubscription;
