import Layout from '@theme/Layout';
import React from 'react';

const PrivacyPolicy = () => {
	return (
		<Layout title="Privacy Policy" description="OHTP Privacy Policy">
			<main className="bg-white dark:bg-gray-900">
				<div className="container px-6 py-10 mx-auto">
					<h1 className="text-3xl font-semibold text-center text-gray-800 capitalize xl:text-5xl lg:text-4xl dark:text-white">
						Privacy Policy
					</h1>

					<div className="flex justify-center mx-auto mt-6">
						<span className="inline-block w-40 h-1 bg-rose-500 rounded-full"></span>
						<span className="inline-block w-3 h-1 mx-1 bg-rose-500 rounded-full"></span>
						<span className="inline-block w-1 h-1 bg-rose-500 rounded-full"></span>
					</div>

					<div className="flex items-start max-w-6xl mx-auto mt-16">
						<div>
							<p className="flex items-center text-gray-500 lg:mx-8">
								Your privacy is very important to us. Accordingly, we have developed this Policy in
								order for you to understand how we collect, use, communicate and disclose and make use
								of personal information. The following outlines our privacy policy.
							</p>

							<ul className="my-4 list-disc text-gray-500 lg:mx-8">
								<li>
									Before or at the time of collecting personal information, we will identify the
									purposes for which information is being collected.
								</li>
								<li>
									We will collect and use of personal information solely with the objective of
									fulfilling those purposes specified by us and for other compatible purposes, unless
									we obtain the consent of the individual concerned or as required by law.
								</li>
								<li>
									We will only retain personal information as long as necessary for the fulfillment of
									those purposes.
								</li>
								<li>
									We will collect personal information by lawful and fair means and, where
									appropriate, with the knowledge or consent of the individual concerned.
								</li>
								<li>
									Personal data should be relevant to the purposes for which it is to be used, and, to
									the extent necessary for those purposes, should be accurate, complete, and
									up-to-date.
								</li>
								<li>
									We will protect personal information by reasonable security safeguards against loss
									or theft, as well as unauthorized access, disclosure, copying, use or modification.
								</li>
								<li>
									We will make readily available to customers information about our policies and
									practices relating to the management of personal information.
								</li>
							</ul>

							<p className="flex text-gray-500 lg:mx-8">
								We are committed to conducting our business in accordance with these principles in order
								to ensure that the confidentiality of personal information is protected and maintained.
							</p>
						</div>
					</div>
				</div>
			</main>
		</Layout>
	);
};

export default PrivacyPolicy;
