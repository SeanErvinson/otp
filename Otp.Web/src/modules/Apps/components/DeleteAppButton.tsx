import useModal from '@/hooks/useModal';

import DeleteAppModal from './DeleteAppModal';

interface Props {
	appId: string;
}

const DeleteAppButton = ({ appId }: Props) => {
	const { toggle, visible } = useModal();

	return (
		<>
			<button className="btn btn-sm md:btn-md btn-error" onClick={toggle}>
				Delete
			</button>
			<DeleteAppModal appId={appId} showModal={visible} onClose={toggle}></DeleteAppModal>
		</>
	);
};

export default DeleteAppButton;
