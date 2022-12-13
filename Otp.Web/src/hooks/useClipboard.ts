import { useState } from 'react';

const useClipboard = (duration?: 500) => {
	const [isCopying, setIsCopying] = useState(false);

	const handleOnCopy = (value: string) => {
		setIsCopying(true);
		navigator.clipboard.writeText(value);
		setTimeout(() => {
			setIsCopying(false);
		}, duration);
	};

	return { copy: handleOnCopy, isCopying };
};

export default useClipboard;
