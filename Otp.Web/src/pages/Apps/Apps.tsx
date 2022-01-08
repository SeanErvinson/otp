import { useState } from 'react';
import { useQuery } from 'react-query';

import { getApps } from '@/api/otpApi';
import EmptyIcon from '@/components/misc/EmptyIcon';

import AppRowItem from './AppRowItem';
import CreateAppButton from './CreateAppButton';

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
					<table className="table w-full bg-base-300">
						<thead>
							<tr>
								<th className="bg-base-300">Name</th>
								<th className="bg-base-300">Created</th>
								<th className="bg-base-300">Tags</th>
								<th className="bg-base-300"></th>
							</tr>
						</thead>
						<tbody>
							{query.data?.items.map(app => (
								<AppRowItem key={app.id} app={app} />
							))}
						</tbody>
					</table>
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
			<div className="flex flex-row justify-between">
				<h1 className="text-4xl font-bold mb-6">Apps</h1>
				{query.data && query.data.items.length > 0 && <CreateAppButton />}
			</div>

			{defaultComponent}
		</main>
	);
};

export default Apps;
