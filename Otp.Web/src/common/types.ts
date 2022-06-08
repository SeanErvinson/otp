/**
 * Misc
 */

export type PagedResult<T> = {
	items: T[];
	pageNumber: number;
	totalPages: number;
	totalCount: number;
	hasPreviousPage: boolean;
	hasNextPage: boolean;
};

export type CursorResult<T> = {
	before: string;
	after: string;
	hasAfter: boolean;
	hasBefore: boolean;
	items: T[];
};

export type CursorRequest = {
	before?: string;
	after?: string;
};

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

export const Channel = ['SMS', 'Email'] as const;
export type Channel = typeof Channel[number];

export type OtpRequest = {
	backgroundUrl?: string;
	logoUrl?: string;
	contact: string;
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
	receiver: string;
	channel: Channel;
	app: string;
};

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

export type CustomError = {
	type: string;
	title: string;
	detail: string;
};
