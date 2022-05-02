import React from 'react';
import ErrorAlertIcon from '../icons/ErrorAlertIcon';

interface Props {
	message: string;
}

const ErrorAlert = (props: Props) => {
	return (
		<div className="flex w-full max-w-sm mx-auto overflow-hidden bg-white rounded-lg shadow-md dark:bg-gray-800">
			<div className="flex items-center justify-center w-12 bg-red-500">
				<ErrorAlertIcon className="w-6 h-6 text-white fill-current" />
			</div>

			<div className="px-4 py-2 -mx-3">
				<div className="mx-3">
					<span className="font-semibold text-red-500 dark:text-red-400">Error</span>
					<p className="text-sm text-gray-600 dark:text-gray-200">{props.message}</p>
				</div>
			</div>
		</div>
	);
};

export default ErrorAlert;
