import React from 'react';
import NewsletterCTA from '../NewsletterCTA';

const HomepageHeader = () => {
	return (
		<header className="bg-white dark:bg-gray-900">
			<div className="p-8 md:p-12 lg:px-16 lg:py-24">
				<div className="max-w-lg mx-auto text-center">
					<h1 className="text-4xl font-bold text-gray-900 md:text-5xl dark:text-white">
						No fuss simple OTP integration.
					</h1>
					<p className="hidden text-gray-500 sm:mt-4 sm:block dark:text-gray-300">
						Why waste your developers time? Let us the do heavy lifting while you focus on your idea. Send a
						request, we'll do the rest.
					</p>
				</div>

				<div className="max-w-xl mx-auto mt-8">
					<NewsletterCTA />
				</div>
			</div>
		</header>
	);
};

export default HomepageHeader;
