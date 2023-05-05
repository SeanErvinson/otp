import { useResetAtom } from 'jotai/utils';
import { useEffect, useState } from 'react';
import { useQuery } from '@tanstack/react-query';

import { OtpApi } from '@/api/otpApi';
import PageHeader from '@/components/PageHeader/PageHeader';
import TableContainer from '@/components/Layouts/TableContainer';
import MainContainer from '@/components/Layouts/MainContainer';
import EmptyIcon from '@/components/misc/EmptyIcon';
import { CursorResult, Log } from '@/types/types';

import ChannelColumnFilter from '../components/ChannelColumnFilter';
import FilterContainer from '../components/FilterContainer';
import LogTable from '../components/LogTable';
import StatusColumnFilter from '../components/StatusColumnFilter';
import { channelFilterAtom } from '../states/ChannelFilterAtom';
import { statusFilterAtom } from '../states/StatusFilterAtom';

const Hello = ({ queryData }: { queryData: CursorResult<Log> }) => {
	const [beforeCursor, setBeforeCursor] = useState<string | null>();
	const [afterCursor, setAfterCursor] = useState<string | null>();
	const [cursor, setCursor] = useState<string | null>();

	const { data, error, isLoading, isError, isFetching, isPreviousData, refetch } = useQuery(
		['logs', cursor],
		() => OtpApi.getLogs(beforeCursor, afterCursor),
		{
			refetchOnMount: false,
			refetchOnWindowFocus: false,
		},
	);

	useEffect(() => {
		if (data) {
			setAfterCursor(data.after);
			setBeforeCursor(data.before);
		}
	}, [data]);

	return (
		<div>
			{beforeCursor && (
				<button onClick={() => setCursor(beforeCursor)} disabled={isFetching}>
					{isFetching ? 'Loading more...' : 'Nothing more to load'}
				</button>
			)}
			{afterCursor && (
				<button onClick={() => setCursor(afterCursor)} disabled={isFetching}>
					{isFetching ? 'Loading more...' : 'Nothing more to load'}
				</button>
			)}
		</div>
	);
};

const Logs = () => {
	const resetChannelFilter = useResetAtom(channelFilterAtom);
	const resetStatusFilter = useResetAtom(statusFilterAtom);

	const { data, isLoading, isError, isSuccess } = useQuery(
		['initialLogs'],
		() => OtpApi.getLogs(),
		{
			staleTime: Infinity,
			cacheTime: Infinity,
		},
	);

	useEffect(() => {
		return () => {
			resetChannelFilter();
			resetStatusFilter();
		};
	}, []);

	return (
		<MainContainer id="usage">
			<PageHeader title="Logs" />
			<article className="flex flex-row gap-8">
				<section className="flex flex-col gap-4">
					<FilterContainer title="Channel">
						<ChannelColumnFilter />
					</FilterContainer>
					<FilterContainer title="Status">
						<StatusColumnFilter />
					</FilterContainer>
				</section>
				<TableContainer isError={isError} isLoading={isLoading} isSuccess={isSuccess}>
					{data ? (
						<section className="flex-1">
							{data && <LogTable logs={data.items} />}
						</section>
					) : (
						<>
							<EmptyIcon height={200} />
							<h3 className="text-lg font-semibold">No logs</h3>
						</>
					)}
				</TableContainer>
			</article>
		</MainContainer>
	);
};

export default Logs;
