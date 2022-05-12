interface ImportMetaEnv {
	readonly VITE_OTP_API_BASE_URL: string;
	readonly VITE_OTP_SITE_BASE_URL: string;
}

interface ImportMeta {
	readonly env: ImportMetaEnv;
}
