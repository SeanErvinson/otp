import 'animate.css';
import { useState } from 'react';
import { useMutation } from 'react-query';

import { RegenerateApiKeyResponse, regenerateAppApiKey } from '@/api/otpApi';

import ApiKeyPreview from '../App/components/ApiKeyPreview';

interface Props {
	appId: string;
	showCreateAppModal: boolean;
	onClose: () => void;
}

const RegenerateApiModal = (props: Props) => {
	const [isLoading, setIsLoading] = useState(false);
	const [createAppResponse, setCreateAppResponse] = useState({} as RegenerateApiKeyResponse);
	const [isSuccess, setIsSuccess] = useState(false);

	const mutation = useMutation(regenerateAppApiKey, {
		onMutate: () => {
			setIsLoading(true);
		},
		onSuccess: response => {
			setCreateAppResponse(response);
			setIsLoading(false);
			setIsSuccess(true);
		},
	});

	const onClick = (id: string) => {
		mutation.mutate(id);
	};

	const onClose = () => {
		setIsLoading(false);
		setIsSuccess(false);
		props.onClose();
	};

	let defaultComponent = !isSuccess ? (
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
					onClick={() => onClick(props.appId)}>
					{!isLoading ? 'I understand' : 'Regenerating'}
				</button>
				<button className="btn btn-ghost" type="button" onClick={onClose}>
					Cancel
				</button>
			</div>
		</>
	) : (
		<ApiKeyPreview apiKey={createAppResponse.apiKey} onClose={onClose} />
	);

	return (
		<div id="createAppModal" className={`modal ${props.showCreateAppModal && 'modal-open'}`}>
			<div
				className={`modal-box flex flex-col justify-between ${
					isSuccess && 'animate__animated animate__flipInY min-h-[21rem]'
				}`}>
				{defaultComponent}
			</div>
		</div>
	);
};

export default RegenerateApiModal;
