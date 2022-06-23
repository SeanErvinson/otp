import 'animate.css';
import { useState } from 'react';
import ReactDOM from 'react-dom';
import { useMutation } from 'react-query';
import { useNavigate } from 'react-router-dom';

import { OtpApi } from '@/api/otpApi';

interface Props {
	appId: string;
	showModal: boolean;
	onClose: () => void;
}

const DeleteAppModal = (props: Props) => {
	const [isLoading, setIsLoading] = useState(false);
	const navigate = useNavigate();

	const mutation = useMutation(OtpApi.deleteApp, {
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

	return ReactDOM.createPortal(
		<div id="deleteAppModal" className={`modal ${props.showModal && 'modal-open'}`}>
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
						{!isLoading ? 'I understand' : 'Deleting'}
					</button>
					<button className="btn btn-ghost" type="button" onClick={onClose}>
						Cancel
					</button>
				</div>
			</div>
		</div>,
		document.getElementById('portal')!,
	);
};

export default DeleteAppModal;
