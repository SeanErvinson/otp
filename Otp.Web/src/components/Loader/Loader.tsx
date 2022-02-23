import LoaderIcon from '@/components/misc/LoaderIcon';

const Loader = () => {
	return (
		<div className="flex h-screen">
			<div className="m-auto">
				<LoaderIcon className="fill-bg-base-300 stroke-bg-base-300 h-28" />
			</div>
		</div>
	);
};

export default Loader;
