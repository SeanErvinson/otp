import { ReactChild, ReactChildren } from 'react';

interface Props {
	title: string;
	description: string;
	children: ReactChildren | ReactChild;
}

const SettingsSection = (props: Props) => {
	return (
		<section className="my-12">
			<h2 className="text-2xl font-bold mb-2">{props.title}</h2>
			<p className="text-sm mb-2">{props.description}</p>

			<div className="card bg-base-100 shadow-md">
				<div className="card-body">{props.children}</div>
			</div>
		</section>
	);
};

export default SettingsSection;
