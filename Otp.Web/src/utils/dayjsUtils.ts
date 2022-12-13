import dayjs from 'dayjs';

export const toLocalTime = (date: string | number | Date | null | undefined, format: string) => {
	if (!date) return '';
	return dayjs.utc(date).local().format(format);
};
