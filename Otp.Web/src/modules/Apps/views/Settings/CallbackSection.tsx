import { useQueryClient } from '@tanstack/react-query';

import { AppDetail } from '@/types/types';

import appKeys from '../../queries/appKeys';
import CallbackForm from '../../components/CallbackForm/CallbackForm';

const CallbackSection = ({ appId }: { appId: string }) => {
	const queryClient = useQueryClient();
	const queryData = queryClient.getQueryData<AppDetail>(appKeys.details(appId))!;
	const selectedApp = queryData ?? ({} as AppDetail);

	return (
		<div className="max-w-lg mb-2">
			<CallbackForm appDetail={selectedApp} />
		</div>
	);
};

export default CallbackSection;
