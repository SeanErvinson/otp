import 'animate.css';

import { yupResolver } from '@hookform/resolvers/yup';
import axios, { AxiosError } from 'axios';
import { useState } from 'react';
import { useForm, useFormState, useWatch } from 'react-hook-form';

import ErrorIcon from '@/components/misc/ErrorIcon';
import { TagInput } from '@/components/TagInput';
import { ProblemDetails } from '@/types/types';

import useCreateApp from '../../mutations/useCreateApp';

import validationSchema from './validationSchema';

type FormData = {
	name: string;
	description: string;
};

interface Props {
	onSubmit: (apiKey: string) => void;
	onClose: () => void;
}

const CreateAppForm = (props: Props) => {
	const [tags, setTags] = useState<string[]>([]);
	const {
		handleSubmit,
		register,
		reset: formReset,
		control,
	} = useForm<FormData>({
		resolver: yupResolver(validationSchema),
	});
	const {
		errors: formErrors,
		isSubmitting,
		isValid,
	} = useFormState<FormData>({
		control,
	});
	const descriptionWatch = useWatch({ control, name: 'description', defaultValue: '' });
	const mutation = useCreateApp();

	const handleOnClose = () => {
		formReset();
		mutation.reset();
		props.onClose();
	};

	const handleOnSubmit = (data: FormData) => {
		mutation.mutate(
			{
				name: data.name,
				tags: tags,
				description: data.description,
			},
			{
				onSuccess(data) {
					props.onSubmit(data.apiKey);
				},
			},
		);
	};

	return (
		<>
			<h3 className="text-xl font-semibold">Create App</h3>
			<form onSubmit={handleSubmit(handleOnSubmit)}>
				<div className="form-control">
					<label className="label" htmlFor="name">
						<span className="label-text">Name</span>
					</label>
					<input
						id="name"
						type="text"
						placeholder="best-app"
						className={`input input-bordered ${formErrors.name && 'input-error'}`}
						{...register('name')}
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
						<label className="label" htmlFor="description">
							<span className="label-text">Description</span>
						</label>
						<span className="label-text">{128 - descriptionWatch.length}</span>
					</div>
					<textarea
						id="description"
						className={`textarea h-24 textarea-bordered ${
							formErrors.description && 'input-error'
						}`}
						placeholder="Greatest description of all time"
						{...register('description')}></textarea>
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
				{axios.isAxiosError<ProblemDetails>(mutation.error) && mutation.error.response && (
					<div className="alert alert-error mt-2">
						<div className="flex-1">
							<ErrorIcon />
							<label>
								{
									(mutation.error as AxiosError<ProblemDetails>).response?.data
										.detail
								}
							</label>
						</div>
					</div>
				)}
				<div className="modal-action">
					<button
						className={`btn btn-accent ${mutation.isLoading && 'loading'}`}
						disabled={mutation.isLoading || !isValid || isSubmitting}
						type="submit">
						{!mutation.isLoading ? 'Create' : 'Creating'}
					</button>
					<button className="btn btn-ghost" type="button" onClick={handleOnClose}>
						Cancel
					</button>
				</div>
			</form>
		</>
	);
};

export default CreateAppForm;
