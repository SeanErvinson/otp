import { useEffect, useState } from 'react';
import { useQuery } from 'react-query';
import { NavLink, Outlet, useParams } from 'react-router-dom';

import { getApp } from '@/api/otpApi';
import { TagInput } from '@/components/TagInput';
import SpinnerIcon from '@/components/misc/SpinnerIcon';
import XIcon from '@/components/misc/XIcon';

import DeleteAppButton from './DeleteAppButton';
import { TagCollection } from '@/components/TagCollection';
import { useForm, useFormState } from 'react-hook-form';

type FormData = {
	name: string;
	description: string;
	tags?: string[];
};

const App = () => {
	const params = useParams();
	const appId = params.appId;

	const query = useQuery(['getApp', appId], () => getApp(appId), {
		enabled: !!appId,
	});

	const { handleSubmit, register, control, resetField, setValue, reset, getValues } =
		useForm<FormData>({
			defaultValues: {
				name: query.data?.name,
				description: query.data?.description,
				tags: query.data?.tags,
			},
		});

	const { isDirty, dirtyFields } = useFormState<FormData>({ control });
	const [showTagInput, setShowTagInput] = useState(false);

	const handleOnTagInput = (values: string[]) => {
		if (values.length <= 0) return;
		setValue('tags', values, {
			shouldDirty: true,
			shouldTouch: true,
			shouldValidate: true,
		});
	};

	const handleOnTagCancel = () => {
		reset({
			tags: query.data?.tags,
		});
		setShowTagInput(!showTagInput);
	};

	useEffect(() => {
		register('tags');
	}, []);

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
		<main id="app" className="h-full mx-auto">
			{query.isLoading && (
				<div className="flex flex-col gap-3 items-center h-full w-full justify-center">
					<SpinnerIcon />
				</div>
			)}
			{query.isSuccess && (
				<>
					<div className="flex flex-row justify-between mb-4">
						<form
							onSubmit={handleSubmit(onSubmit)}
							className="flex flex-1 flex-col gap-2 w-full">
							<div className="flex flex-row justify-between">
								<div className="form-control flex flex-row items-center gap-4">
									<input
										type="text"
										defaultValue={query.data?.name}
										className="input text-3xl font-bold bg-base-200 focus:bg-base-100 px-0 cursor-text w-4/5"
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
								<DeleteAppButton appId={params.appId ?? ''} />
							</div>
							<div className="form-control flex flex-row items-center gap-4">
								<input
									type="text"
									defaultValue={query.data?.description}
									{...register('description', {
										maxLength: 128,
									})}
									className="input input-sm text-xl font-semibold bg-base-200 focus:bg-base-100 px-0 cursor-text w-4/5"
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
								<div className="flex flex-row items-center">
									<div className="w-9/12 md:w-auto">
										{query.data?.tags && (
											<TagCollection tags={query.data.tags} tagCount={5} />
										)}
									</div>
									<div
										className="flex-3 badge badge-outline border-dashed text-sm cursor-pointer hover:bg-base-300 ml-1 whitespace-nowrap"
										onClick={() => setShowTagInput(!showTagInput)}>
										+ Add tags
									</div>
								</div>
							) : (
								<div className="flex flex-row gap-2 items-center lg:w-full">
									<TagInput
										onUpdate={handleOnTagInput}
										initialTags={query.data?.tags}
									/>

									<button
										className="btn btn-outline btn-xs border-dashed"
										onClick={handleOnTagCancel}>
										<XIcon />
										Cancel
									</button>
								</div>
							)}
							{isDirty && (
								<div>
									<button className="btn btn-sm btn-success" type="submit">
										Save
									</button>
								</div>
							)}
						</form>
					</div>

					<div>
						<div className="tabs">
							<NavLink
								to=""
								end
								className={({ isActive }) =>
									`tab tab-lifted ${isActive && 'tab-active'}`
								}>
								Settings
							</NavLink>
							{query.data?.callbackUrl && (
								<NavLink
									to="recent-callbacks"
									end
									className={({ isActive }) =>
										`tab tab-lifted ${isActive && 'tab-active'}`
									}>
									Recent Callbacks
								</NavLink>
							)}
						</div>

						<Outlet context={query.data} />
					</div>
				</>
			)}
		</main>
	);
};

export default App;
