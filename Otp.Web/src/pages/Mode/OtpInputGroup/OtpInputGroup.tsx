import { nanoid } from 'nanoid';
import React from 'react';
import { useRef } from 'react';
import { OtpInput } from './OtpInput';

interface Props {
	inputLength?: number;
	onSetValue: (value: string) => void;
}

const OtpInputGroup = ({ onSetValue, inputLength = 6 }: Props) => {
	const otpInputRefs = useRef(new Array<HTMLInputElement>(inputLength));

	const handleInput = (e: React.ChangeEvent<HTMLInputElement>) => {
		const input = e.target;

		onSetValue(
			Array.from({ length: inputLength }, (element, i) => {
				return otpInputRefs.current[i].value || '';
			}).join(''),
		);

		if (input.nextElementSibling && input.value) {
			var position = parseInt(input.name);
			otpInputRefs.current[position + 1].focus();
			otpInputRefs.current[position + 1].select();
		}
	};

	const handlePaste = (e: React.ClipboardEvent<HTMLInputElement>) => {
		const paste = e.clipboardData.getData('text');
		otpInputRefs.current.forEach((element, index) => {
			element.value = paste[index] || '';
		});
		onSetValue(paste);
	};

	const handleBackspace = (e: React.KeyboardEvent<HTMLInputElement>) => {
		if (e.key !== 'Backspace') return;
		const input = e.target as HTMLInputElement;
		var position = parseInt(input.name);
		if (position - 1 < 0) return;

		if (position == otpInputRefs.current.length - 1) {
			otpInputRefs.current[position].value = '';
		}
		otpInputRefs.current[position - 1].focus();
	};

	return (
		<>
			{Array.from({ length: inputLength }, (item, index) => (
				<OtpInput
					key={nanoid()}
					name={index.toString()}
					onBackspace={handleBackspace}
					onInput={handleInput}
					onPaste={handlePaste}
					ref={(el: HTMLInputElement) => (otpInputRefs.current[index] = el)}
				/>
			))}
		</>
	);
};

export default React.memo(OtpInputGroup);
