import { UseFormRegister, FieldError, UseFormResetField } from 'react-hook-form';

import XIcon from '@/components/misc/XIcon';

import { Fields, FormData } from './AppDetailForm';

interface Props {
	register: UseFormRegister<FormData>;
	dirtyFields: Fields;
	fieldError?: FieldError;
	resetField: UseFormResetField<FormData>;
}

const NameInput = (props: Props) => {
	return (
		<div className="flex flex-row justify-between">
			<div className="form-control w-full">
				<div className="flex flex-row items-center">
					<div className="flex flex-row items-center gap-4 flex-1">
						<input
							type="text"
							className="input text-4xl font-bold bg-base-200 focus:bg-base-100 px-0 cursor-text w-4/5 md:w-2/5"
							{...props.register('name')}
						/>
						{props.dirtyFields.name && (
							<button
								onClick={() => props.resetField('name')}
								className="btn btn-outline btn-circle btn-xs">
								<XIcon className="inline-block w-4 h-4 stroke-current" />
							</button>
						)}
					</div>
				</div>
				{props.fieldError && (
					<label className="label">
						<span className="label-text-alt text-error">
							{props.fieldError.message}
						</span>
					</label>
				)}
			</div>
		</div>
	);
};

export default NameInput;
