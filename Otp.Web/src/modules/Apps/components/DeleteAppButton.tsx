import { useMutation } from 'react-query';
import { useNavigate } from 'react-router-dom';

import { OtpApi } from '@/api/otpApi';
import ConfirmationModal from '@/components/Modal/ConfirmationModal';
import useModal from '@/hooks/useModal';

interface Props {
	appId: string;
}

const DeleteAppButton = ({ appId }: Props) => {
	const { toggle, visible } = useModal();

	const navigate = useNavigate();

	const mutation = useMutation(OtpApi.deleteApp, {
		onSuccess: () => {
			navigate('/apps');
		},
	});

	return (
		<>
			<button className="btn btn-sm md:btn-md btn-error" onClick={toggle}>
				Delete
			</button>
			{visible && (
				<ConfirmationModal
					onClose={toggle}
					showModal={visible}
					title="Are you sure you want to delete this app?"
					body="Any applications or scripts using this app will no longer be able to access the API. You cannot undo this action."
					onConfirm={() => mutation.mutateAsync(appId)}
					confirmActionLabel="I understand"
					confirmLoadingActionLabel="Deleting"
				/>
			)}
		</>
	);
};

export default DeleteAppButton;
