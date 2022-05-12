interface Props {
	title: string;
	value: string;
	description?: string | null;
}

const SingleStat = (props: Props) => {
	return (
		<div className="shadow stats">
			<div className="stat">
				<div className="stat-title">{props.title}</div>
				<div className="stat-value">{props.value}</div>
				{props.description !== null && <div className="stat-desc">{props.description}</div>}
			</div>
		</div>
	);
};

export default SingleStat;
