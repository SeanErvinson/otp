import { useState } from 'react';

import RegenerateApiModal from './RegenerateApiModal';

interface Props {
	appId: string;
}

const RegenerateApiButton = ({ appId }: Props) => {
	const [showRegenerateApiButton, setShowRegenerateApiButton] = useState(false);
	return (
		<>
			<button
				className="btn btn-sm md:btn-md btn-error"
				onClick={() => setShowRegenerateApiButton(!showRegenerateApiButton)}>
				Regenerate Key
			</button>
			<RegenerateApiModal
				appId={appId}
				showCreateAppModal={showRegenerateApiButton}
				onClose={() => {
					setShowRegenerateApiButton(!showRegenerateApiButton);
				}}></RegenerateApiModal>
		</>
	);
};

export default RegenerateApiButton;
