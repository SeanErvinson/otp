import { useAtom } from 'jotai';
import { useResetAtom } from 'jotai/utils';
import { useEffect } from 'react';
import ReactDOM from 'react-dom';

import LoadingIndicator from '@/components/LoadingIndicator/LoadingIndicator';
import RefetchIcon from '@/components/misc/RefetchIcon';
import XIcon from '@/components/misc/XIcon';
import { dateFormat, dateTimeFormat, timelineTimeFormat } from '@/consts/date';
import { toLocalTime } from '@/utils/dayjsUtils';

import { logAtom } from '../states/LogAtom';
import { EventStatus } from '@/types/types';
import useOtpDetails from '../queries/useOtpDetails';

interface Props {
	showModal: boolean;
	onClose: () => void;
}

const SummaryInfo = ({
	className,
	title,
	value,
}: {
	className?: string;
	title: string;
	value?: string;
}) => {
	return (
		<div className={`my-2 ${className ?? ''}`}>
			<p>{title}</p>
			<p className="text-base-content">{value ? value : '\u2014'}</p>
		</div>
	);
};

const TimeEntryItem = ({
	className,
	date,
	title,
}: {
	className?: string;
	date: Date;
	title: string;
}) => {
	return (
		<li className={`text-base-content/70 my-1 ${className ?? ''}`}>
			{toLocalTime(date, timelineTimeFormat)} -{' '}
			<span className="text-base-content">{title}</span>
		</li>
	);
};

const TimelineModal = (props: Props) => {
	const [log] = useAtom(logAtom);
	const resetLog = useResetAtom(logAtom);

	const otpDetailQuery = useOtpDetails(log.id);

	const handleOnEscClick = (event: KeyboardEvent) => {
		if (event.key === 'Escape') {
			props.onClose();
		}
	};

	useEffect(() => {
		window.addEventListener('keydown', handleOnEscClick);

		return () => {
			window.removeEventListener('keydown', handleOnEscClick);
			console.log('Reset');
			resetLog();
		};
	}, []);

	const getEventInfo = (title: string, status: EventStatus, expand = false) => {
		if (!otpDetailQuery.data) return;
		const eventInfo = otpDetailQuery.data.timeline.find(event => event.state == status);
		console.log(eventInfo);

		return (
			<SummaryInfo
				className={expand ? 'col-span-2' : ''}
				title={title}
				value={eventInfo ? toLocalTime(eventInfo.occuredAt, dateTimeFormat) : ''}
			/>
		);
	};

	return ReactDOM.createPortal(
		<div
			id="timelineModal"
			className={`modal modal-bottom sm:modal-middle ${props.showModal && 'modal-open'}`}>
			<div className="modal-box sm:w-11/12 sm:max-w-4xl relative">
				<label
					onClick={props.onClose}
					className="btn btn-sm btn-circle absolute right-2 top-2">
					<XIcon className="h-[24px] fill-base-100" />
				</label>
				<div className="flex flex-row justify-between mb-2">
					<h2 className="text-base-content font-bold text-xl">Log details</h2>
					{!otpDetailQuery.isLoading && otpDetailQuery.isFetching && (
						<RefetchIcon className="stroke-current h-6 mr-4" />
					)}
				</div>
				{otpDetailQuery.isLoading && <LoadingIndicator />}
				{otpDetailQuery.data && (
					<div className="sm:grid sm:grid-cols-3">
						<div className="flex-1 flex flex-col col-span-2 sm:order-2">
							<h3 className="text-base-primary font-semibold">Summary</h3>
							<div className="rounded-lg p-4 bg-base-200 text-base-content/70 sm:grid sm:grid-cols-2 flex-1">
								<SummaryInfo title="RequestId" value={otpDetailQuery.data.id} />
								<SummaryInfo
									title="Recipient"
									value={otpDetailQuery.data.recipient}
								/>
								<SummaryInfo
									title="Requested"
									value={toLocalTime(
										otpDetailQuery.data.requestedAt,
										dateTimeFormat,
									)}
								/>
								{getEventInfo('Sent', EventStatus.send)}
								{getEventInfo('Delivered', EventStatus.deliver, true)}
								<SummaryInfo title="Status" value={'asdf'} />
								<SummaryInfo title="Retry" />
								<SummaryInfo
									title="User Ip Address"
									value={otpDetailQuery.data.clientInfo.ipAddress}
								/>
								<SummaryInfo
									title="Referrer"
									value={otpDetailQuery.data.clientInfo.referrer}
								/>
								<SummaryInfo
									className="col-span-2"
									title="User Agent"
									value={otpDetailQuery.data.clientInfo.userAgent}
								/>
							</div>
						</div>
						<div className="my-2">
							<div id="timeline" className="mb-4">
								<h2 className="text-base-content font-semibold">Timeline</h2>
								<h2 className="text-base-content/70 font-semibold">
									{toLocalTime(new Date(), dateFormat)}
								</h2>
								<ul id="timeline-entries">
									{otpDetailQuery.data.timeline.map(event => (
										<TimeEntryItem date={event.occuredAt} title={event.state} />
									))}
								</ul>
							</div>
							<div id="attempts">
								<h2 className="text-base-content font-semibold">Attempts</h2>
								<h2 className="text-base-content/70 font-semibold">
									{toLocalTime(new Date(), dateFormat)}
								</h2>
								<ul id="attempt-entries">
									{otpDetailQuery.data.attempts.map(attempt => (
										<TimeEntryItem
											date={attempt.attemptedOn}
											title={attempt.attemptStatus.toLocaleString()}
										/>
									))}
								</ul>
							</div>
						</div>
					</div>
				)}
			</div>
		</div>,
		document.getElementById('portal')!,
	);
};

export default TimelineModal;
