interface Props {
	size?: string;
	height?: number;
}

const Spinner = ({ size = 'lg', height = 32 }: Props) => {
	return (
		<button
			className={`btn btn-${size} btn-ghost btn-circle self-center loading h-${height} w-${height}`}></button>
	);
};

export default Spinner;
