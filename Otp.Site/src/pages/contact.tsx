import Layout from '@theme/Layout';
import React, { FormEvent } from 'react';
import EmailSvg from '../../static/img/email.svg';

const Contact = () => {
	const handleOnSubmit = (event: FormEvent<HTMLFormElement>) => {
		event.preventDefault();
		fetch('/contact', {
			method: 'POST',
			headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
			body: new URLSearchParams(new FormData(event.currentTarget) as any).toString(),
		})
			.then(() => console.log('Form successfully submitted'))
			.catch(error => alert(error));
	};

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
										support@ohtp.dev
									</span>
								</p>
							</div>
						</div>

						<div className="mt-8 lg:w-1/2 lg:mx-6">
							<div className="w-full px-8 py-10 mx-auto overflow-hidden bg-white rounded-lg shadow-2xl dark:bg-gray-900 lg:max-w-xl shadow-gray-300/50 dark:shadow-black/50">
								<h1 className="text-lg font-medium text-gray-700">What do you want to ask</h1>

								<form
									className="mt-6"
									name="contact"
									method="POST"
									data-netlify="true"
									onSubmit={handleOnSubmit}>
									<input type="hidden" name="form-name" value="contact" />

									<div className="flex-1">
										<label className="block mb-2 text-sm text-gray-600 dark:text-gray-200">
											Full Name
										</label>
										<input
											name="name"
											type="text"
											required
											className="block w-full px-5 py-3 mt-2 text-gray-700 placeholder-gray-400 bg-white border border-gray-200 rounded-md dark:placeholder-gray-600 dark:bg-gray-900 dark:text-gray-300 dark:border-gray-700 focus:border-blue-400 dark:focus:border-blue-400 focus:ring-blue-400 focus:outline-none focus:ring focus:ring-opacity-40"
										/>
									</div>

									<div className="flex-1 mt-6">
										<label className="block mb-2 text-sm text-gray-600 dark:text-gray-200">
											Email address
										</label>
										<input
											name="email"
											type="email"
											required
											className="block w-full px-5 py-3 mt-2 text-gray-700 placeholder-gray-400 bg-white border border-gray-200 rounded-md dark:placeholder-gray-600 dark:bg-gray-900 dark:text-gray-300 dark:border-gray-700 focus:border-blue-400 dark:focus:border-blue-400 focus:ring-blue-400 focus:outline-none focus:ring focus:ring-opacity-40"
										/>
									</div>

									<div className="w-full mt-6">
										<label className="block mb-2 text-sm text-gray-600 dark:text-gray-200">
											Message
										</label>
										<textarea
											required
											name="message"
											maxLength={500}
											className="block w-full h-32 px-5 py-3 mt-2 text-gray-700 placeholder-gray-400 bg-white border border-gray-200 rounded-md md:h-48 dark:placeholder-gray-600 dark:bg-gray-900 dark:text-gray-300 dark:border-gray-700 focus:border-blue-400 dark:focus:border-blue-400 focus:ring-blue-400 focus:outline-none focus:ring focus:ring-opacity-40"
											placeholder="Hello there"></textarea>
									</div>
									<button
										type="submit"
										className="w-full px-6 py-3 mt-6 text-sm font-medium tracking-wide text-white capitalize transition-colors duration-300 transform bg-blue-500 rounded-md hover:bg-blue-400 focus:outline-none focus:ring focus:ring-blue-300 focus:ring-opacity-50">
										get in touch
									</button>
								</form>
							</div>
						</div>
					</div>
				</div>
			</section>
		</Layout>
	);
};

export default Contact;
