export const isValidUrl = (value: string) => {
	let url: URL;

	try {
		url = new URL(value);
	} catch (_) {
		return false;
	}

	return url.protocol === 'http:' || url.protocol === 'https:';
};

export const truncate = (str: string, n: number) => {
	return str.length > n ? str.slice(0, n) + '...' : str;
};
