import React from 'react';

import useGeneratedId from '@/hooks/useGeneratedId';

interface Props {
	name: string;
	onPaste: (event: React.ClipboardEvent<HTMLInputElement>) => void;
	onInput: (event: React.ChangeEvent<HTMLInputElement>) => void;
	onBackspace: (event: React.KeyboardEvent<HTMLInputElement>) => void;
}

const idPrefix = 'otp-input';
const OtpInput = React.forwardRef<HTMLInputElement, Props>((props, ref) => {
	const generatedId = useGeneratedId();

	return (
		<input
			type="tel"
			title="otp-input"
			id={generatedId(idPrefix)}
			name={props.name}
			ref={ref}
			onPaste={props.onPaste}
			onChange={props.onInput}
			onKeyDown={props.onBackspace}
			maxLength={1}
			className="border border-gray-500 w-10 h-10 text-center"
		/>
	);
});
OtpInput.displayName = 'OtpInput';
export default OtpInput;
