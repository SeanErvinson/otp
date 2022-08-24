import { ReactChild } from 'react';

interface Props {
	title: string;
	children: ReactChild;
}

const FilterContainer = (props: Props) => {
	return (
		<div className="bg-base-100 w-56 flex flex-col p-4 rounded-lg ">
			<h3 className="font-bold font-lg">{props.title}</h3>
			{props.children}
		</div>
	);
};

export default FilterContainer;
