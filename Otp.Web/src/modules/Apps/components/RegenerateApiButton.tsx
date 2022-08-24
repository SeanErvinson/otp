import { useMutation } from 'react-query';

import { OtpApi } from '@/api/otpApi';
import Modal from '@/components/Modal/Modal';
import useModal from '@/hooks/useModal';

import ApiKeyPreview from './ApiKeyPreview';

interface Props {
	appId: string;
}

const RegenerateApiButton = ({ appId }: Props) => {
	const { toggle, visible } = useModal();
	const { mutateAsync, isLoading, isSuccess, data, reset } = useMutation(
		OtpApi.regenerateAppApiKey,
		{},
	);

	const handleOnClose = () => {
		reset();
		toggle();
	};

	let defaultComponent = !data ? (
		<>
			<h3 className="text-xl font-semibold">Are you sure you want to regenerate this key?</h3>
			<br />
			<p className="text-md">
				Any applications or scripts using this api key will no longer be able to access the
				API. You cannot undo this action.
			</p>
			<div className="modal-action">
				<button
					className="btn btn-error"
					type="button"
					disabled={isLoading ? true : false}
					onClick={() => mutateAsync(appId)}>
					{!isLoading ? 'I understand' : 'Regenerating'}
				</button>
				<button className="btn btn-ghost" type="button" onClick={handleOnClose}>
					Cancel
				</button>
			</div>
		</>
	) : (
		<>{data && <ApiKeyPreview apiKey={data.apiKey} onClose={handleOnClose} />}</>
	);

	return (
		<>
			<button className="btn btn-sm md:btn-md btn-error" onClick={handleOnClose}>
				Regenerate Key
			</button>
			{visible && (
				<Modal showModal={visible} onClose={handleOnClose}>
					<div
						className={`modal-box flex flex-col gap-4 ${
							isSuccess && 'animate__animated animate__flipInY'
						}`}>
						{defaultComponent}
					</div>
				</Modal>
			)}
		</>
	);
};

export default RegenerateApiButton;
