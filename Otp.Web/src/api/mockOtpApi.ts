import { createServer } from 'miragejs';

export const makeServer = () => {
	const otpApiBaseUrl = import.meta.env.VITE_OTP_API_BASE_URL;
	return createServer({
		routes() {
			this.get(`${otpApiBaseUrl}/apps`, () => {
				return {
					hasNextPage: false,
					hasPreviousPage: false,
					pageNumber: 1,
					totalCount: 4,
					totalPages: 1,
					items: [
						{
							id: '90c0d1dd-c757-4d94-b77e-54d4b542c3ff',
							createdAt: new Date(),
							tags: ['startup'],
							name: 'Willow',
						},
						{
							id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cba6',
							createdAt: new Date(),
							tags: ['goat'],
							name: 'Microsoft',
						},
						{
							id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cbf1',
							createdAt: new Date(),
							tags: ['social-media'],
							name: 'Twitter',
						},
						{
							id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cbf3',
							createdAt: new Date(),
							tags: ['service'],
							name: 'Google',
						},
						{
							id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cbf3',
							createdAt: new Date(),
							tags: ['finetech'],
							name: 'Stripe',
						},
					],
				};
			});

			this.get(`${otpApiBaseUrl}/apps/:id`, () => {
				return {
					id: 'f90b5605-fe2e-43e9-9fa0-8d53b481cbf3',
					name: 'Google',
					description: 'Hello world',
					tags: [
						'hello',
						'world',
						'hellasdfaso1',
						'hell2',
						'asdfasdfa',
						'helzo',
						'helz123asdfo',
					],
					createdAt: new Date(),
					updatedAt: new Date(),
				};
			});
			this.get(
				`${otpApiBaseUrl}/otp/:id`,
				() => {
					return {
						id: '8164cd9a-8ff9-4db7-a81a-bfe1d4fa2002',
						app: {
							id: 'd3399ac7-e00c-4023-99ac-5c50727b79f6',
							createdAt: new Date(),
							name: 'Willow',
						},
						attempts: [],
						channel: 'SMS',
						status: 'Success',
						timeline: [],
						requestedAt: new Date(),
						clientInfo: {
							ipAddress: '0.0.0.0',
							referrer: 'http://localhost:3000/logs',
							userAgent:
								'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.53 Safari/537.36',
						},
					};
				},
				{
					timing: 2000,
				},
			);

			this.post(`${otpApiBaseUrl}/apps/:id/regenerate-api-key`, () => {
				return {
					apiKey: 'hello',
				};
			});

			this.passthrough('https://otpdev.b2clogin.com/**');
		},
	});
};
