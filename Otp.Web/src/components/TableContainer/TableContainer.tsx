import { ReactChild, ReactChildren } from 'react';

interface Props {
	isLoading: boolean;
	isSuccess: boolean;
	isError: boolean;
	children: ReactChild | ReactChildren;
}

const TableContainer = (props: Props) => {
	return (
		<article className="border-2 flex-1 flex flex-col gap-3 items-center p-6 mx-8">
			{props.isLoading && <button className="btn btn-lg btn-ghost loading">loading</button>}
			{props.isError && <h1>Error</h1>}
			{props.isSuccess && <>{props.children}</>}
		</article>
	);
};

export default TableContainer;
