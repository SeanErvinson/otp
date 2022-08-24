import { useState } from 'react';
import { useQuery } from 'react-query';

import { OtpApi } from '@/api/otpApi';
import EmptyIcon from '@/components/misc/EmptyIcon';
import PageHeader from '@/components/PageHeader/PageHeader';
import CreateAppButton from '@/modules/Apps/components/CreateAppButton';

import AppsTable from '../components/AppsTable';
import TableContainer from '@/components/TableContainer/TableContainer';

const Apps = () => {
	const [page, setPage] = useState(1);
	const { isSuccess, data, isLoading, isError } = useQuery(
		['getApps', page],
		() => OtpApi.getApps(page),
		{
			keepPreviousData: true,
		},
	);

	return (
		<main id="apps">
			<PageHeader
				title="Apps"
				rightElement={data && data.items.length > 0 && <CreateAppButton />}
			/>

			<TableContainer isError={isError} isLoading={isLoading} isSuccess={isSuccess}>
				{data ? (
					<article className="flex flex-col gap-6">
						<div className="overflow-x-auto">
							<AppsTable apps={data.items} />
						</div>
						<div className="btn-group justify-center">
							{data.hasPreviousPage && (
								<button
									className="btn btn-outline btn-wide"
									onClick={() => setPage(prev => prev - 1)}>
									Previous Page
								</button>
							)}
							{data.hasNextPage && (
								<button
									className="btn btn-outline btn-wide"
									onClick={() => setPage(prev => prev + 1)}>
									Next Page
								</button>
							)}
						</div>
					</article>
				) : (
					<>
						<EmptyIcon height={200} />
						<h3 className="text-lg font-semibold">
							hmm...seems like you don't have an app.
						</h3>
						<CreateAppButton />
					</>
				)}
			</TableContainer>
		</main>
	);
};

export default Apps;
