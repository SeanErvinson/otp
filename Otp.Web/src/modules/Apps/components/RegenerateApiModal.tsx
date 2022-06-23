import 'animate.css';
import ReactDOM from 'react-dom';
import { useMutation } from 'react-query';

import { OtpApi } from '@/api/otpApi';

import ApiKeyPreview from './ApiKeyPreview';

interface Props {
	appId: string;
	showCreateAppModal: boolean;
	onClose: () => void;
}

const RegenerateApiModal = (props: Props) => {
	const { mutate, isLoading, isSuccess, data } = useMutation(OtpApi.regenerateAppApiKey, {});

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
					onClick={() => mutate(props.appId)}>
					{!isLoading ? 'I understand' : 'Regenerating'}
				</button>
				<button className="btn btn-ghost" type="button" onClick={props.onClose}>
					Cancel
				</button>
			</div>
		</>
	) : (
		<>{data && <ApiKeyPreview apiKey={data.apiKey} onClose={props.onClose} />}</>
	);

	return ReactDOM.createPortal(
		<div
			id="regenerateApiModal"
			className={`modal ${props.showCreateAppModal && 'modal-open'}`}>
			<div
				className={`modal-box flex flex-col justify-between ${
					isSuccess && 'animate__animated animate__flipInY min-h-[21rem]'
				}`}>
				{defaultComponent}
			</div>
		</div>,
		document.getElementById('portal')!,
	);
};

export default RegenerateApiModal;
