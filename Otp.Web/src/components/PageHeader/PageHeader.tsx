interface Props {
	title: string;
	rightElement?: React.ReactNode;
}

const PageHeader = (props: Props) => {
	return (
		<div className="flex flex-row justify-between">
			<h1 className="text-4xl font-bold mb-6">{props.title}</h1>
			{props.rightElement}
		</div>
	);
};

export default PageHeader;
