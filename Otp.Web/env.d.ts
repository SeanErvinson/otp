interface ImportMetaEnv {
	readonly VITE_OTP_API_BASE_URL: string;
	readonly VITE_OTP_SITE_BASE_URL: string;
	readonly VITE_ENABLE_MOCK_SERVER: boolean;

	//Azure B2C
	readonly VITE_B2C_CONFIG_CLIENT_ID: string;
	readonly VITE_B2C_CONFIG_AUTHORITY: string;
	readonly VITE_B2C_CONFIG_REDIRECT_URI: string;
	readonly VITE_B2C_CONFIG_KNOWN_AUTHORITY: string;
}

interface ImportMeta {
	readonly env: ImportMetaEnv;
}
