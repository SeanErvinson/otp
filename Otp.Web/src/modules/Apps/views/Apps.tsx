import { useState } from 'react';

import EmptyIcon from '@/components/misc/EmptyIcon';
import MainContainer from '@/components/Layouts/MainContainer';
import PageHeader from '@/components/PageHeader/PageHeader';
import TableContainer from '@/components/Layouts/TableContainer';
import CreateAppButton from '@/modules/Apps/components/CreateAppButton';

import AppsTable from '../components/AppsTable';
import useApps from '../queries/useApps';

const Apps = () => {
	const [page, setPage] = useState(1);
	const appsQuery = useApps(page);

	return (
		<MainContainer id="apps">
			<PageHeader
				title="Apps"
				rightElement={
					appsQuery.data && appsQuery.data.items.length > 0 && <CreateAppButton />
				}
			/>

			<TableContainer
				isError={appsQuery.isError}
				isLoading={appsQuery.isLoading}
				isSuccess={appsQuery.isSuccess}>
				{appsQuery.data ? (
					<article className="flex flex-col gap-6">
						<div className="overflow-x-auto">
							<AppsTable apps={appsQuery.data.items} />
						</div>
						<div className="btn-group justify-center">
							{appsQuery.data.hasPreviousPage && (
								<button
									className="btn btn-outline btn-wide"
									onClick={() => setPage(prev => prev - 1)}>
									Previous Page
								</button>
							)}
							{appsQuery.data.hasNextPage && (
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
		</MainContainer>
	);
};

export default Apps;
