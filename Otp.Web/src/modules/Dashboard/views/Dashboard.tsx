import { useQuery } from 'react-query';

import { OtpApi } from '@/api/otpApi';
import SingleStat from '@/components/SIngleStat/SingleStat';
import PageHeader from '@/components/PageHeader/PageHeader';
import { CountMetric } from '@/types/types';
import DateUtils from '@/utils/dateUtils';

import ChannelUsageChart from '../components/ChannelUsageChart';

const refreshInterval = 10000;

const Dashboard = () => {
	const currentDate = new Date();
	const startMonthDate = DateUtils.startOfMonth(currentDate);
	const endMonthDate = DateUtils.endOfMonth(currentDate);

	const smsCountMetricQuery = useQuery(
		['getSmsCountMetric'],
		() =>
			OtpApi.getMetrics<CountMetric>(
				'SmsUsageCount',
				`${startMonthDate.toISOString()}/${endMonthDate.toISOString()}`,
			),
		{
			refetchInterval: refreshInterval,
			refetchOnWindowFocus: true,
		},
	);

	const emailCountMetricQuery = useQuery(
		['getEmailCountMetric'],
		() =>
			OtpApi.getMetrics<CountMetric>(
				'EmailUsageCount',
				`${startMonthDate.toISOString()}/${endMonthDate.toISOString()}`,
			),
		{
			refetchInterval: refreshInterval,
			refetchOnWindowFocus: true,
		},
	);

	const totalSmsRequest = smsCountMetricQuery.data?.data.sentRequest;
	const totalPrevSmsRequest = smsCountMetricQuery.data?.data.previousMonthSentRequest;

	const totalEmailRequest = emailCountMetricQuery.data?.data.sentRequest;
	const totalPrevEmailRequest = emailCountMetricQuery.data?.data.previousMonthSentRequest;

	return (
		<main id="home" className="h-full mx-auto">
			<PageHeader title="Dashboard" />
			<article className="flex flex-col gap-4">
				<div className="flex flex-col md:flex-row gap-6">
					<SingleStat
						title="SMS sent this month"
						value={totalSmsRequest?.toString() ?? '0'}
						description={
							totalPrevSmsRequest && totalSmsRequest
								? `${totalPrevSmsRequest / totalSmsRequest}% more than last month`
								: null
						}
					/>

					<SingleStat
						title="Email sent this month"
						value={totalEmailRequest?.toString() ?? '0'}
						description={
							totalPrevEmailRequest && totalEmailRequest
								? `${
										totalPrevEmailRequest / totalEmailRequest
								  }% more than last month`
								: null
						}
					/>
				</div>
				<ChannelUsageChart />
			</article>
		</main>
	);
};

export default Dashboard;
