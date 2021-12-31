import instance from '@/api/https';

export const getApps = async (pageIndex: number): Promise<GetAppsResponse | null> => {
	const response = await instance.get('/apps', {
		params: {
			pageIndex: pageIndex,
			pageSize: 7,
		},
	});

	return response.data;
	// return { items: [] as GetAppsApp[] } as GetAppsResponse;
	// return {
	// 	apps: [
	// 		{
	// 			name: 'hello',
	// 			description: 'for testing purpose',
	// 			createdAt: new Date(),
	// 			tags: ['website', 'app', 'willow', 'sean', 'dev', 'twin'],
	// 		},
	// 		{
	// 			name: 'hello',
	// 			description: 'for testing purpose',
	// 			createdAt: new Date(),
	// 			tags: ['website', 'app', 'willow', 'sean', 'dev', 'twin'],
	// 		},
	// 	],
	// } as GetAppsResponse;
};

export const getApp = async (id: string | undefined): Promise<GetAppResponse | null> => {
	if (!id) {
		return null;
	}
	const response = await instance.get(`/apps/${id}`);
	if (response.status === 404) {
		return null;
	}
	return response.data;
};

export const createApp = async (request: CreateAppRequest): Promise<CreateAppResponse> => {
	const response = await instance.post('/apps', request);
	return response.data;
};

export const regenerateAppApiKey = async (
	id: string | undefined,
): Promise<RegenerateApiKeyResponse> => {
	const response = await instance.post(`/apps/${id}/regenerate-api-key`);
	return response.data;
};

export const deleteApp = async (id: string): Promise<void> => {
	await instance.delete(`/apps/${id}`);
};

type CreateAppRequest = {
	name: string;
	description: string;
};

export type CreateAppResponse = {
	id: string;
	apiKey: string;
};

export type RegenerateApiKeyResponse = {
	apiKey: string;
};

export type GetAppResponse = {
	id: string;
	name: string;
	description: string;
	tags: string[];
	callbackUrl: string;
	createdAt: Date;
	updatedAt: Date;
};

type GetAppsResponse = {
	items: GetAppsApp[];
	pageNumber: number;
	totalPages: number;
	totalCount: number;
	hasPreviousPage: boolean;
	hasNextPage: boolean;
};

export type GetAppsApp = {
	id: string;
	name: string;
	description?: string | undefined;
	createdAt: Date;
	tags: string[];
};
