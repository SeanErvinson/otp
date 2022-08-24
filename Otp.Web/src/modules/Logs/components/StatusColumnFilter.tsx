import { useAtom } from 'jotai';
import { ChangeEvent } from 'react';

import LogDeliveredIcon from '@/components/misc/LogDeliveredIcon';
import LogErrorIcon from '@/components/misc/LogErrorIcon';
import LogSentIcon from '@/components/misc/LogSentIcon';
import LogSuccessIcon from '@/components/misc/LogSuccessIcon';
import LogWarningIcon from '@/components/misc/LogWarningIcon';
import { Status } from '@/types/types';

import { statusFilterAtom } from '../states/StatusFilterAtom';

const statusMaps = {
	Success: <LogSuccessIcon className="h-[24px] fill-success" />,
	Warning: <LogWarningIcon className="h-[24px] fill-warning" />,
	Error: <LogErrorIcon className="h-[24px] fill-error" />,
	Delivered: <LogDeliveredIcon className="h-[24px] fill-success" />,
	Sent: <LogSentIcon className="h-[24px] fill-success" />,
};

const StatusColumnFilter = () => {
	const [statuses, setStatuses] = useAtom(statusFilterAtom);

	const handleOnChange = (event: ChangeEvent<HTMLInputElement>) => {
		const selectedStatus = event.target.value as Status;
		if (statuses.includes(selectedStatus)) {
			setStatuses(prev => prev.filter(item => item !== selectedStatus));
		} else {
			setStatuses(prev => [...prev, selectedStatus]);
		}
	};

	return (
		<div>
			{Object.values(Status).map((item, index) => (
				<div key={index} className="form-control">
					<label className="cursor-pointer flex flex-row items-center gap-2 py-2 px-1">
						<input
							type="checkbox"
							className="checkbox checkbox-secondary"
							defaultChecked={statuses.includes(item)}
							value={item}
							onChange={handleOnChange}
						/>
						<span>{statusMaps[item]}</span>
						<span className="label-text capitalize">{item}</span>
					</label>
				</div>
			))}
		</div>
	);
};

export default StatusColumnFilter;
