export const invertKeyValues = <T>(obj: Record<string, T>): Record<string, T> => {
	return Object.fromEntries(Object.entries(obj).map(([k, v]) => [v, k]));
};
