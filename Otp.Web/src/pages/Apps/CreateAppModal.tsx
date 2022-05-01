import { useState } from 'react';
import { useForm, useFormState, useWatch } from 'react-hook-form';
import { useMutation } from 'react-query';
import 'animate.css';

import { createApp, CreateAppResponse } from '@/api/otpApi';
import ErrorIcon from '@/components/misc/ErrorIcon';
import { TagInput } from '@/components/TagInput';

import ApiKeyPreview from './ApiKeyPreview';
import { AxiosError } from 'axios';
import { CustomError } from '@/common/types';

interface Props {
	showCreateAppModal: boolean;
	onClose: () => void;
}

type FormData = {
	name: string;
	description: string;
};

const CreateAppModal = (props: Props) => {
	const [error, setError] = useState<string | undefined>();
	const [isLoading, setIsLoading] = useState(false);
	const [isSuccess, setIsSuccess] = useState(false);
	const [tags, setTags] = useState<string[]>([]);
	const { handleSubmit, register, reset, control } = useForm<FormData>();
	const { errors } = useFormState<FormData>({
		control,
	});
	const [createAppResponse, setCreateAppResponse] = useState({} as CreateAppResponse);
	const descriptionWatch = useWatch({ control, name: 'description', defaultValue: '' });

	const mutation = useMutation(createApp, {
		onMutate: () => {
			setError(undefined);
			setIsLoading(true);
		},
		onSuccess: response => {
			if (response instanceof Error) return;
			setCreateAppResponse(response);
			setIsSuccess(true);
			setIsLoading(false);
		},
		onError: (response: AxiosError<CustomError>) => {
			setIsLoading(false);
			setError(response.response?.data.detail);
		},
	});

	const onSubmit = (data: FormData) => {
		mutation.mutate({
			name: data.name,
			tags: tags,
			description: data.description,
		});
	};

	const onClose = () => {
		setIsLoading(false);
		setIsSuccess(false);
		reset();
		props.onClose();
	};

	let defaultComponent = !isSuccess ? (
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
						className={`input input-bordered ${errors.name && 'input-error'}`}
						{...register('name', {
							required: true,
							minLength: 5,
							maxLength: 64,
							pattern: {
								value: /[\w-]/,
								message: 'Name should only contain alphanumeric, -, or _',
							},
						})}
					/>

					{errors.name && (
						<label className="label">
							<span className="label-text-alt text-error">{errors.name.message}</span>
						</label>
					)}
				</div>
				<div className="form-control">
					<div className="flex flex-row items-center justify-between">
						<label className="label">
							<span className="label-text">Description</span>
						</label>
						<span className="label-text">{128 - descriptionWatch.length}</span>
					</div>
					<textarea
						className={`textarea h-24 textarea-bordered ${
							errors.description && 'input-error'
						}`}
						placeholder="Greatest description of all time"
						{...register('description', {
							maxLength: 128,
						})}></textarea>
					{errors.description && (
						<label className="label">
							<span className="label-text-alt text-error">
								{errors.description.message}
							</span>
						</label>
					)}
				</div>
				<div className="form-control">
					<label className="label">
						<span className="label-text">Tags</span>
					</label>
					<TagInput onUpdate={setTags} />
				</div>
				{error && (
					<div className="alert alert-error mt-2">
						<div className="flex-1">
							<ErrorIcon />
							<label>{error}</label>
						</div>
					</div>
				)}
				<div className="modal-action">
					<button
						className={`btn btn-accent ${isLoading && 'loading'}`}
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

export default CreateAppModal;
