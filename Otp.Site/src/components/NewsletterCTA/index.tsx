import React, { FormEvent, useRef } from 'react';
import ReCAPTCHA from 'react-google-recaptcha';

const NewsletterCTA = () => {
	const captchaRef = useRef(null);
	const formRef = useRef(null);

	const handleOnSubmit = (event: FormEvent<HTMLFormElement>) => {
		event.preventDefault();
		captchaRef.current.execute();
	};

	const handleOnChangeRecaptcha = (token: string) => {
		if (token) {
			const formData = new FormData(formRef.current);
			formData.append('g-recaptcha-response', token);

			fetch('/', {
				method: 'POST',
				headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
				body: new URLSearchParams(formData as any).toString(),
			})
				.then(() => console.log('Form successfully submitted'))
				.catch(error => alert(error));
		}
		captchaRef.current.reset();
	};

	return (
		<>
			<div className="w-full mt-8 bg-transparent border rounded-md lg:max-w-sm dark:border-gray-700 focus-within:border-blue-400 focus-within:ring focus-within:ring-blue-300 dark:focus-within:border-blue-400 focus-within:ring-opacity-40">
				<form
					className="flex flex-col lg:flex-row"
					ref={formRef}
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
					<ReCAPTCHA
						ref={captchaRef}
						sitekey="6LdI3f8hAAAAAO_5fv1tetK__ZnCL0X2j-kDsCu-"
						size="invisible"
						onChange={handleOnChangeRecaptcha}
					/>
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
