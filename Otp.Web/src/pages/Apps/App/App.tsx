import { useEffect, useMemo, useState } from 'react';
import { useQuery } from 'react-query';
import { NavLink, Outlet, useParams } from 'react-router-dom';

import { getApp } from '@/api/otpApi';
import { TagInput } from '@/components/TagInput';
import Spinner from '@/components/misc/Spinner';
import XIcon from '@/components/misc/XIcon';

import DeleteAppButton from './DeleteAppButton';
import { TagCollection } from '@/components/TagCollection';
import { useForm, useFormState } from 'react-hook-form';

type FormData = {
	name: string;
	description: string;
	tags: string[];
};

const App = () => {
	const params = useParams();
	const query = useQuery(['getApp', params.appId], () => getApp(params.appId ?? ''));

	const { handleSubmit, register, control, resetField, setValue, reset } = useForm<FormData>({
		defaultValues: {
			name: query.data?.name,
			description: query.data?.description,
			tags: query.data?.tags,
		},
	});

	const { isDirty, dirtyFields } = useFormState<FormData>({ control });
	const [showTagInput, setShowTagInput] = useState(false);
	const [tags, setTags] = useState<string[]>([]);

	const test = (values: string[]) => {
		if (values.length <= 0) return;
		setTags(values);
		setValue('tags', values, { shouldDirty: true });
	};

	const test2 = () => {
		setShowTagInput(!showTagInput);
		resetField('tags');
	};

	useEffect(() => {
		reset({
			name: query.data?.name,
			description: query.data?.description,
			tags: query.data?.tags,
		});
	}, [reset, query.isSuccess]);

	const onSubmit = (data: FormData) => {
		console.log(data);
	};

	return (
		<main id="app" className="h-full w-4/5 mx-auto pt-5">
			{query.isLoading && (
				<div className="flex flex-col gap-3 items-center h-full w-full justify-center">
					<Spinner />
				</div>
			)}
			{query.isSuccess && (
				<>
					<div className="flex flex-row justify-between mb-4">
						<form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-2">
							<div className="form-control flex flex-row items-center gap-4">
								<input
									type="text"
									defaultValue={query.data?.name}
									className="input text-3xl font-bold bg-base-200 focus:bg-base-100 px-0 cursor-text"
									{...register('name', {
										required: true,
										pattern: {
											value: /[\w-]/,
											message:
												'Name should only contain alphanumeric, -, or _',
										},
									})}
								/>
								{dirtyFields.name && (
									<button
										onClick={() => resetField('name')}
										className="btn btn-outline btn-circle btn-xs">
										<XIcon />
									</button>
								)}
							</div>
							<div className="form-control flex flex-row items-center gap-4">
								<input
									type="text"
									defaultValue={query.data?.description}
									{...register('description', {
										maxLength: 128,
									})}
									className="input input-sm text-xl font-semibold bg-base-200 focus:bg-base-100 px-0 cursor-text"
								/>
								{dirtyFields.description && (
									<button
										onClick={() => resetField('description')}
										className="btn btn-outline btn-circle btn-xs">
										<XIcon />
									</button>
								)}
							</div>

							{!showTagInput ? (
								<div className="flex flex-row">
									{query.data?.tags && (
										<TagCollection tags={query.data.tags} tagCount={5} />
									)}
									<div
										className="badge badge-outline border-dashed text-sm cursor-pointer hover:bg-base-300"
										onClick={() => setShowTagInput(!showTagInput)}>
										+ Add tags
									</div>
								</div>
							) : (
								<div className="flex flex-row gap-2 items-center">
									<TagInput onUpdate={test} initialTags={query.data?.tags} />

									<button
										className="btn btn-outline btn-xs border-dashed"
										onClick={test2}>
										<XIcon />
										Cancel
									</button>
								</div>
							)}
							{isDirty && (
								<button className="btn btn-success" type="submit">
									Update App
								</button>
							)}
						</form>
						<DeleteAppButton appId={params.appId ?? ''} />
					</div>

					<div>
						<div className="tabs">
							<NavLink
								to=""
								end
								className={({ isActive }) =>
									`tab tab-lifted ${isActive ? 'tab-active' : ''}`
								}>
								Settings
							</NavLink>
						</div>

						<Outlet context={query.data} />
					</div>
				</>
			)}
		</main>
	);
};

export default App;
