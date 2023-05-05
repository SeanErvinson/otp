import { yupResolver } from '@hookform/resolvers/yup';
import { useForm, useFormState } from 'react-hook-form';
import { useQueryClient } from '@tanstack/react-query';

import { AppDetail } from '@/types/types';

import useSaveAppDescriptors from '../../mutations/useSaveAppDescriptors';
import appKeys from '../../queries/appKeys';

import DescriptionInput from './DescriptionInput';
import NameInput from './NameInput';
import TagsInput from './TagsInput';
import validationSchema from './validationSchema';

export type FormData = {
	name: string;
	description: string;
	tags: string[];
};

export interface Fields {
	name?: boolean | undefined;
	description?: boolean | undefined;
	tags?: boolean[] | undefined;
}

interface Props {
	id: string;
}

const AppDetailForm = (props: Props) => {
	const queryClient = useQueryClient();
	const queryData = queryClient.getQueryData<AppDetail>(appKeys.details(props.id));
	const selectedApp = queryData ?? ({} as AppDetail);

	const { handleSubmit, register, control, resetField, setValue, getValues } = useForm<FormData>({
		defaultValues: {
			name: selectedApp.name,
			description: selectedApp.description,
			tags: selectedApp.tags,
		},
		resolver: yupResolver(validationSchema),
	});
	const { isDirty, dirtyFields, errors, isValid, isSubmitting } = useFormState<FormData>({
		control,
	});
	const mutation = useSaveAppDescriptors();

	const onSubmit = (data: FormData) => {
		mutation.mutate({
			appId: selectedApp.id,
			name: data.name,
			description: data.description,
			tags: data.tags,
		});
	};

	return (
		<form onSubmit={handleSubmit(onSubmit)} className="flex flex-1 flex-col gap-2 w-full">
			<NameInput
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
						className={`btn btn-sm btn-success ${mutation.isLoading && 'loading'}`}
						type="submit"
						disabled={mutation.isLoading || !isValid || isSubmitting}>
						{!mutation.isLoading ? 'Save' : 'Saving'}
					</button>
				</div>
			)}
		</form>
	);
};

export default AppDetailForm;
