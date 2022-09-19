import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Layout from '@theme/Layout';
import React from 'react';
import EmailSvg from '../../static/img/email.svg';
import ContactForm from '../components/ContactForm';

const Contact = () => {
	const {
		siteConfig: { customFields },
	} = useDocusaurusContext();

	return (
		<Layout title="Contact Us" description="Very simple and affordable">
			<section className="bg-white dark:bg-gray-900">
				<div className="container px-6 py-12 mx-auto">
					<div className="lg:flex lg:items-center lg:-mx-6">
						<div className="lg:w-1/2 lg:mx-6">
							<h1 className="text-3xl font-semibold text-gray-800 capitalize dark:text-white lg:text-5xl">
								Contact us for <br /> more info
							</h1>

							<div className="mt-6 space-y-8 md:mt-8">
								<p className="flex items-start mx-2">
									<EmailSvg className="h-6 w-6 fill-current" />
									<span className="mx-2 text-gray-700 truncate w-72 dark:text-gray-400">
										{customFields.supportUrl}
									</span>
								</p>
							</div>
						</div>

						<div className="mt-8 lg:w-1/2 lg:mx-6">
							<div className="w-full px-8 py-10 mx-auto overflow-hidden bg-white rounded-lg shadow-2xl dark:bg-gray-900 lg:max-w-xl shadow-gray-300/50 dark:shadow-black/50">
								<h1 className="text-lg font-medium text-gray-700">What do you want to ask</h1>
								<ContactForm />
							</div>
						</div>
					</div>
				</div>
			</section>
		</Layout>
	);
};

export default Contact;
