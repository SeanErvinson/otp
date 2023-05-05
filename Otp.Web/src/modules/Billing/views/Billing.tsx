import { useQueryClient } from '@tanstack/react-query';

import MainContainer from '@/components/Layouts/MainContainer';
import SectionContainer from '@/components/Layouts/SettingsSection';
import PageHeader from '@/components/PageHeader/PageHeader';
import useSubscription from '@/hooks/useSubscription';
import useUserConfig, { UserConfig, userConfigKeys } from '@/hooks/useUserConfig';

const RemainingCredit = ({ credit }: { credit: string }) => {
	const queryClient = useQueryClient();
	const handleOnFree = () => {
		queryClient.setQueryData<UserConfig>(
			userConfigKeys.config,
			prev =>
				prev && {
					...prev,
					subscription: 'Free',
				},
		);
	};

	const handleOnConsumption = () => {
		queryClient.setQueryData<UserConfig>(
			userConfigKeys.config,
			prev =>
				prev && {
					...prev,
					subscription: 'Consumption',
				},
		);
	};

	return (
		<div className="stats bg-primary text-primary-content">
			<div className="stat">
				<div className="stat-title">Remaining credit</div>
				<div className="stat-value">{credit}</div>
				<div className="stat-actions">
					<button className="btn btn-primary" onClick={handleOnFree}>
						Free
					</button>
					<button className="btn btn-primary" onClick={handleOnConsumption}>
						Consumption
					</button>
				</div>
			</div>
		</div>
	);
};

const PredefinedPricing = ({ value }: { value: number }) => {
	return (
		<li className="form-control flex flex-col cursor-pointer p-6 rounded-lg items-center">
			<label className="label">
				<span className="label-text">{value}</span>
				<input type="radio" name="radio-6" className="radio checked:bg-red-500" checked />
			</label>
		</li>
	);
};

const AddCreditModal = () => {
	const pricing = [500, 1000, 2500, 5000, 10000, 25000, 50000, 100000];

	return (
		<div className="flex flex-col w-full">
			<input type="text" placeholder="Type here" className="input w-full max-w-xs" />
			<div className="divider"></div>
			<ul className="grid gap-4 grid-cols-pricing w-full">
				{pricing.map(price => PredefinedPricing({ value: price }))}
			</ul>
		</div>
	);
};

const Billing = () => {
	const { userConfig } = useUserConfig();
	const consumptionOnly = useSubscription('Consumption');
	const freeOnly = useSubscription('Free');

	return (
		<MainContainer id="billing">
			<PageHeader title="Billing" />
			<article>
				<RemainingCredit credit={''} />
				<div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
					<div className="order-2 sm:order-1 card bg-base-100 shadow-md">
						<div className="card-body">
							<div className="flex flex-row justify-between">
								<h2 className="card-title">Current subscription:</h2>
								<span className="rounded-full text-md self-center bg-slate-500 text-white px-5 py-1 text-center">
									{userConfig.subscription}
								</span>
							</div>
							<p>Current billing period runs from Oct 31 to Nov 30.</p>
							<div className="card-actions justify-end">
								{freeOnly && <button className="btn btn-primary">Upgrade</button>}
								{consumptionOnly && (
									<button className="btn btn-ghost">Cancel</button>
								)}
							</div>
						</div>
					</div>
					<div className="order-1 sm:order-2">
						<div className="stats bg-white text-black">
							<div className="stat">
								<div className="stat-title">Amount due</div>
								<div className="stat-value">$1000.00</div>
								<div className="stat-actions">
									<button className="btn btn-primary">View invoice</button>
								</div>
							</div>
						</div>
					</div>
				</div>
				<SectionContainer
					title="Billing details"
					description="Payment and billing information for this account">
					<div className="mb-2">
						<h2 className="text-xl font-semibold">Payment information</h2>
						<div className="flex flex-col sm:flex-row">
							<span>Payment Method</span>
							<span className="font-semibold">
								{null ?? 'No credit card on file'}
							</span>{' '}
							{/*Mastercard ending in xxxx */}
						</div>
						<button className="btn btn-ghost btn-outline">Change payment method</button>
					</div>

					<div>
						<h2 className="text-xl font-semibold">Billing information</h2>

						<div className="flex flex-col sm:flex-row">
							<span>Name:</span>
							<span className="font-semibold">{userConfig.name}</span>
						</div>

						<div className="flex flex-col sm:flex-row">
							<span>Email:</span>
							<span className="font-semibold">{userConfig.email}</span>
						</div>

						<div className="flex flex-col sm:flex-row">
							<span>Payment Method:</span>
							<span className="font-semibold">
								{null ?? 'No billing details set'}
							</span>
						</div>

						<button className="btn btn-ghost btn-outline">
							Update billing information
						</button>
					</div>
				</SectionContainer>
			</article>
		</MainContainer>
	);
};

export default Billing;
