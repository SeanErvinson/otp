import { isValidUrl } from '@/utils/stringUtils';
import { object, string } from 'yup';

const validationSchema = object({
	callbackUrl: string().test('isValidUrl', 'This doesn’t look like a valid URL.', value =>
		isValidUrl(value || ''),
	),
	secret: string().notRequired().max(128, 'Secret should be shorter than 128 characters'),
});

export default validationSchema;
