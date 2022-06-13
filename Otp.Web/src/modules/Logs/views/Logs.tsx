import { useResetAtom } from 'jotai/utils';
import { useEffect } from 'react';
import PageHeader from '@/components/PageHeader/PageHeader';

import ChannelColumnFilter from '../components/ChannelColumnFilter';
import FilterContainer from '../components/FilterContainer';
import LogTable from '../components/LogTable';
import StatusColumnFilter from '../components/StatusColumnFilter';
import { channelFilterAtom } from '../states/ChannelFilterAtom';
import { statusFilterAtom } from '../states/StatusFilterAtom';

const Logs = () => {
	const resetChannelFilter = useResetAtom(channelFilterAtom);
	const resetStatusFilter = useResetAtom(statusFilterAtom);

	useEffect(() => {
		return () => {
			resetChannelFilter();
			resetStatusFilter();
		};
	}, []);

	return (
		<main id="usage">
			<PageHeader title="Usage" />
			<article className="flex flex-row gap-8">
				{/* <LastSpanSelect onChange={e => console.log(e)} /> */}
				<section className="flex flex-col gap-4">
					<FilterContainer title="Channel">
						<ChannelColumnFilter />
					</FilterContainer>
					<FilterContainer title="Status">
						<StatusColumnFilter />
					</FilterContainer>
				</section>
				<section className="flex-1">
					<LogTable
						logs={[
							{
								app: 'hello',
								channel: 'Email',
								eventDate: new Date(),
								id: '212bfa42-48aa-4015-81f1-ff77b451721e',
								receiver: 'ervinsonong@gmail.com',
								status: 'Delivered',
							},
							{
								app: 'hello',
								channel: 'SMS',
								eventDate: new Date(),
								id: '212bfa42-48aa-4015-81f1-ff77b451721e',
								receiver: '639182642270',
								status: 'Success',
							},
							{
								app: 'hello',
								channel: 'SMS',
								eventDate: new Date(),
								id: '212bfa42-48aa-4015-81f1-ff77b451721e',
								receiver: '639182642270',
								status: 'Error',
							},
							{
								app: 'hello',
								channel: 'SMS',
								eventDate: new Date(),
								id: '212bfa42-48aa-4015-81f1-ff77b451721e',
								receiver: '639182642270',
								status: 'Sent',
							},
							{
								app: 'hello',
								channel: 'SMS',
								eventDate: new Date(),
								id: '212bfa42-48aa-4015-81f1-ff77b451721e',
								receiver: '639182642270',
								status: 'Warning',
							},
						]}
					/>
				</section>
			</article>
		</main>
	);
};

export default Logs;
