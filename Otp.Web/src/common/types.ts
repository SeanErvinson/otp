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

/**
 * Otp
 */

const Channel = ['SMS', 'Email'] as const;
export type Channel = typeof Channel[number];

export type OtpRequest = {
	backgroundUri?: string;
	logoUri?: string;
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
	callbackUrl: string;
	createdAt: Date;
	updatedAt: Date;
};