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
	callbackUrl: string;
	backgroundUrl?: string;
	logoUrl?: string;
	createdAt: Date;
	updatedAt: Date;
};

/**
 * Misc
 */

export type CustomError = {
	type: string;
	title: string;
	detail: string;
};
