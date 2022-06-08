import { MetricStrategy, MetricInterval, Log } from './../common/types';
import { oauthInstance, otpInstance } from '@/api/https';
import { App, AppDetail, Channel, OtpRequest, PagedResult } from '@/common/types';

export const getApps = async (pageIndex: number): Promise<PagedResult<App> | null> => {
	// const pageSize = 7;
	// const response = await oauthInstance.get('/apps', {
	// 	params: {
	// 		pageIndex: pageIndex,
	// 		pageSize: pageSize,
	// 	},
	// });

	return {
		hasNextPage: false,
		hasPreviousPage: false,
		pageNumber: 1,
		totalCount: 4,
		totalPages: 1,
		items: [
			{
				id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cbf6',
				createdAt: new Date(),
				tags: ['Hello'],
				name: 'Willow',
			},
			{
				id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cba6',
				createdAt: new Date(),
				tags: ['Hello'],
				name: 'Microsoft',
			},
			{
				id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cbf1',
				createdAt: new Date(),
				tags: ['Hello'],
				name: 'Twitter',
			},
			{
				id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cbf3',
				createdAt: new Date(),
				tags: ['Hello'],
				name: 'Google',
			},
		],
	};

	// return response.data;
};

export const getApp = async (id: string | undefined): Promise<AppDetail | null> => {
	if (!id) {
		return null;
	}
	return {
		id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cbf3',
		name: 'Google',
		description: 'Hello world',
		tags: ['hello', 'world', 'hellasdfaso1', 'hell2', 'asdfasdfa', 'helzo', 'helz123asdfo'],
		createdAt: new Date(),
		updatedAt: new Date(),
	};
	const response = await oauthInstance.get(`/apps/${id}`);
	if (response.status === 404) {
		return null;
	}
	return response.data;
};

export const getAppRecentCallbacks = async (
	id: string | undefined,
): Promise<GetAppRecentCallbacksResponse[] | null> => {
	if (!id) {
		return null;
	}
	const response = await oauthInstance.get(`/apps/${id}/recent-callbacks`);
	if (response.status === 404) {
		return null;
	}
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

export const getMetrics = async <T>(
	strategy: MetricStrategy,
	timeSpan: string,
	metricInterval?: MetricInterval | null,
): Promise<T> => {
	const response = await oauthInstance.get(`/metrics`, {
		params: {
			metricName: strategy,
			timeSpan: timeSpan,
			metricInterval: metricInterval,
		},
	});
	return response.data;
};

export const getLogs = async (pageIndex: number): Promise<Log> => {
	const response = await oauthInstance.get('/logs', {
		params: {
			pageIndex: pageIndex,
		},
	});
	return response.data;
};

/**
 * Otp-Related
 */

export const getOtpRequest = async (id: string, key: string): Promise<OtpRequest> => {
	const response = await otpInstance(key).get(`/otp/${id}`, {
		params: {
			key: decodeURI(key),
		},
	});
	return response.data;
};

export const verifyOtp = async (
	id: string,
	key: string,
	code: string,
): Promise<VerifyOtpResponse> => {
	const response = await otpInstance(key).post(`/otp/verify`, {
		id: id,
		code: code,
	});
	return response.data;
};

export const cancelOtp = async (id: string, key: string): Promise<CancelOtpResponse> => {
	const response = await otpInstance(key).post(`/otp/cancel`, {
		id: id,
	});
	return response.data;
};

export const resendOtp = async (id: string, key: string) => {
	const response = await otpInstance(key).post(`/otp/resend`, {
		id: id,
	});
	return response.data;
};

export const createApp = async (request: CreateAppRequest): Promise<CreateAppResponse> => {
	const response = await oauthInstance.post('/apps', request);
	return response.data;
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

export type GetAppRecentCallbacksResponse = {
	requestId: string;
	createdAt: Date;
	statusCode: number;
	channel: Channel;
	responseMessage?: string;
};
