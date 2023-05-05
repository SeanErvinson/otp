import { useEffect, useState } from 'react';
import ReactDOM from 'react-dom';

import Modal from './Modal';

export interface ConfirmationProps {
	title: string;
	body?: string;
	confirmActionLabel?: string;
	confirmLoadingActionLabel?: string;
	cancelActionLabel?: string;
	showModal: boolean;
	onConfirm: () => void;
	onClose: () => void;
}

const ConfirmationModal = (props: ConfirmationProps) => {
	const [isLoading, setIsLoading] = useState(false);

	const handleOnConfirm = async () => {
		setIsLoading(true);
		try {
			props.onConfirm();
			props.onClose();
		} finally {
			setIsLoading(false);
		}
	};

	const handleOnEscClick = (event: KeyboardEvent) => {
		if (event.key === 'Escape') {
			props.onClose();
		}
	};

	useEffect(() => {
		window.addEventListener('keydown', handleOnEscClick);
		return () => {
			window.removeEventListener('keydown', handleOnEscClick);
			setIsLoading(false);
		};
	}, []);

	return ReactDOM.createPortal(
		<Modal onClose={props.onClose} showModal={props.showModal}>
			<div className="modal-box">
				<h3 className="text-xl font-semibold">{props.title}</h3>
				<br />
				{props.body && <p className="text-md">{props.body}</p>}
				<div className="modal-action">
					<button
						className="btn btn-error"
						type="button"
						disabled={isLoading ? true : false}
						onClick={handleOnConfirm}>
						{!isLoading
							? props.confirmActionLabel ?? 'Confirm'
							: props.confirmLoadingActionLabel ?? 'Loading...'}
					</button>
					<button className="btn btn-ghost" type="button" onClick={props.onClose}>
						{props.cancelActionLabel ?? 'Cancel'}
					</button>
				</div>
			</div>
		</Modal>,
		// eslint-disable-next-line @typescript-eslint/no-non-null-assertion
		document.getElementById('portal')!,
	);
};

export default ConfirmationModal;
