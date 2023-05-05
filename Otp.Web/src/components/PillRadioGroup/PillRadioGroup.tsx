import { ChangeEvent, useEffect, useState } from 'react';

import useGeneratedId from '@/hooks/useGeneratedId';

interface Props {
	name: string;
	className?: string;
	initialData: string;
	selections: Selection[] | string[];
	onChange: (value: string) => void;
}

export type Selection = {
	label: string;
	value: string;
};

const PillRadioGroup = (props: Props) => {
	const generatedId = useGeneratedId();
	const [selectedSelection, setSelectedSelections] = useState(props.initialData);

	const handleOnChange = (event: ChangeEvent<HTMLInputElement>) => {
		event.preventDefault();
		setSelectedSelections(event.target.value);
	};

	useEffect(() => {
		props.onChange(selectedSelection);
	}, [selectedSelection]);

	return (
		<div className={`flex ${props.className}`}>
			<div className="overflow-x-auto">
				<div className="inline-flex flex-row border-base-300 bg-white border shadow rounded-full divide-x items-center justify-center overflow-hidden">
					{props.selections.map((selection, index) => {
						const label =
							typeof selection === 'string'
								? selection
								: (selection as Selection).label;
						const value =
							typeof selection === 'string'
								? index.toString()
								: (selection as Selection).value;

						return (
							<label
								key={index}
								className={`label cursor-pointer whitespace-nowrap ${
									selectedSelection === value ? 'bg-gray-200' : ''
								}`}>
								<span className="text-xs px-2">{label}</span>
								<input
									type="radio"
									hidden
									name={generatedId(props.name)}
									defaultValue={value}
									className="radio checked:bg-red-500"
									onChange={handleOnChange}
								/>
							</label>
						);
					})}
				</div>
			</div>
		</div>
	);
};

export default PillRadioGroup;
