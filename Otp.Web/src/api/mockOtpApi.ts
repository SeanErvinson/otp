import { createServer } from 'miragejs';

export class MockOtpApi {
	static LoadMockServer = () => {
		createServer({
			routes() {
				this.urlPrefix = import.meta.env.VITE_OTP_API_BASE_URL;
				this.get('/apps', () => {
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

				this.get('/apps/:id', () => {
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
			},
		});
	};
}
