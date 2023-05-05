import { useState, useEffect } from 'react';
import {
	UseFormRegister,
	UseFormGetValues,
	UseFormSetValue,
	UseFormResetField,
} from 'react-hook-form';

import XIcon from '@/components/misc/XIcon';
import { TagCollection } from '@/components/TagCollection';
import { TagInput } from '@/components/TagInput';

import { Fields, FormData } from './AppDetailForm';

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
					<TagInput onUpdate={handleOnTagInput} initialTags={tags} autoFocus />
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

export default TagsInput;
