import { nanoid } from 'nanoid';
import { useEffect, useState } from 'react';

import XIcon from '@/components/misc/XIcon';

import './tagInput.less';

interface Props {
	initialTags?: string[];
	onUpdate: (tags: string[]) => void;
}

const TagInput = ({ onUpdate, initialTags }: Props) => {
	const [value, setValue] = useState('');
	const [tags, setTags] = useState<string[]>(initialTags ?? []);
	const [keyUp, setKeyUp] = useState(false);

	useEffect(() => {
		onUpdate(tags);
	}, [tags]);

	const addTags = (event: React.KeyboardEvent<HTMLInputElement>) => {
		const cleanedValue = value.trim();
		if (event.key === 'Enter') {
			event.preventDefault();
		}
		if (event.key === 'Enter' && cleanedValue && !tags.includes(cleanedValue)) {
			event.preventDefault();
			setTags(prev => [...prev, cleanedValue]);
			setValue('');
		}
		if (event.key === 'Backspace' && tags.length && !cleanedValue.length && keyUp) {
			event.preventDefault();
			const tagsCopy = [...tags];
			const previousTag = tagsCopy.pop() ?? '';
			setTags(tagsCopy);
			setValue(previousTag);
		}
		setKeyUp(false);
	};

	const removeTag = (value: string) => {
		setTags([...tags.filter(tag => tag !== value)]);
	};

	return (
		<div className="input input-bordered tag-input bg-base-100 w-full">
			<ul className="flex items-center gap-1">
				{tags.map(tag => (
					<li key={nanoid()} className="badge badge-lg flex whitespace-nowrap">
						{tag}&nbsp;
						<span
							className="pl-1 self-center cursor-pointer"
							onClick={() => removeTag(tag)}>
							<XIcon className="inline-block w-4 h-4 stroke-current" />
						</span>
					</li>
				))}
			</ul>
			<input
				type="text"
				autoFocus
				value={value}
				onChange={e => setValue(e.target.value)}
				onKeyUp={() => setKeyUp(true)}
				onKeyDown={addTags}
			/>
		</div>
	);
};

export default TagInput;
