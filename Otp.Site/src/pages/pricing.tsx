import Layout from '@theme/Layout';
import React, { ReactNode, useState } from 'react';
import CheckMarkIcon from '../components/icons/CheckMarkIcon';
import DropDownIcon from '../components/icons/DropDownIcon';
import PricingCalculator from '../components/PricingCalculator';
import { emailPricingTable, smsPricingTable } from '../types';

interface Props {
	pricingTable: Record<number, number>;
	channel: string;
}

const PricingTable = (props: Props) => {
	const createRow = () => {
		const rows: ReactNode[] = [];
		const table = Object.entries(props.pricingTable);
		for (let index = 1; index < table.length; index++) {
			const [prevCountLimit, prevMultiplier] = table[index - 1];
			const [currCountLimit] = table[index];

			rows.push(
				<tr className="flex" key={index}>
					<td className="flex-1 border border-slate-300">
						{Number(prevCountLimit) + 1} - {Number(currCountLimit)}
					</td>
					<td className="flex-1 border border-slate-300">
						&#8369;{prevMultiplier}
						<span className="text-base font-medium">/{props.channel}</span>
					</td>
				</tr>,
			);
		}

		// rows.push(
		// 	<tr className="flex">
		// 		<td className="flex-1 border border-slate-300">
		// 			{Number(table[table.length - 1][0]) + 1}+
		// 		</td>
		// 		<td className="flex-1 border border-slate-300">Please contact us</td>
		// 	</tr>,
		// );
		return rows;
	};

	return (
		<table className="border-collapse border border-slate-400 bg-gray-100">
			<tbody className="flex flex-col">{createRow()}</tbody>
		</table>
	);
};

const IncludedList = () => {
	const features = ['Reporting and Analytics', 'Own company branding', 'Downloadable logs', 'And much more!'];

	return (
		<div className="p-6">
			<h1 className="text-lg font-medium text-gray-700 capitalize lg:text-xl dark:text-white">
				What’s included:
			</h1>
			<div className="mt-8 space-y-4">
				{features.map((feature, index) => (
					<div className="flex items-center" key={index}>
						<CheckMarkIcon className="h-6 stroke-rose-600" />

						<span className="mx-4 text-gray-700 dark:text-gray-300">{feature} </span>
					</div>
				))}
			</div>
		</div>
	);
};

