import BrowserOnly from '@docusaurus/BrowserOnly';
import React, { FormEvent, useEffect, useState } from 'react';

declare var grecaptcha: any;

declare global {
	interface Window {
		onRecaptchaResponse: any;
	}
}

const NewsletterCTA = () => {
	const handleOnSubmit = (event: FormEvent<HTMLFormElement>) => {
		event.preventDefault();
		setFormData(null);
		console.log('Executing grecaptcha');

		grecaptcha.execute();

		console.log('Executed grecaptcha');

		setFormData(event.currentTarget);
	};

	const [formData, setFormData] = useState<HTMLFormElement | null>();

	return (
		<>
			<BrowserOnly>
				{() => {
					window.onRecaptchaResponse = (token: string) => {
						console.log('Executing callback');
						console.log(token);

						const x = grecaptcha.getResponse();
						console.log(x);

						if (formData) {
							fetch('/', {
								method: 'POST',
								headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
								body: new URLSearchParams(
									new FormData({ ...formData, 'g-recaptcha-response': token }) as any,
								).toString(),
							})
								.then(() => console.log('Form successfully submitted'))
								.catch(error => alert(error));
						}
					};
					return <></>;
				}}
			</BrowserOnly>
			<div className="w-full mt-8 bg-transparent border rounded-md lg:max-w-sm dark:border-gray-700 focus-within:border-blue-400 focus-within:ring focus-within:ring-blue-300 dark:focus-within:border-blue-400 focus-within:ring-opacity-40">
				<form
					className="flex flex-col lg:flex-row"
					name="newsletter"
					data-netlify="true"
					data-netlify-recaptcha="true"
					onSubmit={handleOnSubmit}>
					<input type="hidden" name="form-name" value="newsletter" />
					<input
						name="email"
						type="email"
						required
						placeholder="Enter your email address"
						className="flex-1 h-10 px-4 py-2 m-1 text-gray-700 placeholder-gray-400 bg-transparent border-none appearance-none dark:text-gray-200 focus:outline-none focus:placeholder-transparent focus:ring-0"
					/>
					<div
						className="g-recaptcha"
						data-callback="onRecaptchaResponse"
						data-sitekey="6LdI3f8hAAAAAO_5fv1tetK__ZnCL0X2j-kDsCu-"
						data-size="invisible"></div>
					<button
						type="submit"
						className="h-10 px-4 py-2 m-1 text-white transition-colors duration-200 transform bg-blue-500 rounded-md hover:bg-blue-400 focus:outline-none focus:bg-blue-400">
						Get Notified
					</button>
				</form>
			</div>
			<p className="mt-3 text-sm text-gray-500 dark:text-gray-300">
				Be the first to get notified once we release the product.
			</p>
		</>
	);
};

export default NewsletterCTA;
