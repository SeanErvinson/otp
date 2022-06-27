import 'animate.css';
import { AxiosError } from 'axios';
import { useState } from 'react';
import { useForm, useFormState, useWatch } from 'react-hook-form';
import { useMutation } from 'react-query';

import { OtpApi } from '@/api/otpApi';
import ErrorIcon from '@/components/misc/ErrorIcon';
import Modal from '@/components/Modal/Modal';
import { TagInput } from '@/components/TagInput';
import { CustomError } from '@/types/types';

import ApiKeyPreview from './ApiKeyPreview';

interface Props {
	showModal: boolean;
	onClose: () => void;
}

type FormData = {
	name: string;
	description: string;
};

const CreateAppModal = (props: Props) => {
	const [tags, setTags] = useState<string[]>([]);
	const { handleSubmit, register, reset: formReset, control } = useForm<FormData>();
	const { errors: formErrors } = useFormState<FormData>({
		control,
	});
	const descriptionWatch = useWatch({ control, name: 'description', defaultValue: '' });

	const { mutate, data, reset, error, isLoading, isSuccess } = useMutation(OtpApi.createApp, {});

	const handleOnSubmit = (data: FormData) => {
		mutate({
			name: data.name,
			tags: tags,
			description: data.description,
		});
	};

	const handleOnClose = () => {
		formReset();
		reset();
		props.onClose();
	};

	let defaultComponent = !data ? (
		<>
			<h3 className="text-xl font-semibold">Create App</h3>
			<form onSubmit={handleSubmit(handleOnSubmit)}>
				<div className="form-control">
					<label className="label">
						<span className="label-text">Name</span>
					</label>
					<input
						type="text"
						placeholder="best-app"
						className={`input input-bordered ${formErrors.name && 'input-error'}`}
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

					{formErrors.name && (
						<label className="label">
							<span className="label-text-alt text-error">
								{formErrors.name.message}
							</span>
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
							formErrors.description && 'input-error'
						}`}
						placeholder="Greatest description of all time"
						{...register('description', {
							maxLength: 128,
						})}></textarea>
					{formErrors.description && (
						<label className="label">
							<span className="label-text-alt text-error">
								{formErrors.description.message}
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
							<label>
								{(error as AxiosError<CustomError>).response?.data.detail}
							</label>
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
					<button className="btn btn-ghost" type="button" onClick={handleOnClose}>
						Cancel
					</button>
				</div>
			</form>
		</>
	) : (
		<ApiKeyPreview apiKey={data.apiKey} onClose={handleOnClose} />
	);

	return (
		<Modal showModal={props.showModal} onClose={props.onClose}>
			<div
				className={`modal-box flex flex-col justify-between ${
					isSuccess && 'animate__animated animate__flipInY min-h-[21rem]'
				}`}>
				{defaultComponent}
			</div>
		</Modal>
	);
};

export default CreateAppModal;
