import { useAtom } from 'jotai';
import { ChangeEvent } from 'react';

import { Channel } from '@/types/types';
import ChannelEmailIcon from '@/components/misc/ChannelEmailIcon';
import ChannelSmsIcon from '@/components/misc/ChannelSmsIcon';

import { channelFilterAtom } from '../states/ChannelFilterAtom';

const channelMaps = {
	SMS: <ChannelSmsIcon className="h-[24px] fill-primary" />,
	Email: <ChannelEmailIcon className="h-[24px] fill-primary" />,
};

const ChannelColumnFilter = () => {
	const [channels, setChannels] = useAtom(channelFilterAtom);

	const handleOnChange = (event: ChangeEvent<HTMLInputElement>) => {
		const selectedChannel = event.target.value as Channel;
		if (channels.includes(selectedChannel)) {
			setChannels(prev => prev.filter(item => item !== selectedChannel));
		} else {
			setChannels(prev => [...prev, selectedChannel]);
		}
	};

	return (
		<div>
			{Channel.map((item, index) => (
				<div key={index} className="form-control">
					<label className="cursor-pointer flex flex-row items-center gap-2 py-2 px-1">
						<input
							type="checkbox"
							className="checkbox checkbox-secondary"
							defaultChecked={channels.includes(item)}
							value={item}
							onChange={handleOnChange}
						/>
						<span>{channelMaps[item]}</span>
						<span className="label-text">{item}</span>
					</label>
				</div>
			))}
		</div>
	);
};

export default ChannelColumnFilter;
