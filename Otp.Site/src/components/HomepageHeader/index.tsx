import React from 'react';
import NewsletterCTA from '../NewsletterCTA';

const HomepageHeader = () => {
	return (
		<div className="container flex flex-col px-6 py-4 mx-auto space-y-6 items-center lg:py-10">
			<h1 className="text-4xl font-bold tracking-wider text-gray-900 dark:text-white lg:text-6xl">
				OHTP
			</h1>
			<p className="pb-6 text-gray-800 text-center dark:text-gray-300 text-xl">
				Why waste your developers time? Let us the do heavy lifting while you focus on your
				idea.
			</p>

			<NewsletterCTA />

			<h2 className="pt-6 text-2xl font-bold tracking-wider text-center text-gray-900 dark:text-white lg:text-4xl">
				No fuss simple OTP integration.
			</h2>
			<p className="text-gray-800 text-center dark:text-gray-300 text-xl">
				Send a request, we'll do the rest.
			</p>
		</div>
	);
};

export default HomepageHeader;
