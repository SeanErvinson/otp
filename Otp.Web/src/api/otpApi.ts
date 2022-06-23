import { CursorResult } from './../types/types';
import { MetricStrategy, MetricInterval, Log } from '../types/types';
import { oauthInstance, otpInstance, request } from '@/api/https';
import { App, AppDetail, Channel, OtpRequest, PagedResult } from '@/types/types';

export class OtpApi {
	static getApps = async (pageIndex: number): Promise<PagedResult<App> | null> => {
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

	static getApp = async (id: string | undefined): Promise<AppDetail | null> => {
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

	static getAppRecentCallbacks = async (
		id?: string,
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

	static getLogs = async (
		before: string | null = null,
		after: string | null = null,
	): Promise<CursorResult<Log>> => {
		return request(oauthInstance, {
			method: 'GET',
			url: '/logs',
			params: {
				before,
				after,
			},
		});
	};

	static getTimeline = async (id: string) => {
		return request(oauthInstance, {
			method: 'GET',
			url: `/logs/${id}/timeline`,
		});
	};

	static getMetrics = async <T>(
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

	static createApp = async (request: CreateAppRequest): Promise<CreateAppResponse> => {
		const response = await oauthInstance.post('/apps', request);
		return response.data;
	};

	static updateAppCallback = async (request: UpdateCallbackRequest): Promise<void> => {
		await oauthInstance.put(`apps/${request.id}/callback`, request);
	};

	static regenerateAppApiKey = async (
		id: string | undefined,
	): Promise<RegenerateApiKeyResponse> => {
		const response = await oauthInstance.post(`/apps/${id}/regenerate-api-key`);
		return response.data;
	};

	static deleteApp = async (id: string): Promise<void> => {
		await oauthInstance.delete(`/apps/${id}`);
	};
	/**
	 * Otp-Related
	 */

	static getOtpRequest = async (id: string, key: string): Promise<OtpRequest> => {
		const response = await otpInstance(key).get(`/otp/${id}`, {
			params: {
				key: decodeURI(key),
			},
		});
		return response.data;
	};

	static verifyOtp = async (
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

	static cancelOtp = async (id: string, key: string): Promise<CancelOtpResponse> => {
		const response = await otpInstance(key).post(`/otp/cancel`, {
			id: id,
		});
		return response.data;
	};

	static resendOtp = async (id: string, key: string) => {
		const response = await otpInstance(key).post(`/otp/resend`, {
			id: id,
		});
		return response.data;
	};
}

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
