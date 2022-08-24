import { ReactChild, ReactChildren, useEffect } from 'react';
import ReactDOM from 'react-dom';

interface Props {
	children: ReactChildren | ReactChild;
	showModal: boolean;
	onClose: () => void;
}

const Modal = (props: Props) => {
	const handleOnEscClick = (event: KeyboardEvent) => {
		if (event.key === 'Escape') {
			props.onClose();
		}
	};

	useEffect(() => {
		window.addEventListener('keydown', handleOnEscClick);
		return () => {
			window.removeEventListener('keydown', handleOnEscClick);
		};
	}, []);

	return ReactDOM.createPortal(
		<div id="confirmationModal" className={`modal ${props.showModal && 'modal-open'}`}>
			{props.children}
		</div>,
		document.getElementById('portal')!,
	);
};

export default Modal;
