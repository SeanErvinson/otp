import { object, string } from 'yup';

const validationSchema = object({
	name: string()
		.required('Please enter a name')
		.min(4, 'Name must be longer than 4 characters')
		.max(32, 'Name must be shorter than 32 characters')
		.matches(/^[\w-]+$/, 'Name should only contain alphanumeric, -, or _'),
	description: string().max(128, 'Description should be shorter than 128 characters'),
});

export default validationSchema;
