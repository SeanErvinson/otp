import { useAtom } from 'jotai';

import RegenerateApiButton from '../../components/RegenerateApiButton';
import { selectedAppAtom } from '../../states/SelectedAppAtom';

const ApiKeySection = () => {
	const [selectedApp] = useAtom(selectedAppAtom);
	return (
		<div className="flex flex-row justify-between">
			<div className="flex flex-col gap-2">
				<label className="text-md font-medium">Last used</label>
				<span className="text-xs">Never</span>
			</div>
			<RegenerateApiButton appId={selectedApp.id} />
		</div>
	);
};

export default ApiKeySection;
