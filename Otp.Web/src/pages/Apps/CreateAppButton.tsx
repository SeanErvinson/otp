import { useState } from 'react';

import CreateAppModal from './CreateAppModal';

const CreateAppButton = () => {
	const [showCreateAppModal, setShowCreateAppModal] = useState(false);
	return (
		<>
			<button
				className="btn btn-accent"
				onClick={() => setShowCreateAppModal(!showCreateAppModal)}>
				Create App
			</button>
			<CreateAppModal
				showCreateAppModal={showCreateAppModal}
				onClose={() => {
					setShowCreateAppModal(!showCreateAppModal);
				}}></CreateAppModal>
		</>
	);
};

export default CreateAppButton;
