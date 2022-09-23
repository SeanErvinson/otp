import { emailPricingTable, smsPricingTable } from '@site/src/types';
import React, { ChangeEvent, useState } from 'react';

const maxEmailCount = 100000;
const maxSmsCount = 10000;

const PricingCalculator = () => {
	const [excessSmsCount, setExcessSmsCount] = useState(0);
	const [excessEmailCount, setExcessEmailCount] = useState(0);

	const calculatePrice = (pricingTable: Record<number, number>, excess: number): number => {
		const sortedPricingTable = Object.entries(pricingTable).sort((a, b) => parseInt(a[0]) - parseInt(b[0]));
		let totalPrice = 0;
		for (let index = sortedPricingTable.length - 1; index >= 0; index--) {
			const [countLimit, multiplier] = sortedPricingTable[index];
			const limit = Number(countLimit);
			const diff = excess - limit;
			if (diff < 0) {
				continue;
			}
			totalPrice += multiplier * diff;
			excess = limit;
		}
		return totalPrice;
	};

	const calculateTotalPrice = (): Number => {
		const smsPrice = calculatePrice(smsPricingTable, excessSmsCount);
		const emailPrice = calculatePrice(emailPricingTable, excessEmailCount);
		const total = 50 + smsPrice + emailPrice;
		return Number(total.toFixed(2));
	};

	const handleOnChangeExcess = (
		event: ChangeEvent<HTMLInputElement>,
		setState: (value: number) => void,
		maxValue: number,
	) => {
		event.preventDefault();
		if (!event.target.value) {
			setState(0);
			return;
		}
		const parsedValue = parseInt(event.target.value);
		if (parsedValue > maxValue) {
			return;
		}
		setState(parsedValue);
	};

	return (
		<div
			id="pricing-calculator"
			className="max-w-xs mx-auto overflow-hidden bg-white border rounded-lg shadow-lg dark:bg-gray-800">
			<div className="p-6">
				<h2 className="text-xl mb-4 font-semibold text-gray-800 dark:text-white">Pricing Calculator</h2>
				<div>
					<div className="mb-3">
						<label htmlFor="smsCount" className="font-bold text-gray-600">
							Excess SMS
						</label>
						<div className="flex flex-col">
							<div className="flex flex-row justify-between">
								<span>0</span>
								<input
									type="number"
									name="manualSmsCount"
									min={0}
									max={maxSmsCount}
									value={excessSmsCount}
									onChange={e => handleOnChangeExcess(e, setExcessSmsCount, maxSmsCount)}
									className="appearance-none cursor-pointer text-right"
								/>
							</div>
							<input
								id="smsCount"
								type="range"
								min={0}
								max={maxSmsCount}
								value={excessSmsCount}
								onChange={e => handleOnChangeExcess(e, setExcessSmsCount, maxSmsCount)}
								className="w-full h-2 bg-rose-100 appearance-none"
							/>
						</div>
					</div>

					<div>
						<label htmlFor="emailCount" className="font-bold text-gray-600">
							Excess Emails
						</label>
						<div className="flex flex-col">
							<div className="flex flex-row justify-between">
								<span>0</span>
								<input
									type="number"
									name="manualEmailCount"
									min={0}
									max={maxEmailCount}
									value={excessEmailCount}
									onChange={e => handleOnChangeExcess(e, setExcessEmailCount, maxEmailCount)}
									className="appearance-none cursor-pointer text-right"
								/>
							</div>

							<input
								id="emailCount"
								type="range"
								min={0}
								max={maxEmailCount}
								value={excessEmailCount}
								onChange={e => handleOnChangeExcess(e, setExcessEmailCount, maxEmailCount)}
								className="w-full h-2 bg-rose-100 appearance-none"
							/>
						</div>
					</div>
				</div>
			</div>

			<div className="bg-gray-100">
				<h3 className="text-md font-semibold text-gray-800 dark:text-white text-center py-2">
					Transaction breakdown
				</h3>

				<div className="flex items-center justify-between max-w-2xl px-6 py-2 mx-auto">
					<div className="flex items-center">
						<div className="flex flex-col items-start space-y-1">
							<h2 className="text-md font-medium text-gray-700 dark:text-gray-200">Base</h2>
							<p className="text-xs text-gray-500 dark:text-gray-300">(Includes 50 SMS and 100 Email)</p>
						</div>
					</div>

					<div className="flex flex-col items-end">
						<h2 className="text-lg font-semibold">&#8369;50</h2>
					</div>
				</div>

				<div className="flex items-center justify-between max-w-2xl px-6 py-2 mx-auto">
					<div className="flex items-center">
						<div className="flex flex-col items-center space-y-1">
							<h2 className="text-md font-medium text-gray-700 dark:text-gray-200">Excess SMS</h2>
						</div>
					</div>

					<div className="flex items-end">
						<h2 className="text-lg font-semibold">
							&#8369;
							{calculatePrice(smsPricingTable, excessSmsCount).toLocaleString()}
						</h2>
					</div>
				</div>

				<div className="flex items-center justify-between max-w-2xl px-6 py-2 mx-auto">
					<div className="flex items-center">
						<div className="flex flex-col items-center space-y-1">
							<h2 className="text-md font-medium text-gray-700 dark:text-gray-200">Excess Email</h2>
						</div>
					</div>

					<div className="flex items-end">
						<h2 className="text-lg font-semibold">
							&#8369; {calculatePrice(emailPricingTable, excessEmailCount).toLocaleString()}
						</h2>
					</div>
				</div>

				<div className="px-4 py-2 bg-gray-900">
					<div className="flex items-center justify-between">
						<h4 className="text-lg font-bold text-white">Total Cost</h4>
						<h4 className="text-lg font-bold text-white">
							<span className="tracking-wider">
								<>&#8369;{calculateTotalPrice().toLocaleString()}</>
							</span>
							<span className="text-base font-medium">/Month</span>
						</h4>
					</div>

					<p className="mt-1 text-xs text-gray-300">
						Customer pays via credit or debit cards. All fees are VAT-inclusive.
					</p>
				</div>
			</div>
		</div>
	);
};

export default PricingCalculator;
