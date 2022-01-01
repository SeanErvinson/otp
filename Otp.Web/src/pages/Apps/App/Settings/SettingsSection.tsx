import { ReactChild, ReactChildren } from 'react';

interface Props {
	title: string;
	description: string;
	children: ReactChildren | ReactChild;
}

const SettingsSection = (props: Props) => {
	return (
		<section className="my-6">
			<h2 className="text-lg font-semibold mb-2">{props.title}</h2>
			<p className="text-xs mb-2">{props.description}</p>
			<hr className="mx-4 my-3" />
			{props.children}
		</section>
	);
};

export default SettingsSection;
