import 'animate.css';
import { useState } from 'react';

import Modal from '@/components/Modal/Modal';

import ApiKeyPreview from './ApiKeyPreview';
import CreateAppForm from './CreateAppForm/CreateAppForm';

interface Props {
	showModal: boolean;
	onClose: () => void;
}

const CreateAppModal = (props: Props) => {
	const [apiKey, setApiKey] = useState<string | undefined>();
	const handleOnClose = () => {
		props.onClose();
	};

	const handleOnSubmit = (value: string) => {
		setApiKey(value);
	};

	let defaultComponent = apiKey ? (
		<ApiKeyPreview apiKey={apiKey} onClose={handleOnClose} />
	) : (
		<CreateAppForm onClose={handleOnClose} onSubmit={handleOnSubmit} />
	);

	return (
		<Modal showModal={props.showModal} onClose={props.onClose}>
			<div
				className={`modal-box flex flex-col justify-between ${
					apiKey && 'animate__animated animate__flipInY min-h-[21rem]'
				}`}>
				{defaultComponent}
			</div>
		</Modal>
	);
};

export default CreateAppModal;
