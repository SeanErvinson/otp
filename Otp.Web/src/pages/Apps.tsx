import { useState } from 'react';
import { useQuery } from 'react-query';

import { getApps } from '@/api/otpApi';
import EmptyIcon from '@/components/misc/EmptyIcon';

import CreateAppButton from '@/modules/App/components/CreateAppButton';
import AppTable from '@/features/App/components/AppTable';
import PageHeader from '@/components/PageHeader/PageHeader';

const Apps = () => {
	const [page, setPage] = useState(1);
	const query = useQuery(['getApps', page], () => getApps(page), { keepPreviousData: true });

	const incrementPage = () => {
		setPage(prev => prev + 1);
	};

	const decrementPage = () => {
		setPage(prev => prev - 1);
	};

	let defaultComponent = (
		<article className="border-2 flex flex-col gap-3 items-center p-6 mx-8">
			{query.isLoading && <button className="btn btn-lg btn-ghost loading">loading</button>}
			{query.isError && <h1>Error</h1>}
			{query.isSuccess && (
				<>
					<EmptyIcon height={200} />
					<h3 className="text-lg font-semibold">
						hmm...seems like you don't have an app.
					</h3>
					<CreateAppButton />
				</>
			)}
		</article>
	);

	if (query.isSuccess && query.data && query.data.items.length > 0) {
		defaultComponent = (
			<article className="flex flex-col gap-6">
				<div className="overflow-x-auto">
					<AppTable apps={query.data.items} />
				</div>
				<div className="btn-group justify-center">
					{query.data.hasPreviousPage && (
						<button className="btn btn-outline btn-wide" onClick={decrementPage}>
							Previous Page
						</button>
					)}
					{query.data.hasNextPage && (
						<button className="btn btn-outline btn-wide" onClick={incrementPage}>
							Next Page
						</button>
					)}
				</div>
			</article>
		);
	}

	return (
		<main id="apps">
			<PageHeader
				title="Apps"
				rightElement={query.data && query.data.items.length > 0 && <CreateAppButton />}
			/>

			{defaultComponent}
		</main>
	);
};

export default Apps;
