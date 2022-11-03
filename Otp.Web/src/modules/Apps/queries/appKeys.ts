const appKeys = {
	all: ['apps'] as const,
	details: (id: string) => [...appKeys.all, 'details', id] as const,
};

export default appKeys;
