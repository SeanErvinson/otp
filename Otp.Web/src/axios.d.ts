declare module 'axios' {
	interface AxiosStatic {
		isAxiosError<T = any, D = any>(payload: any): payload is AxiosError<T, D>;
	}
}

export {};
