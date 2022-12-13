import { ReactNode } from 'react';

interface Props {
	isLoading: boolean;
	isSuccess: boolean;
	isError: boolean;
	children: ReactNode;
}

const TableContainer = (props: Props) => {
	if (props.isSuccess) return <>{props.children}</>;

	return (
		<article className="border-2 flex-1 flex flex-col gap-3 items-center p-6 mx-8">
			{props.isLoading && <button className="btn btn-lg btn-ghost loading">loading</button>}
			{props.isError && <h1>Error</h1>}
		</article>
	);
};

export default TableContainer;
