import { useAtom } from 'jotai';
import { useEffect, useMemo } from 'react';
import { Link } from 'react-router-dom';
import { Column, Cell, useTable, useFilters, Row, IdType } from 'react-table';

import ChannelSmsIcon from '@/components/misc/ChannelSmsIcon';
import ChannelEmailIcon from '@/components/misc/ChannelEmailIcon';
import LogDeliveredIcon from '@/components/misc/LogDeliveredIcon';
import LogErrorIcon from '@/components/misc/LogErrorIcon';
import LogSentIcon from '@/components/misc/LogSentIcon';
import LogSuccessIcon from '@/components/misc/LogSuccessIcon';
import LogWarningIcon from '@/components/misc/LogWarningIcon';
import PaginationButtonGroup from '@/components/PaginationButtonGroup/PaginationButtons';
import { dateTimeFormat } from '@/consts/date';
import useModal from '@/hooks/useModal';
import { Channel, Log, Status } from '@/types/types';
import { toLocalTime } from '@/utils/dayjsUtils';

import { channelFilterAtom } from '../states/ChannelFilterAtom';
import { statusFilterAtom } from '../states/StatusFilterAtom';
import { logTablePaginationAtom } from '../states/LogTablePaginationAtom';
import TimelineModal from './TimelineModal';
import { logAtom } from '../states/LogAtom';

interface Props {
	logs: Log[];
}

const statusMaps = {
	Success: <LogSuccessIcon className="h-[24px] fill-success" />,
	Warning: <LogWarningIcon className="h-[24px] fill-warning" />,
	Error: <LogErrorIcon className="h-[24px] fill-error" />,
	Delivered: <LogDeliveredIcon className="h-[24px] fill-success" />,
	Sent: <LogSentIcon className="h-[24px] fill-success" />,
};

const channelMaps = {
	SMS: <ChannelSmsIcon className="h-[24px] fill-primary" />,
	Email: <ChannelEmailIcon className="h-[24px] fill-primary" />,
};

const LogTable = (props: Props) => {
	const data = useMemo(() => props.logs, []);
	const [pagination] = useAtom(logTablePaginationAtom);
	const [channels] = useAtom(channelFilterAtom);
	const [statuses] = useAtom(statusFilterAtom);
	const [, setLog] = useAtom(logAtom);
	const { toggle, visible } = useModal();

	const channelFilter = (rows: Row<Log>[], _filler: IdType<Log>[], filterValue: Channel[]) => {
		const arr: Row<Log>[] = [];
		rows.forEach(val => {
			if (filterValue.includes(val.original.channel)) arr.push(val);
		});
		return arr;
	};

	const statusFilter = (rows: Row<Log>[], _filler: IdType<Log>[], filterValue: Status[]) => {
		const arr: Row<Log>[] = [];
		rows.forEach(val => {
			if (filterValue.includes(val.original.status)) arr.push(val);
		});
		return arr;
	};

	const handleOnRowSelect = (selectedLog: Log) => {
		setLog(selectedLog);
		toggle();
	};

	useEffect(() => {
		if (channels.length) {
			setFilter('Channel', channels);
		} else {
			setFilter('Channel', undefined);
		}
	}, [channels]);

	useEffect(() => {
		if (statuses.length) {
			setFilter('Event', statuses);
		} else {
			setFilter('Event', undefined);
		}
	}, [statuses]);

	const columns = useMemo<Column<Log>[]>(
		() => [
			{
				Header: 'Event',
				Cell: ({ row }: Cell<Log>) => (
					<div className="flex flex-row items-center gap-1">
						{statusMaps[row.original.status]}
						<span className="text-md">
							{toLocalTime(row.original.eventDate, dateTimeFormat)}
						</span>
					</div>
				),
				filter: statusFilter,
			},
			{
				Header: 'Receiver',
				accessor: row => row.receiver,
			},
			{
				Header: 'App',
				Cell: ({ row }: Cell<Log>) => (
					<Link className="btn btn-ghost btn-xs" to={`/apps/${row.original.id}`}>
						{row.original.app}
					</Link>
				),
			},
			{
				Header: 'Channel',
				accessor: row => row.channel,
				Cell: ({ row }: Cell<Log>) => (
					<span className="flex flex-row items-center gap-1">
						{channelMaps[row.original.channel]}
						{row.original.channel}
					</span>
				),
				filter: channelFilter,
			},
		],
		[],
	);

	const {
		getTableProps,
		getTableBodyProps,
		headerGroups,
		rows,
		prepareRow,
		setFilter,
		canNextPage,
		canPreviousPage,
		nextPage,
		previousPage,
	} = useTable(
		{
			columns,
			data,
			manualPagination: true,
			initialState: { pageIndex: pagination },
		},
		useFilters,
	);

	return (
		<>
			<table className="table w-full bg-base-200" {...getTableProps()}>
				<thead>
					{headerGroups.map(headerGroup => (
						<tr {...headerGroup.getHeaderGroupProps()}>
							{headerGroup.headers.map(column => (
								<th className="bg-base-300" {...column.getHeaderProps()}>
									<div className="flex flex-row justify-between items-center">
										{column.render('Header')}
									</div>
								</th>
							))}
						</tr>
					))}
				</thead>
				<tbody {...getTableBodyProps()}>
					{rows.map(row => {
						prepareRow(row);
						return (
							<tr
								{...row.getRowProps()}
								onClick={() => handleOnRowSelect(row.original)}
								className="cursor-pointer">
								{row.cells.map(cell => (
									<td {...cell.getCellProps()}>{cell.render('Cell')}</td>
								))}
							</tr>
						);
					})}
				</tbody>
			</table>
			<PaginationButtonGroup
				hasNext={canNextPage}
				hasPrevious={canPreviousPage}
				nextPage={nextPage}
				previousPage={previousPage}
			/>
			<TimelineModal showModal={visible} onClose={toggle} />
		</>
	);
};

export default LogTable;
