/**
 * User
 */

export const SubscriptionType = ['Free', 'Consumption'] as const;
export type SubscriptionType = typeof SubscriptionType[keyof typeof SubscriptionType];

/**
 * Otp
 */
export enum MetricInterval {
	Day = 'Day',
	Month = 'Month',
}
const MetricStrategy = ['ChannelUsageCount', 'EmailUsageCount', 'SmsUsageCount'] as const;
export type CountMetric = {
	name: string;
	data: {
		previousMonthSentRequest: number;
		sentRequest: number;
	};
};

export type ChannelUsageMetric = {
	name: string;
	data: {};
};
export type MetricStrategy = typeof MetricStrategy[number];

export const OtpRequestStatus = ['Success', 'Failed'] as const;
export type OtpRequestStatus = typeof OtpRequestStatus[keyof typeof OtpRequestStatus];

export const Channel = ['SMS', 'Email'] as const;
export type Channel = typeof Channel[number];

export type OtpRequestConfig = {
	backgroundUrl?: string;
	logoUrl?: string;
	contact: string;
};

export const OtpAttemptStatus = ['Success', 'Fail', 'Canceled'] as const;
export type OtpAttemptStatus = typeof OtpAttemptStatus[keyof typeof OtpAttemptStatus];

export type OtpRequest = {
	id: string;
	attempts: {
		attemptedOn: Date;
		attemptStatus: OtpAttemptStatus;
	}[];
	channel: Channel;
	clientInfo: {
		ipAddress: string;
		referrer: string;
		userAgent: string;
	};
	expiresOn: Date;
	maxAttempts: number;
	recipient: string;
	requestedAt: Date;
	resendCount: number;
	status: OtpRequestStatus;
	timeline: {
		state: EventStatus;
		occuredAt: Date;
		response?: string;
		status: string;
	}[];
};

/**
 * Apps
 */

export type App = {
	id: string;
	name: string;
	description?: string | undefined;
	createdAt: Date;
	tags: string[];
};

export type AppDetail = {
	id: string;
	name: string;
	description: string;
	tags?: string[];
	callbackUrl?: string;
	backgroundUrl?: string;
	logoUrl?: string;
	createdAt: Date;
	updatedAt: Date;
};

/**
 * Logs
 */

export type Log = {
	id: string;
	status: Status;
	eventDate: Date;
	recipient: string;
	channel: Channel;
	app: {
		appId: string;
		appName: string;
	};
};

export const EventStatus = {
	request: 'Request',
	send: 'Send',
	deliver: 'Deliver',
};

export type EventStatus = typeof EventStatus[keyof typeof EventStatus];

export const Status = {
	success: 'Success',
	warning: 'Warning',
	error: 'Error',
	delivered: 'Delivered',
	sent: 'Sent',
} as const;
export type Status = typeof Status[keyof typeof Status];

/**
 * Misc
 */

export type ProblemDetails = {
	type?: string;
	title?: string;
	status?: number;
	detail?: string;
	instance?: string;
	additionalProperties?: Record<string, any>;
};

export type PagedResult<T> = {
	items: T[];
	pageNumber: number;
	totalPages: number;
	totalCount: number;
	hasPreviousPage: boolean;
	hasNextPage: boolean;
};

export type CursorResult<T> = {
	before?: string;
	after?: string;
	items: T[];
};

export type CursorRequest = {
	before?: string;
	after?: string;
};