import RegenerateApiButton from './RegenerateApiButton';

interface Props {
	appId: string;
}

const ApiKeySection = ({ appId }: Props) => {
	return (
		<div className="flex flex-row justify-between">
			<div className="flex flex-col gap-2">
				<label className="text-md font-medium">Last used</label>
				<span className="text-xs">Never</span>
			</div>
			<RegenerateApiButton appId={appId} />
		</div>
	);
};

export default ApiKeySection;
