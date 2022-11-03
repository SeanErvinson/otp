import XIcon from '@/components/misc/XIcon';
import { UseFormRegister, FieldError, UseFormResetField } from 'react-hook-form';
import { Fields, FormData } from './AppDetailForm';

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
					{...register('description')}
					className="input input-sm text-2xl font-semibold bg-base-200 focus:bg-base-100 px-0 cursor-text w-4/5"
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

export default DescriptionInput;
