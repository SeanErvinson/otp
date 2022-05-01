import Layout from '@theme/Layout';
import React, { ReactNode, useState } from 'react';
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

const Pricing = () => {
	const [showSmsPriceTable, setShowSmsPriceTable] = useState(false);
	const [showEmailPriceTable, setShowEmailPriceTable] = useState(false);

	return (
		<Layout title="Pricing" description="Very simple and affordable">
			<div className="bg-white dark:bg-gray-900">
				<div className="container px-6 py-8 mx-auto">
					<div className="xl:items-center xl:-mx-8 xl:flex">
						<div className="flex flex-col items-center xl:items-start xl:mx-8">
							<h1 className="text-3xl font-medium text-gray-800 capitalize lg:text-4xl dark:text-white">
								Simple, transparent pricing
							</h1>

							<div className="mt-4">
								<span className="inline-block w-40 h-1 bg-blue-500 rounded-full"></span>
								<span className="inline-block w-3 h-1 mx-1 bg-blue-500 rounded-full"></span>
								<span className="inline-block w-1 h-1 bg-blue-500 rounded-full"></span>
							</div>

							<p className="mt-4 font-medium text-gray-500 dark:text-gray-300">
								Just a single plan. No Contracts. No surprise fees.
							</p>

							<p className="mt-4 font-medium text-gray-500 dark:text-gray-300">
								Gets cheaper the more you use it.
							</p>
						</div>

						<div className="flex-1 xl:mx-8">
							<div className="mt-8 space-y-8 md:-mx-4 md:flex md:items-center md:justify-center md:space-y-0 xl:mt-0">
								<div className="max-w-sm mx-auto border rounded-lg md:mx-4 dark:border-gray-700">
									<div className="p-6">
										<h1 className="text-xl font-medium text-gray-700 capitalize lg:text-3xl dark:text-white">
											Usage
										</h1>

										<p className="mt-4 text-gray-500 dark:text-gray-300">
											You're only billed what you use. Easily calculate using
											a{' '}
											<a
												href={`#pricing-calculator`}
												className="text-sm font-bold text-blue-500 dark:text-blue-400 hover:underline">
												pricing calculator
											</a>
										</p>

										<h2 className="mt-4 text-2xl font-medium text-gray-700 sm:text-4xl dark:text-gray-300">
											&#8369;50.00
										</h2>

										<p className="mt-1 text-gray-500 dark:text-gray-300">
											flat fee with up to 50 SMS and 100 emails+ onwards
										</p>

										<div className="mt-4 flex flex-row justify-between items-center">
											<h2 className="text-xl font-medium text-gray-700 sm:text-2xl dark:text-gray-300">
												+ excess SMS
												<span className="text-base font-medium"></span>
											</h2>

											<button
												onClick={() =>
													setShowSmsPriceTable(!showSmsPriceTable)
												}
												className="flex flex-row p-2 text-gray-700 bg-white border border-transparent rounded-md dark:text-white focus:border-blue-500 focus:ring-opacity-40 dark:focus:ring-opacity-40 focus:ring-blue-300 dark:focus:ring-blue-400 focus:ring dark:bg-gray-800 focus:outline-none">
												show price table
												<svg
													className="w-5 h-5 text-gray-800 dark:text-white"
													xmlns="http://www.w3.org/2000/svg"
													viewBox="0 0 20 20"
													fill="currentColor">
													<path
														fillRule="evenodd"
														d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
														clipRule="evenodd"
													/>
												</svg>
											</button>
										</div>
										{showSmsPriceTable && (
											<PricingTable
												channel="SMS"
												pricingTable={smsPricingTable}
											/>
										)}

										<div className="mt-4 flex flex-row justify-between items-center">
											<h2 className="text-xl font-medium text-gray-700 sm:text-2xl dark:text-gray-300">
												+ excess Email
												<span className="text-base font-medium"></span>
											</h2>

											<button
												onClick={() =>
													setShowEmailPriceTable(!showEmailPriceTable)
												}
												className="flex flex-row p-2 text-gray-700 bg-white border border-transparent rounded-md dark:text-white focus:border-blue-500 focus:ring-opacity-40 dark:focus:ring-opacity-40 focus:ring-blue-300 dark:focus:ring-blue-400 focus:ring dark:bg-gray-800 focus:outline-none">
												show price table
												<svg
													className="w-5 h-5 text-gray-800 dark:text-white"
													xmlns="http://www.w3.org/2000/svg"
													viewBox="0 0 20 20"
													fill="currentColor">
													<path
														fillRule="evenodd"
														d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
														clipRule="evenodd"
													/>
												</svg>
											</button>
										</div>
										{showEmailPriceTable && (
											<PricingTable
												channel="Email"
												pricingTable={emailPricingTable}
											/>
										)}
										<p className="mt-1 text-gray-500 dark:text-gray-300">
											Billed monthly
										</p>

										<button className="w-full px-4 py-2 mt-6 tracking-wide text-white capitalize transition-colors duration-200 transform bg-blue-600 rounded-md hover:bg-blue-500 focus:outline-none focus:bg-blue-500 focus:ring focus:ring-blue-300 focus:ring-opacity-80">
											Start Now
										</button>
									</div>

									<hr className="border-gray-200 dark:border-gray-700" />

									<div className="p-6">
										<h1 className="text-lg font-medium text-gray-700 capitalize lg:text-xl dark:text-white">
											What’s included:
										</h1>
										<div className="mt-8 space-y-4">
											<div className="flex items-center">
												<svg
													xmlns="http://www.w3.org/2000/svg"
													className="w-5 h-5 text-blue-500"
													viewBox="0 0 20 20"
													fill="currentColor">
													<path
														fillRule="evenodd"
														d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
														clipRule="evenodd"
													/>
												</svg>

												<span className="mx-4 text-gray-700 dark:text-gray-300">
													Reporting and Analytics
												</span>
											</div>

											<div className="flex items-center">
												<svg
													xmlns="http://www.w3.org/2000/svg"
													className="w-5 h-5 text-blue-500"
													viewBox="0 0 20 20"
													fill="currentColor">
													<path
														fillRule="evenodd"
														d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
														clipRule="evenodd"
													/>
												</svg>

												<span className="mx-4 text-gray-700 dark:text-gray-300">
													Own company branding
												</span>
											</div>

											<div className="flex items-center">
												<svg
													xmlns="http://www.w3.org/2000/svg"
													className="w-5 h-5 text-blue-500"
													viewBox="0 0 20 20"
													fill="currentColor">
													<path
														fillRule="evenodd"
														d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
														clipRule="evenodd"
													/>
												</svg>

												<span className="mx-4 text-gray-700 dark:text-gray-300">
													Downloadable logs
												</span>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>

			<PricingCalculator />
		</Layout>
	);
};

export default Pricing;
