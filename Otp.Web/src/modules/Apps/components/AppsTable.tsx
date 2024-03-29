import dayjs from 'dayjs';
import relativeTime from 'dayjs/plugin/relativeTime';
import utc from 'dayjs/plugin/utc';
import { useMemo } from 'react';
import { Link } from 'react-router-dom';
import { Column, useTable, useSortBy, Cell } from 'react-table';

import { App } from '@/types/types';
import useGeneratedId from '@/hooks/useGeneratedId';
import SortIcon from '@/components/misc/SortIcon';
import { TagCollection } from '@/components/TagCollection';
import { truncate } from '@/utils/stringUtils';

interface Props {
	apps: App[];
}

dayjs.extend(utc);
dayjs.extend(relativeTime);

const maxDescriptionLength = 32;

const AppsTable = (props: Props) => {
	const generatedId = useGeneratedId();
	const data = useMemo(() => props.apps, []);

	const columns = useMemo<Column<App>[]>(
		() => [
			{
				Header: 'Name',
				accessor: 'name',
				Cell: ({ row }: Cell<App>) => (
					<div className="flex flex-col h-full">
						<span className="font-semibold">{row.original.name}</span>
						<span className="text-md opacity-75">
							{row.original.description
								? truncate(row.original.description, maxDescriptionLength)
								: '---'}
						</span>
					</div>
				),
			},
			{
				Header: 'Created',
				accessor: row => dayjs().to(dayjs.utc(row.createdAt).local()),
			},
			{
				Header: 'Tags',
				Cell: ({ row }: Cell<App>) => (
					<div className="flex flex-row gap-2">
						{row.original.tags && <TagCollection tags={row.original.tags} />}
					</div>
				),
				disableSortBy: true,
			},
			{
				id: generatedId('action'),
				Cell: ({ row }: Cell<App>) => (
					<Link className="btn btn-ghost btn-xs" to={`/apps/${row.original.id}`}>
						Details
					</Link>
				),
			},
		],
		[],
	);

	const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } = useTable(
		{
			columns,
			data,
		},
		useSortBy,
	);

	return (
		<table className="table w-full bg-base-200" {...getTableProps()}>
			<thead>
				{headerGroups.map(headerGroup => (
					<tr {...headerGroup.getHeaderGroupProps()} key={headerGroup.id}>
						{headerGroup.headers.map(column => (
							<th
								className="bg-base-300"
								{...column.getHeaderProps(column.getSortByToggleProps())}
								key={column.id}>
								<div className="flex flex-row justify-between items-center">
									{column.render('Header')}
									{column.isSorted && <SortIcon className="w-4" />}
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
						<tr {...row.getRowProps()} key={row.id}>
							{row.cells.map(cell => (
								<td {...cell.getCellProps()} key={cell.value.id}>
									{cell.render('Cell')}
								</td>
							))}
						</tr>
					);
				})}
			</tbody>
		</table>
	);
};

export default AppsTable;
