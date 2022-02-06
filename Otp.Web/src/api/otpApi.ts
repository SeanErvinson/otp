import { oauthInstance, otpInstance } from '@/api/https';
import { App, AppDetail, OtpRequest, PagedResult } from '@/common/types';

export const getApps = async (pageIndex: number): Promise<PagedResult<App> | null> => {
	const pageSize = 7;
	const response = await oauthInstance.get('/apps', {
		params: {
			pageIndex: pageIndex,
			pageSize: pageSize,
		},
	});

	return response.data;
};

export const getApp = async (id: string | undefined): Promise<AppDetail | null> => {
	if (!id) {
		return null;
	}
	const response = await oauthInstance.get(`/apps/${id}`);
	if (response.status === 404) {
		return null;
	}
	return response.data;
};

export const getOtpRequest = async (id: string, secret: string): Promise<OtpRequest> => {
	const response = await otpInstance(secret).get(`/otp/${id}`, {
		params: {
			secret: decodeURI(secret),
		},
	});
	return response.data;
};

export const verifyOtp = async (
	id: string,
	secret: string,
	code: string,
): Promise<VerifyOtpResponse> => {
	const response = await otpInstance(secret).post(`/otp/verify`, {
		id: id,
		code: code,
	});
	return response.data;
};

export const cancelOtp = async (id: string, secret: string): Promise<CancelOtpResponse> => {
	const response = await otpInstance(secret).post(`/otp/cancel`, {
		id: id,
	});
	return response.data;
};

export const resendOtp = async (id: string, secret: string) => {
	const response = await otpInstance(secret).post(`/otp/resend`, {
		id: id,
	});
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

export type VerifyOtpResponse = {
	successUrl: string;
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
