import { useAtom } from 'jotai';
import { useState, useEffect } from 'react';
import {
	FieldError,
	useForm,
	UseFormGetValues,
	UseFormRegister,
	UseFormResetField,
	UseFormSetValue,
	useFormState,
} from 'react-hook-form';
import { useMutation } from 'react-query';

import { OtpApi } from '@/api/otpApi';
import XIcon from '@/components/misc/XIcon';
import { TagCollection } from '@/components/TagCollection';
import { TagInput } from '@/components/TagInput';
import DeleteAppButton from '@/modules/Apps/components/DeleteAppButton';

import { selectedAppAtom } from '../states/SelectedAppAtom';

const AppNameInput = ({
	register,
	dirtyFields,
	fieldError,
	resetField,
}: {
	register: UseFormRegister<FormData>;
	dirtyFields: Fields;
	fieldError?: FieldError;
	resetField: UseFormResetField<FormData>;
}) => {
	const [selectedApp] = useAtom(selectedAppAtom);

	return (
		<div className="flex flex-row justify-between">
			<div className="form-control w-full">
				<div className="flex flex-row items-center">
					<div className="flex flex-row items-center gap-4 flex-1">
						<input
							type="text"
							className="input text-3xl font-bold bg-base-200 focus:bg-base-100 px-0 cursor-text w-4/5 md:w-2/5"
							{...register('name', {
								required: {
									value: true,
									message: 'Name is required',
								},
								minLength: {
									value: 4,
									message: 'Name must be longer than 4 characters',
								},
								maxLength: {
									value: 32,
									message: 'Name must be shorter than 32 characters',
								},
								pattern: {
									value: /^[\w-]+$/,
									message: 'Name should only contain alphanumeric, -, or _',
								},
							})}
						/>
						{dirtyFields.name && (
							<button
								onClick={() => resetField('name')}
								className="btn btn-outline btn-circle btn-xs">
								<XIcon className="inline-block w-4 h-4 stroke-current" />
							</button>
						)}
					</div>
					<DeleteAppButton appId={selectedApp.id} />
				</div>
				{fieldError && (
					<label className="label">
						<span className="label-text-alt text-error">{fieldError.message}</span>
					</label>
				)}
			</div>
		</div>
	);
};

const DescriptionInput = ({
	register,
	dirtyFields,
	fieldError,
	resetField,
}: {
	register: UseFormRegister<FormData>;
	dirtyFields: Fields;
	fieldError?: FieldError;
	resetField: UseFormResetField<FormData>;
}) => {
	return (
		<div className="form-control">
			<div className="flex flex-row items-center gap-4">
				<input
					type="text"
					{...register('description', {
						maxLength: {
							message: 'Description should be shorter than 128 characters',
							value: 128,
						},
					})}
					className="input input-sm text-xl font-semibold bg-base-200 focus:bg-base-100 px-0 cursor-text w-4/5"
				/>
				{dirtyFields.description && (
					<button
						onClick={() => resetField('description')}
						className="btn btn-outline btn-circle btn-xs">
						<XIcon className="inline-block w-4 h-4 stroke-current" />
					</button>
				)}
			</div>
			{fieldError && (
				<label className="label">
					<span className="label-text-alt text-error">{fieldError.message}</span>
				</label>
			)}
		</div>
	);
};

const TagsInput = ({
	register,
	setValue,
	getValues,
	resetField,
}: {
	register: UseFormRegister<FormData>;
	dirtyFields: Fields;
	getValues: UseFormGetValues<FormData>;
	setValue: UseFormSetValue<FormData>;
	resetField: UseFormResetField<FormData>;
}) => {
	const [showTagInput, setShowTagInput] = useState(false);
	useEffect(() => {
		register('tags');
	}, []);

	const handleOnTagInput = (values: string[]) => {
		if (values.length <= 0) return;
		setValue('tags', values, {
			shouldDirty: true,
			shouldTouch: true,
			shouldValidate: true,
		});
	};

	const tags = getValues('tags');

	const handleOnTagCancel = () => {
		resetField('tags');
		setShowTagInput(!showTagInput);
	};
	return (
		<>
			{!showTagInput ? (
				<div className="flex flex-row items-center">
					<div className="w-9/12 md:w-auto">
						{tags && <TagCollection tags={tags} tagCount={5} />}
					</div>
					<div
						className="flex-3 badge badge-outline border-dashed text-sm cursor-pointer hover:bg-base-300 ml-1 whitespace-nowrap"
						onClick={() => setShowTagInput(!showTagInput)}>
						+ Add tags
					</div>
				</div>
			) : (
				<div className="flex flex-row gap-2 items-center lg:w-full">
					<TagInput onUpdate={handleOnTagInput} initialTags={tags} />
					<button
						className="btn btn-outline btn-xs border-dashed"
						onClick={handleOnTagCancel}>
						<XIcon className="inline-block w-4 h-4 stroke-current" />
						Cancel
					</button>
				</div>
			)}
		</>
	);
};

type FormData = {
	name: string;
	description: string;
	tags: string[];
};

interface Fields {
	name?: boolean | undefined;
	description?: boolean | undefined;
	tags?: boolean[] | undefined;
}

const AppDetailForm = () => {
	const [selectedApp, setSelectedApp] = useAtom(selectedAppAtom);
	const { handleSubmit, register, control, resetField, setValue, getValues } = useForm<FormData>({
		defaultValues: {
			name: selectedApp.name,
			description: selectedApp.description,
			tags: selectedApp.tags,
		},
	});
	const { isDirty, dirtyFields, errors } = useFormState<FormData>({ control });

	const { mutate, isLoading } = useMutation(OtpApi.saveAppDescriptors, {
		onSuccess(data) {
			setSelectedApp(data);
		},
	});

	const onSubmit = (data: FormData) => {
		mutate({
			appId: selectedApp.id,
			name: data.name,
			description: data.description,
			tags: data.tags,
		});
	};

	return (
		<form onSubmit={handleSubmit(onSubmit)} className="flex flex-1 flex-col gap-2 w-full">
			<AppNameInput
				dirtyFields={dirtyFields}
				fieldError={errors.name}
				register={register}
				resetField={resetField}
			/>
			<DescriptionInput
				dirtyFields={dirtyFields}
				fieldError={errors.description}
				register={register}
				resetField={resetField}
			/>
			<TagsInput
				dirtyFields={dirtyFields}
				getValues={getValues}
				register={register}
				resetField={resetField}
				setValue={setValue}
			/>

			{isDirty && (
				<div>
					<button
						className={`btn btn-sm btn-success ${isLoading && 'loading'}`}
						type="submit"
						disabled={isLoading}>
						{!isLoading ? 'Save' : 'Saving'}
					</button>
				</div>
			)}
		</form>
	);
};

export default AppDetailForm;
