import { useState } from 'react';
import DatePicker, { CalendarContainer } from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import './lastSpanSelect.less';

import RightArrowIcon from '@/components/misc/RightArrowIcon';

interface Props {
	onChange: (value: number) => void;
}

const LastSpanSelect = (props: Props) => {
	const currentDate = new Date();

	const [startDate, setStartDate] = useState<Date>(currentDate);
	const [endDate, setEndDate] = useState<Date>(currentDate);

	const onDatePickerChange = () => {
		const utc1 = Date.UTC(startDate.getFullYear(), startDate.getMonth(), startDate.getDate());
		const utc2 = Date.UTC(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
		onChange(Math.floor((utc2 - utc1) / (1000 * 60 * 60 * 24)));
	};

	const onChangeStartDate = (date: Date | null) => {
		setStartDate(date ?? currentDate);
	};

	const onChangeEndDate = (date: Date | null) => {
		setEndDate(date ?? currentDate);
		onDatePickerChange();
	};

	const onClickDatePreset = (offsetBy: number) => {
		const date = subtractDays(offsetBy);
		setStartDate(date);
		setEndDate(currentDate);
		onDatePickerChange();
	};

	const onChange = (value: number) => {
		props.onChange(value);
	};

	const subtractDays = (offset: number) => {
		const date = new Date();
		date.setDate(date.getDate() - offset);
		return date;
	};

	const Selectors = ({ children }: { children: React.ReactNode[] }) => {
		return (
			<div className="flex flex-row gap-2 pl-2 border rounded-md h-[268px]">
				<div className="flex flex-col justify-center">
					<button onClick={() => onClickDatePreset(0)} className="btn btn-ghost">
						Today
					</button>
					<button onClick={() => onClickDatePreset(7)} className="btn btn-ghost">
						Last 7 days
					</button>
					<button onClick={() => onClickDatePreset(14)} className="btn btn-ghost">
						Last 14 days
					</button>
					<button onClick={() => onClickDatePreset(30)} className="btn btn-ghost">
						Last 30 days
					</button>
				</div>
				<div className="my-2 border border-solid border-slate-300"></div>
				<CalendarContainer>
					<div style={{ position: 'relative' }}>{children}</div>
				</CalendarContainer>
			</div>
		);
	};

	return (
		<>
			<div className="flex flex-row items-center">
				<DatePicker
					className="select select-bordered rounded-l-lg rounded-r-none w-full max-w-xs"
					selected={startDate}
					onChange={onChangeStartDate}
					selectsStart
					minDate={subtractDays(90)}
					maxDate={currentDate}
					startDate={startDate}
					endDate={endDate}
					calendarContainer={Selectors}
				/>
				<div className="bg-base-300 self-stretch flex items-center p-2">
					<RightArrowIcon />
				</div>
				<DatePicker
					className="select select-bordered rounded-r-lg rounded-l-none w-full max-w-xs"
					selected={endDate}
					onChange={onChangeEndDate}
					selectsEnd
					maxDate={currentDate}
					startDate={startDate}
					endDate={endDate}
					minDate={startDate}
					calendarContainer={Selectors}
				/>
			</div>
		</>
	);
};

export default LastSpanSelect;
