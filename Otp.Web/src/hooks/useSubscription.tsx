import { SubscriptionContext } from '@/contexts/SubscriptionContext';
import { SubscriptionType } from '@/types/types';
import { useContext } from 'react';

const useSubscription = (subscription: SubscriptionType) => {
	const { isAllowedTo } = useContext(SubscriptionContext);
	return isAllowedTo(subscription);
};

export default useSubscription;
