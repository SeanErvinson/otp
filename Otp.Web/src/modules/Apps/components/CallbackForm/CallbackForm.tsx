import { yupResolver } from '@hookform/resolvers/yup';
import { useEffect, useState } from 'react';
import { useForm, useFormState } from 'react-hook-form';

import WarningIcon from '@/components/misc/WarningIcon';
import useGeneratedId from '@/hooks/useGeneratedId';
import { AppDetail } from '@/types/types';

import useSaveAppCallback from '../../mutations/useSaveAppCallback';
import validationSchema from './validationSchema';

type FormData = {
	callbackUrl: string;
	secret: string;
};

interface Props {
	appDetail: AppDetail;
}

const CallbackForm = (props: Props) => {
	const generateId = useGeneratedId();
	const [enableChangeSecret, setEnableChangeSecret] = useState(false);

	const { handleSubmit, register, resetField, control, getValues, clearErrors, reset } =
		useForm<FormData>({
			defaultValues: {
				callbackUrl: props.appDetail.callbackUrl ?? '',
				secret: '',
			},
			mode: 'onChange' || 'onSubmit',
			resolver: yupResolver(validationSchema),
		});
	const { isDirty, errors, isSubmitting, isValid } = useFormState<FormData>({
		control,
	});

	useEffect(() => {
		if (!isDirty) {
			reset();
		}
	}, [isDirty]);

	const mutation = useSaveAppCallback();

	const onSubmit = (data: FormData) => {
		mutation.mutate(
			{
				appId: props.appDetail.id,
				callbackUrl: data.callbackUrl,
				endpointSecret: data.secret,
			},
			{
				onSuccess: () => {
					resetField('secret');
					resetField('callbackUrl', {
						keepDirty: false,
						defaultValue: getValues('callbackUrl'),
					});
					setEnableChangeSecret(false);
				},
			},
		);
	};

	const onCancelChangeSecret = () => {
		resetField('secret');
		setEnableChangeSecret(!enableChangeSecret);
	};

	return (
		<form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-2">
			<div className="form-control">
				<label htmlFor={generateId('callback')} className="label">
					<span className="label-text">Callback URL</span>
				</label>
				<input
					id={generateId('callback')}
					type="url"
					{...register('callbackUrl')}
					className={`input input-bordered ${errors.callbackUrl && 'input-error'}`}
				/>

				{errors.callbackUrl && (
					<label className="label">
						<span className="label-text-alt text-error">
							{errors.callbackUrl.message}
						</span>
					</label>
				)}
			</div>
			<div className="form-control">
				<label htmlFor={generateId('secret')} className="label">
					<span className="label-text">Secret</span>
				</label>
				{!!props.appDetail.callbackUrl && !enableChangeSecret ? (
					<div className="alert-warning rounded-2xl p-4 flex flex-row justify-between items-center gap-4">
						<WarningIcon className="w-6 h-6 mx-1 fill-current" />
						<p className="flex-1">
							If you've lost or forgotten this secret, you can change it, but be aware
							that any integrations using this secret will need to be updated. —
							<span>&nbsp;</span>
							<span className="link" onClick={() => setEnableChangeSecret(true)}>
								Change Secret
							</span>
						</p>
					</div>
				) : (
					<div className="flex space-x-2">
						<input
							id={generateId('secret')}
							type="text"
							className="input input-bordered"
							{...register('secret')}
						/>
						{enableChangeSecret ? (
							<button
								className="btn btn-ghost"
								type="button"
								onClick={onCancelChangeSecret}>
								Cancel
							</button>
						) : (
							<></>
						)}
					</div>
				)}

				{errors.secret && (
					<label className="label">
						<span className="label-text-alt text-error">{errors.secret.message}</span>
					</label>
				)}
			</div>

			{isDirty && (
				<div className="mt-2">
					<button
						className={`btn btn-success ${mutation.isLoading && 'loading'}`}
						disabled={mutation.isLoading || !isValid || isSubmitting}
						type="submit">
						{!mutation.isLoading ? 'Save' : 'Saving'}
					</button>
				</div>
			)}
		</form>
	);
};

export default CallbackForm;
