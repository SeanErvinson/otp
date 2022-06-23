import { useState } from 'react';
import { useForm, useFormState } from 'react-hook-form';

import WarningIcon from '@/components/misc/WarningIcon';
import { useGeneratedId } from '@/hooks';
import { useMutation } from 'react-query';
import { OtpApi } from '@/api/otpApi';
import { isValidUrl } from '@/utils/stringUtils';

interface Props {
	appId: string;
	callbackUrl?: string;
}

type FormData = {
	callbackUrl: string;
	secret: string;
};

const CallbackSection = ({ appId, callbackUrl }: Props) => {
	const [enableChangeSecret, setEnableChangeSecret] = useState(false);
	const [isLoading, setIsLoading] = useState(false);
	const [isSuccess, setIsSuccess] = useState(false);

	const { handleSubmit, register, resetField, control, getValues } = useForm<FormData>({
		defaultValues: {
			callbackUrl: callbackUrl ?? '',
			secret: '',
		},
	});
	const { isDirty, errors, dirtyFields } = useFormState<FormData>({
		control,
	});
	const mutation = useMutation(OtpApi.updateAppCallback, {
		onMutate: () => {
			setIsLoading(true);
		},
		onSuccess: () => {
			resetField('secret');
			resetField('callbackUrl', {
				keepDirty: false,
				defaultValue: getValues('callbackUrl'),
			});
			setIsLoading(false);
			setIsSuccess(true);
			setEnableChangeSecret(false);
		},
	});
	const generateId = useGeneratedId();

	const onSubmit = (data: FormData) => {
		mutation.mutate({
			id: appId,
			callbackUrl: data.callbackUrl,
			endpointSecret: data.secret,
		});
	};

	const onCancelChangeSecret = () => {
		resetField('secret');
		setEnableChangeSecret(!enableChangeSecret);
	};

	return (
		<div className="max-w-lg mb-2">
			<form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-2">
				<div className="form-control">
					<label htmlFor={generateId('callback')} className="label">
						<span className="label-text">Callback URL</span>
					</label>
					<input
						id={generateId('callback')}
						type="url"
						defaultValue={callbackUrl}
						{...register('callbackUrl', {
							validate: value =>
								isValidUrl(value) || 'This doesn’t look like a valid URL.',
						})}
						className={`input ${errors.callbackUrl && 'input-error'}`}
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
					{!!callbackUrl && !enableChangeSecret ? (
						<div className="alert-warning rounded-2xl p-4 flex flex-row justify-between items-center gap-4">
							<WarningIcon className="w-6 h-6 mx-1 fill-current" />
							<p className="flex-1">
								If you've lost or forgotten this secret, you can change it, but be
								aware that any integrations using this secret will need to be
								updated. —<span>&nbsp;</span>
								<span className="link" onClick={() => setEnableChangeSecret(true)}>
									Change Secret
								</span>
							</p>
						</div>
					) : (
						<div className="flex space-x-2">
							<input
								type="text"
								id={generateId('secret')}
								className="input"
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
				</div>

				{isDirty && (
					<div>
						<button
							className={`btn btn-sm btn-success ${isLoading && 'loading'}`}
							disabled={isLoading ? true : false}
							type="submit">
							{!isLoading ? 'Save' : 'Saving'}
						</button>
					</div>
				)}
			</form>
		</div>
	);
};

export default CallbackSection;
