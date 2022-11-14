const appKeys = {
	all: ['apps'] as const,
	lists: (pageIndex?: number) => [...appKeys.all, 'list', pageIndex] as const,
	list: () => [...appKeys.all, 'list'] as const,
	details: (id: string) => [...appKeys.all, 'details', id] as const,
};

export default appKeys;
