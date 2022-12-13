import 'animate.css';

import Modal from '@/components/Modal/Modal';

import ApiKeyPreview from './ApiKeyPreview';
import CreateAppForm from './CreateAppForm/CreateAppForm';

interface Props {
	showModal: boolean;
	onClose: () => void;
}

const CreateAppModal = (props: Props) => {
	const handleOnClose = () => {
		props.onClose();
	};

	let defaultComponent = mutation.data ? (
		<ApiKeyPreview apiKey={mutation.data.apiKey} onClose={handleOnClose} />
	) : (
		<CreateAppForm onClose={handleOnClose} />
	);

	return (
		<Modal showModal={props.showModal} onClose={props.onClose}>
			<div
				className={`modal-box flex flex-col justify-between ${
					mutation.isSuccess && 'animate__animated animate__flipInY min-h-[21rem]'
				}`}>
				{defaultComponent}
			</div>
		</Modal>
	);
};

export default CreateAppModal;
