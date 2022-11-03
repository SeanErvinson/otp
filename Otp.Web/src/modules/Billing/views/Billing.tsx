import MainContainer from '@/components/MainContainer/MainContainer';
import PageHeader from '@/components/PageHeader/PageHeader';

const RemainingCredit = ({ credit }: { credit: string }) => {
	return (
		<div className="stats bg-primary text-primary-content">
			<div className="stat">
				<div className="stat-title">Remaining credit</div>
				<div className="stat-value">{credit}</div>
				<div className="stat-actions">
					<button className="btn btn-sm btn-success">Add credit</button>
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
	return (
		<MainContainer id="billing">
			<PageHeader title="Billing" />
			<article>
				<div>
					Current plan: {'Something'}
					<button className="btn btn-primary">Button</button>
				</div>
			</article>
		</MainContainer>
	);
};

export default Billing;
