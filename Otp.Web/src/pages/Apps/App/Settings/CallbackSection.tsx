import { useState } from 'react';
import { useForm, useFormState } from 'react-hook-form';

import WarningIcon from '@/components/misc/WarningIcon';
import { useGeneratedId } from '@/hooks';
import { useMutation } from 'react-query';
import { updateAppCallback } from '@/api/otpApi';
import { isValidUrl } from '@/utils/stringUtils';

interface Props {
	appId: string;
	callbackUrl: string;
}

type FormData = {
	callbackUrl: string;
	secret: string;
};

const CallbackSection = ({ appId, callbackUrl }: Props) => {
	const [enableChangeSecret, setEnableChangeSecret] = useState(false);
	const [isLoading, setIsLoading] = useState(false);
	const [isSuccess, setIsSuccess] = useState(false);
	const mutation = useMutation(updateAppCallback, {
		onMutate: () => {
			setIsLoading(true);
		},
		onSuccess: () => {
			resetField('secret');
			setIsLoading(false);
			setIsSuccess(true);
			setEnableChangeSecret(false);
		},
	});
	const { handleSubmit, register, reset, resetField, control } = useForm<FormData>({
		defaultValues: {
			callbackUrl: callbackUrl ?? '',
			secret: '',
		},
	});
	const { isDirty, errors } = useFormState<FormData>({
		control,
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
		<div className="w-1/2 mb-2">
			<form onSubmit={handleSubmit(onSubmit)}>
				<div className="form-control">
					<label htmlFor={generateId('callback')} className="label">
						<span className="label-text">Callback URL</span>
					</label>
					<input
						id={generateId('callback')}
						type="text"
						defaultValue={callbackUrl}
						{...register('callbackUrl', {
							validate: value =>
								isValidUrl(value) || 'This doesn’t look like a valid URL.',
						})}
						className={`input ${errors.callbackUrl ? 'input-error' : ''}`}
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
					{!callbackUrl || enableChangeSecret ? (
						<>
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
						</>
					) : (
						<div className="alert alert-warning">
							<div className="flex-1">
								<WarningIcon />
								<p>
									If you've lost or forgotten this secret, you can change it, but
									be aware that any integrations using this secret will need to be
									updated. —<span>&nbsp;</span>
									<span
										className="link"
										onClick={() => setEnableChangeSecret(!enableChangeSecret)}>
										Change Secret
									</span>
								</p>
							</div>
						</div>
					)}
				</div>

				{isDirty && (
					<button
						className={`btn btn-success ${isLoading ? 'loading' : ''}`}
						disabled={isLoading ? true : false}
						type="submit">
						{!isLoading ? 'Save' : 'Saving'}
					</button>
				)}
			</form>
		</div>
	);
};

export default CallbackSection;
