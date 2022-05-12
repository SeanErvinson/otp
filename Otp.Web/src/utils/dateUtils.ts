export default class DateUtils {
	static subtractMonths = (numberOfMonths: number, date = new Date()) => {
		date.setMonth(date.getMonth() - numberOfMonths);
		return date;
	};
	static subtractDays = (numberOfDays: number, date = new Date()) => {
		date.setDate(date.getDate() - numberOfDays);
		return date;
	};
	static startOfYear = (currentDate = new Date()) => {
		return new Date(currentDate.getFullYear(), 0, 0);
	};
	static startOfMonth = (currentDate = new Date()) => {
		return new Date(currentDate.getFullYear(), currentDate.getMonth(), 0);
	};
	static endOfMonth = (currentDate = new Date()) => {
		return new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
	};
	static dateToEpoch = (date = new Date()) => {
		return date.valueOf();
	};
	static epochToDate = (epochTime: number) => {
		return new Date(epochTime);
	};
}
