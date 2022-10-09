import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import useModal from '@site/src/hooks/useModal';
import React, { FormEvent, useRef, useState } from 'react';
import ReCAPTCHA from 'react-google-recaptcha';
import ToastAlert from '../ToastAlert';

const ContactForm = () => {
	const {
		siteConfig: { customFields },
	} = useDocusaurusContext();

	const captchaRef = useRef(null);
	const formRef = useRef(null);

	const { toggle, visible } = useModal();

	const [message, setMessage] = useState('');
	const [isError, setIsError] = useState(false);

	const handleOnSubmit = (event: FormEvent<HTMLFormElement>) => {
		event.preventDefault();
		captchaRef.current.execute();
	};

	const handleOnChangeRecaptcha = (token: string) => {
		if (token) {
			const formData = new FormData(formRef.current);
			formData.append('g-recaptcha-response', token);

			fetch('/contact', {
				method: 'POST',
				headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
				body: new URLSearchParams(formData as any).toString(),
			})
				.then(() => {
					setMessage(`Success! We'll get back to you as soon as possible. 🚀`);
					formRef.current.reset();
					captchaRef.current.reset();
				})
				.catch(_ => {
					setMessage('Uh oh something happened! Please try again.');
					setIsError(true);
				})
				.finally(() => {
					toggle();
				});
		}
	};

	return (
		<>
			<form
				className="mt-6"
				ref={formRef}
				name="contact"
				data-netlify="true"
				data-netlify-recaptcha="true"
				onSubmit={handleOnSubmit}>
				<input type="hidden" name="form-name" value="contact" />

				<div className="flex-1">
					<label className="block mb-2 text-sm text-gray-600 dark:text-gray-200">Full Name</label>
					<input
						name="name"
						type="text"
						required
						className="block w-full px-5 py-3 mt-2 text-gray-700 placeholder-gray-400 bg-white border border-gray-200 rounded-md dark:placeholder-gray-600 dark:bg-gray-900 dark:text-gray-300 dark:border-gray-700 focus:border-blue-400 dark:focus:border-blue-400 focus:ring-blue-400 focus:outline-none focus:ring focus:ring-opacity-40"
					/>
				</div>

				<div className="flex-1 mt-6">
					<label className="block mb-2 text-sm text-gray-600 dark:text-gray-200">Email address</label>
					<input
						name="email"
						type="email"
						required
						className="block w-full px-5 py-3 mt-2 text-gray-700 placeholder-gray-400 bg-white border border-gray-200 rounded-md dark:placeholder-gray-600 dark:bg-gray-900 dark:text-gray-300 dark:border-gray-700 focus:border-blue-400 dark:focus:border-blue-400 focus:ring-blue-400 focus:outline-none focus:ring focus:ring-opacity-40"
					/>
				</div>

				<div className="w-full mt-6">
					<label className="block mb-2 text-sm text-gray-600 dark:text-gray-200">Message</label>
					<textarea
						required
						name="message"
						maxLength={500}
						className="block w-full h-32 px-5 py-3 mt-2 text-gray-700 placeholder-gray-400 bg-white border border-gray-200 rounded-md md:h-48 dark:placeholder-gray-600 dark:bg-gray-900 dark:text-gray-300 dark:border-gray-700 focus:border-blue-400 dark:focus:border-blue-400 focus:ring-blue-400 focus:outline-none focus:ring focus:ring-opacity-40"
						placeholder="Hello there"></textarea>
				</div>
				<ReCAPTCHA
					ref={captchaRef}
					sitekey={customFields.recaptchaSiteKey}
					size="invisible"
					onChange={handleOnChangeRecaptcha}
				/>
				<button
					type="submit"
					className="w-full px-6 py-3 mt-6 text-sm font-medium tracking-wide text-white capitalize transition-colors duration-300 transform bg-blue-500 rounded-md hover:bg-blue-400 focus:outline-none focus:ring focus:ring-blue-300 focus:ring-opacity-50">
					get in touch
				</button>
			</form>

			{visible && <ToastAlert message={message} onClose={toggle} isError={isError} />}
		</>
	);
};

export default ContactForm;
