import { ReactNode } from 'react';

interface Props {
	id: string;
	children: ReactNode;
}

const MainContainer = (props: Props) => {
	return (
		<main id={props.id} className="h-full mx-auto container">
			{props.children}
		</main>
	);
};

export default MainContainer;
