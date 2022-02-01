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

export const isValidGuid = (value: string) => {
	const guidPattern =
		/^[0-9a-f]{8}-[0-9a-f]{4}-[0-5][0-9a-f]{3}-[089ab][0-9a-f]{3}-[0-9a-f]{12}$/i;
	return guidPattern.test(value);
};
