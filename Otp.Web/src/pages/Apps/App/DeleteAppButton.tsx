import { useState } from 'react';

import DeleteAppModal from './DeleteAppModal';

interface Props {
	appId: string;
}

const DeleteAppButton = ({ appId }: Props) => {
	const [showDeleteAppButton, setShowDeleteAppButton] = useState(false);
	return (
		<>
			<button
				className="btn btn-error"
				onClick={() => setShowDeleteAppButton(!showDeleteAppButton)}>
				Delete
			</button>
			<DeleteAppModal
				appId={appId}
				showCreateAppModal={showDeleteAppButton}
				onClose={() => {
					setShowDeleteAppButton(!showDeleteAppButton);
				}}></DeleteAppModal>
		</>
	);
};

export default DeleteAppButton;
