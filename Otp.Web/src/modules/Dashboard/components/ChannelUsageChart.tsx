import dayjs from 'dayjs';
import { useState } from 'react';
import Chart from 'react-apexcharts';
import { useQuery } from 'react-query';

import { OtpApi } from '@/api/otpApi';
import { MetricInterval } from '@/types/types';
import PillRadioGroup from '@/components/PillRadioGroup/PillRadioGroup';
import DateUtils from '@/utils/dateUtils';

export interface ChannelUsageMetric {
	name: string;
	data: Data;
}

export interface Data {
	sms: Email[];
	email: Email[];
}

export interface Email {
	timeStamp: string;
	value: number;
}

type Interval = {
	name: string;
	calculate: (date: Date) => Date;
	interval: MetricInterval;
};

const intervals: Interval[] = [
	{
		name: '7 Days',
		calculate: d => DateUtils.subtractDays(7, d),
		interval: MetricInterval.Day,
	},
	{
		name: '14 Days',
		calculate: d => DateUtils.subtractDays(14, d),
		interval: MetricInterval.Day,
	},
	{
		name: '1 Month',
		calculate: d => DateUtils.subtractMonths(1, d),
		interval: MetricInterval.Month,
	},
	{
		name: '3 Month',
		calculate: d => DateUtils.subtractMonths(3, d),
		interval: MetricInterval.Month,
	},
	{
		name: 'YTD',
		calculate: d => DateUtils.startOfYear(d),
		interval: MetricInterval.Month,
	},
];

const intervalLabels = intervals.map(interval => interval.name);

const ChannelUsageChart = () => {
	const currentDate = new Date();
	const [selectedInterval, setSelectedInterval] = useState(intervals[0]);

	const channelCountMetricQuery = useQuery(
		['getChannelUsageCountMetric', selectedInterval],
		() =>
			OtpApi.getMetrics<ChannelUsageMetric>(
				'ChannelUsageCount',
				`${selectedInterval
					.calculate(new Date())
					.toISOString()}/${currentDate.toISOString()}`,
				selectedInterval.interval,
			),
		{
			refetchInterval: 60000,
			refetchOnWindowFocus: true,
		},
	);

	const handleOnChangeIntervalChange = (value: string) => {
		setSelectedInterval(intervals[parseInt(value)]);
	};

	const channelCountMetric = channelCountMetricQuery.data ?? undefined;

	const options: ApexCharts.ApexOptions = {
		chart: {
			id: 'bar',
			stacked: true,
			background: 'white',
			toolbar: {
				tools: {
					zoom: false,
					zoomin: false,
					zoomout: false,
					pan: false,
					reset: false,
					selection: false,
				},
			},
		},
		xaxis: {
			categories:
				selectedInterval?.interval === MetricInterval.Day
					? channelCountMetric?.data.email.map(c => dayjs(c.timeStamp).format('ddd')) ??
					  []
					: channelCountMetric?.data.email.map(c => dayjs(c.timeStamp).format('MMM')) ??
					  [],
		},
	};

	const series = [
		{
			name: 'SMS',
			data: channelCountMetric?.data.sms.map(d => d.value) ?? [],
		},
		{
			name: 'Email',
			data: channelCountMetric?.data.email.map(d => d.value) ?? [],
		},
	];

	return (
		<div className="flex flex-col gap-2">
			<PillRadioGroup
				name="interval"
				className="justify-center md:justify-end"
				initialData={'0'}
				onChange={handleOnChangeIntervalChange}
				selections={intervalLabels}
			/>
			<Chart options={options} series={series} type="bar" />
		</div>
	);
};

export default ChannelUsageChart;
