import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useMutation } from 'react-query';
import 'animate.css';

import { createApp, CreateAppResponse } from '@/api/otpApi';

import ApiKeyPreview from './ApiKeyPreview';

interface Props {
	showCreateAppModal: boolean;
	onClose: () => void;
}

type FormData = {
	name: string;
	description: string;
};

const CreateAppModal = (props: Props) => {
	const { handleSubmit, register } = useForm<FormData>();
	const [isLoading, setIsLoading] = useState(false);
	const [createAppResponse, setCreateAppResponse] = useState({} as CreateAppResponse);
	const [isSuccess, setIsSuccess] = useState(false);

	const mutation = useMutation(createApp, {
		onMutate: () => {
			setIsLoading(true);
		},
		onSuccess: response => {
			console.log(response);
			setCreateAppResponse(response);
			setIsSuccess(true);
			setIsLoading(false);
		},
	});

	const onSubmit = (data: FormData) => {
		mutation.mutate({
			name: data.name,
			description: data.description,
		});
	};

	const onClose = () => {
		setIsLoading(false);
		props.onClose();
	};

	let defaultComponent = (
		<>
			<h3 className="text-xl font-semibold">Create App</h3>
			<form onSubmit={handleSubmit(onSubmit)}>
				<div className="form-control">
					<label className="label">
						<span className="label-text">Name</span>
					</label>
					<input
						type="text"
						placeholder="best-app"
						className="input input-bordered"
						{...register('name')}
					/>
				</div>
				<div className="form-control">
					<label className="label">
						<span className="label-text">Description</span>
					</label>
					<textarea
						className="textarea h-24 textarea-bordered"
						placeholder="Greatest description of all time"
						{...register('description')}></textarea>
				</div>
				<div className="modal-action">
					<button
						className={`btn btn-accent ${isLoading ? 'loading' : ''}`}
						disabled={isLoading ? true : false}
						type="submit">
						{!isLoading ? 'Create' : 'Creating'}
					</button>
					<button className="btn btn-ghost" type="button" onClick={onClose}>
						Cancel
					</button>
				</div>
			</form>
		</>
	);
	if (isSuccess) {
		defaultComponent = <ApiKeyPreview apiKey={createAppResponse.apiKey} onClose={onClose} />;
	}

	return (
		<div
			id="createAppModal"
			className={`modal ${props.showCreateAppModal ? 'modal-open' : ''}`}>
			<div
				className={`modal-box min-h-[21rem] flex flex-col justify-between ${
					isSuccess ? 'animate__animated animate__flipInY' : ''
				}`}>
				{defaultComponent}
			</div>
		</div>
	);
};

export default CreateAppModal;
