import dayjs from 'dayjs';

export const toLocalTime = (date: string | number | Date, format: string) => {
	return dayjs.utc(date).local().format(format);
};
