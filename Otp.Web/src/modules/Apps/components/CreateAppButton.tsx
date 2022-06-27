import useModal from '@/hooks/useModal';

import CreateAppModal from './CreateAppModal';

const CreateAppButton = () => {
	const { toggle, visible } = useModal();
	return (
		<>
			<button className="btn btn-accent" onClick={toggle}>
				Create App
			</button>
			{visible && <CreateAppModal showModal={visible} onClose={toggle}></CreateAppModal>}
		</>
	);
};

export default CreateAppButton;
