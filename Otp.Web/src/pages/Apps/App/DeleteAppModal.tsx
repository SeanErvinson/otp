import 'animate.css';
import { useState } from 'react';
import { useMutation } from 'react-query';

import { deleteApp } from '@/api/otpApi';
import { useNavigate } from 'react-router-dom';

interface Props {
	appId: string;
	showCreateAppModal: boolean;
	onClose: () => void;
}

const DeleteAppModal = (props: Props) => {
	const [isLoading, setIsLoading] = useState(false);
	const navigate = useNavigate();

	const mutation = useMutation(deleteApp, {
		onMutate: () => {
			setIsLoading(true);
		},
		onSuccess: () => {
			setIsLoading(false);
			navigate('/apps');
		},
	});

	const onClick = (id: string) => {
		mutation.mutate(id);
	};

	const onClose = () => {
		setIsLoading(false);
		props.onClose();
	};

	return (
		<div
			id="createAppModal"
			className={`modal ${props.showCreateAppModal ? 'modal-open' : ''}`}>
			<div className="modal-box">
				<h3 className="text-xl font-semibold">Are you sure you want to delete this app?</h3>
				<br />
				<p className="text-md">
					Any applications or scripts using this app will no longer be able to access the
					API. You cannot undo this action.
				</p>
				<div className="modal-action">
					<button
						className="btn btn-error"
						type="button"
						disabled={isLoading ? true : false}
						onClick={() => onClick(props.appId)}>
						{!isLoading ? 'I understand, delete this app' : 'Deleting'}
					</button>
					<button className="btn btn-ghost" type="button" onClick={onClose}>
						Cancel
					</button>
				</div>
			</div>
		</div>
	);
};

export default DeleteAppModal;