const Usage = () => {
	const [showSmsPriceTable, setShowSmsPriceTable] = useState(false);
	const [showEmailPriceTable, setShowEmailPriceTable] = useState(false);

	return (
		<div className="max-w-sm mx-auto border rounded-lg md:mx-4 dark:border-gray-700">
			<div className="p-6">
				<h1 className="text-xl font-medium text-gray-700 capitalize lg:text-3xl dark:text-white">Usage</h1>

				<p className="mt-4 text-gray-500 dark:text-gray-300">
					You're only billed what you use. Estimate your expected monthly costs —{' '}
					<a
						href={`#pricing-calculator`}
						className="text-sm font-bold text-rose-500 dark:text-rose-400 hover:underline">
						pricing calculator
					</a>
				</p>

				<h2 className="mt-4 text-2xl font-medium text-gray-700 sm:text-4xl dark:text-gray-300">&#8369;50.00</h2>

				<p className="mt-1 text-gray-500 dark:text-gray-300">
					flat fee with up to 50 SMS and 100 emails+ onwards
				</p>

				<div className="mt-4 flex flex-row justify-between items-center">
					<h2 className="text-xl font-medium text-gray-700 sm:text-2xl dark:text-gray-300">
						+ excess SMS
						<span className="text-base font-medium"></span>
					</h2>

					<button
						onClick={() => setShowSmsPriceTable(!showSmsPriceTable)}
						className="flex flex-row p-2 text-gray-700 bg-white border border-transparent rounded-md dark:text-white focus:border-rose-500 focus:ring-opacity-40 dark:focus:ring-opacity-40 focus:ring-rose-300 dark:focus:ring-rose-400 focus:ring dark:bg-gray-800 focus:outline-none">
						show price table
						<DropDownIcon className="w-5 h-5 text-gray-800 dark:text-white" />
					</button>
				</div>
				{showSmsPriceTable && <PricingTable channel="SMS" pricingTable={smsPricingTable} />}

				<div className="mt-4 flex flex-row justify-between items-center">
					<h2 className="text-xl font-medium text-gray-700 sm:text-2xl dark:text-gray-300">
						+ excess Email
						<span className="text-base font-medium"></span>
					</h2>

					<button
						onClick={() => setShowEmailPriceTable(!showEmailPriceTable)}
						className="flex flex-row p-2 text-gray-700 bg-white border border-transparent rounded-md dark:text-white focus:border-rose-500 focus:ring-opacity-40 dark:focus:ring-opacity-40 focus:ring-rose-300 dark:focus:ring-rose-400 focus:ring dark:bg-gray-800 focus:outline-none">
						show price table
						<DropDownIcon className="w-5 h-5 text-gray-800 dark:text-white" />
					</button>
				</div>
				{showEmailPriceTable && <PricingTable channel="Email" pricingTable={emailPricingTable} />}
				<p className="mt-1 text-gray-500 dark:text-gray-300">Billed monthly</p>

				<button className="w-full px-4 py-2 mt-6 tracking-wide text-white capitalize transition-colors duration-200 transform bg-rose-600 rounded-md hover:bg-rose-500 focus:outline-none focus:bg-rose-500 focus:ring focus:ring-rose-300 focus:ring-opacity-80">
					Start Now
				</button>
			</div>

			<hr className="border-gray-200 dark:border-gray-700" />

			<IncludedList />
		</div>
	);
};

const Pricing = () => {
	return (
		<Layout title="Pricing" description="Very simple and affordable">
			<main className="bg-white dark:bg-gray-900">
				<section className="container flex flex-col items-center px-4 py-12 mx-auto xl:flex-row">
					<div className="flex flex-col items-center mt-6 xl:items-end xl:w-1/2 xl:mt-0">
						<h1 className="text-3xl font-bold tracking-tight text-gray-800 xl:text-4xl dark:text-white">
							Simple, transparent pricing
						</h1>
						<div className="mt-4">
							<span className="inline-block w-40 h-1 bg-rose-500 rounded-full"></span>
							<span className="inline-block w-3 h-1 mx-1 bg-rose-500 rounded-full"></span>
							<span className="inline-block w-1 h-1 bg-rose-500 rounded-full"></span>
						</div>

						<p className="mt-4 font-medium text-gray-500 dark:text-gray-300">
							Just a single plan. No Contracts. No surprise fees.
						</p>

						<p className="mt-4 font-medium text-gray-500 dark:text-gray-300">
							Gets cheaper the more you use it.
						</p>
					</div>
					<div className="flex justify-center xl:w-1/2">
						<Usage />
					</div>
				</section>

				<section className="container flex flex-col items-center px-4 py-12 mx-auto xl:flex-row">
					<div className="flex justify-center xl:w-1/2">
						<PricingCalculator />
					</div>

					<div className="flex flex-col items-center mt-6 xl:items-start xl:w-1/2 xl:mt-0">
						<h2 className="text-3xl font-bold tracking-tight text-gray-800 xl:text-4xl dark:text-white">
							Estimate your costs
						</h2>
						<div className="mt-4">
							<span className="inline-block w-40 h-1 bg-rose-500 rounded-full"></span>
							<span className="inline-block w-3 h-1 mx-1 bg-rose-500 rounded-full"></span>
							<span className="inline-block w-1 h-1 bg-rose-500 rounded-full"></span>
						</div>

						<p className="mt-4 font-medium text-gray-500 dark:text-gray-300">
							Estimate your expected monthly costs for using any combination of channels.
						</p>
					</div>
				</section>
			</main>
		</Layout>
	);
};

export default Pricing;
