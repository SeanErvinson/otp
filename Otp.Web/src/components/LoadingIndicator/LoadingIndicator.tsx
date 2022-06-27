import SpinnerIcon from '@/components/misc/SpinnerIcon';

const LoadingIndicator = () => {
	return (
		<div className="flex flex-col gap-3 items-center h-full w-full justify-center">
			<SpinnerIcon />
		</div>
	);
};

export default LoadingIndicator;
