import { CursorResult, OtpRequest } from './../types/types';
import { MetricStrategy, MetricInterval, Log } from '../types/types';
import { oauthInstance, otpInstance, request } from '@/api/https';
import { App, AppDetail, Channel, OtpRequestConfig, PagedResult } from '@/types/types';

export class OtpApi {
	static getApps = (pageIndex: number): Promise<PagedResult<App>> => {
		const pageSize = 7;
		return request(oauthInstance, {
			method: 'GET',
			url: '/apps',
			params: {
				pageIndex: pageIndex,
				pageSize: pageSize,
			},
		});
	};

	static getApp = (id: string): Promise<AppDetail> => {
		return request(oauthInstance, {
			method: 'GET',
			url: `/apps/${id}`,
		});
	};

	static getAppRecentCallbacks = async (id: string): Promise<GetAppRecentCallbacksResponse[]> => {
		return request(oauthInstance, {
			method: 'GET',
			url: `/apps/${id}/recent-callbacks`,
		});
	};

	static getLogs = (
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

	static getOtpRequest = (id: string): Promise<OtpRequest> => {
		return request(oauthInstance, {
			method: 'GET',
			url: `/otp/${id}`,
		});
	};

	static getMetrics = <T>(
		strategy: MetricStrategy,
		timeSpan: string,
		metricInterval?: MetricInterval | null,
	): Promise<T> => {
		return request(oauthInstance, {
			method: 'GET',
			url: '/metrics',
			params: {
				metricName: strategy,
				timeSpan: timeSpan,
				metricInterval: metricInterval,
			},
		});
	};

	static createApp = (req: CreateAppRequest): Promise<CreateAppResponse> => {
		return request(oauthInstance, {
			method: 'POST',
			url: '/apps',
			data: req,
		});
	};

	static updateAppCallback = async (req: UpdateCallbackRequest): Promise<void> => {
		return request(oauthInstance, {
			method: 'PUT',
			url: `/apps/${req.id}/callback`,
			data: req,
		});
	};

	static regenerateAppApiKey = async (
		id: string | undefined,
	): Promise<RegenerateApiKeyResponse> => {
		return request(oauthInstance, {
			method: 'POST',
			url: `/apps/${id}/regenerate-api-key`,
		});
	};

	static deleteApp = async (id: string): Promise<void> => {
		return request(oauthInstance, {
			method: 'DELETE',
			url: `/apps/${id}`,
		});
	};

	/**
	 * Otp-Related
	 */

	static getOtpRequestConfig = async (id: string, key: string): Promise<OtpRequestConfig> => {
		return request(otpInstance(key), {
			method: 'GET',
			url: `/otp/${id}/config`,
			params: {
				key: decodeURI(key),
			},
		});
	};

	static verifyOtp = async (
		id: string,
		key: string,
		code: string,
	): Promise<VerifyOtpResponse> => {
		return request(otpInstance(key), {
			method: 'POST',
			url: '/otp/verify',
			data: {
				id: id,
				code: code,
			},
		});
	};

	static cancelOtp = async (id: string, key: string): Promise<CancelOtpResponse> => {
		return request(otpInstance(key), {
			method: 'POST',
			url: '/otp/cancel',
			data: {
				id: id,
			},
		});
	};

	static resendOtp = async (id: string, key: string): Promise<void> => {
		return request(otpInstance(key), {
			method: 'POST',
			url: '/otp/resend',
			data: {
				id: id,
			},
		});
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
