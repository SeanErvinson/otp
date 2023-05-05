import { Link } from 'react-router-dom';

import Missing from '@/components/misc/Missing';

const NotFound = () => {
	return (
		<main id="not-found" className="h-screen w-screen flex items-center ">
			<div className="flex flex-col mx-auto items-center gap-4">
				<h2 className="text-3xl font-bold">Page not found</h2>
				<Missing className="h-48 lg:h-64" />
				<h2 className="text-lg lg:text-xl">Opps! You landed on the wrong page.</h2>
				<Link className="btn btn-accent rounded-3xl" to={`/`}>
					Head back Home
				</Link>
			</div>
		</main>
	);
};

export default NotFound;
