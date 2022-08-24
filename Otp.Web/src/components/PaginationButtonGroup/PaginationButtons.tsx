interface Props {
	hasPrevious: boolean;
	hasNext: boolean;
	nextPage: () => void;
	previousPage: () => void;
}

const PaginationButtonGroup = (props: Props) => {
	return (
		<div className="btn-group justify-center">
			{props.hasPrevious && (
				<button className="btn btn-outline btn-wide" onClick={props.previousPage}>
					Previous Page
				</button>
			)}
			{props.hasNext && (
				<button className="btn btn-outline btn-wide" onClick={props.nextPage}>
					Next Page
				</button>
			)}
		</div>
	);
};

export default PaginationButtonGroup;
