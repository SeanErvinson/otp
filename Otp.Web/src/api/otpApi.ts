import { oauthInstance, headerAuthInstance } from '@/api/https';

export const getApps = async (pageIndex: number): Promise<GetAppsResponse | null> => {
	const pageSize = 7;
	const response = await oauthInstance.get('/apps', {
		params: {
			pageIndex: pageIndex,
			pageSize: pageSize,
		},
	});

	return response.data;
};

export const getApp = async (id: string | undefined): Promise<GetAppResponse | null> => {
	if (!id) {
		return null;
	}
	const response = await oauthInstance.get(`/apps/${id}`);
	if (response.status === 404) {
		return null;
	}
	return response.data;
};

export const getOtpRequest = async (id: string, secret: string): Promise<GetOtpRequestResponse> => {
	const response = await headerAuthInstance.get(`/otp/${id}`, {
		params: {
			secret: decodeURI(secret),
		},
	});
	return response.data;
};

export const verifyOtp = async (request: VerifyOtpRequest): Promise<VerifyOtpResponse> => {
	const response = await headerAuthInstance.post(`/otp/verify`, request);
	return response.data;
};

export const cancelOtp = async (request: CancelOtpRequest): Promise<CancelOtpResponse> => {
	const response = await headerAuthInstance.post(`/otp/cancel`, request);
	return response.data;
};

export const resendOtp = async (request: ResendRequest) => {
	const response = await headerAuthInstance.post(`/otp/resend`, request);
	return response.data;
};

export const createApp = async (request: CreateAppRequest): Promise<CreateAppResponse> => {
	const response = await oauthInstance.post('/apps', request);
	return response.data;
};

export const updateAppCallback = async (request: UpdateCallbackRequest): Promise<void> => {
	await oauthInstance.put(`apps/${request.id}/callback`, request);
};

export const regenerateAppApiKey = async (
	id: string | undefined,
): Promise<RegenerateApiKeyResponse> => {
	const response = await oauthInstance.post(`/apps/${id}/regenerate-api-key`);
	return response.data;
};

export const deleteApp = async (id: string): Promise<void> => {
	await oauthInstance.delete(`/apps/${id}`);
};

export type VerifyOtpRequest = {
	id: string;
	secret: string;
	code: string;
};

export type VerifyOtpResponse = {
	successUrl: string;
};

export type ResendRequest = {
	id: string;
	secret: string;
};

type CancelOtpRequest = {
	id: string;
	secret: string;
};

export type CancelOtpResponse = {
	cancelUrl: string;
};

type CreateAppRequest = {
	name: string;
	tags: string[];
	description: string;
};

export type CreateAppResponse = {
	id: string;
	apiKey: string;
};

type UpdateCallbackRequest = {
	id: string;
	callbackUrl: string;
	endpointSecret: string;
};

export type RegenerateApiKeyResponse = {
	apiKey: string;
};

export type GetOtpRequestResponse = {
	backgroundUri?: string;
	logoUri?: string;
	contact: string;
};

export type GetAppResponse = {
	id: string;
	name: string;
	description: string;
	tags?: string[];
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
