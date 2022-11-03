import RegenerateApiButton from '../../components/RegenerateApiButton';

const ApiKeySection = ({ appId }: { appId: string }) => {
	return (
		<div className="flex flex-row justify-between">
			<div className="flex flex-col gap-2">
				<label className="text-lg font-semibold">Last used</label>
				<span className="text-xs">Never</span>
			</div>
			<RegenerateApiButton appId={appId} />
		</div>
	);
};

export default ApiKeySection;
