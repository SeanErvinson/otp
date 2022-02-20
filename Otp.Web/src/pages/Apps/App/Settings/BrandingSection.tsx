import { ChangeEvent, useState } from 'react';
import { useOutletContext } from 'react-router-dom';

import { AppDetail } from '@/common/types';

const BrandingSection = () => {
	const app = useOutletContext<AppDetail | null>();

	const [selectedBackground, setSelectedBackgroundBlob] = useState<File | null>();
	const [currentBackgroundBlob, setCurrentBackgroundBlob] = useState<string | undefined>(
		app?.backgroundUrl,
	);
	const [selectedLogoBlob, setSelectedLogoBlob] = useState<File | null>();
	const [currentLogoBlob, setCurrentLogoBlob] = useState<string | undefined>(app?.logoUrl);

	const handleOnChange = async (event: ChangeEvent<HTMLInputElement>) => {
		event.preventDefault();
		if (event.target.files) {
			const file = event.target.files[0];
			var url = URL.createObjectURL(file);
			setCurrentBackgroundBlob(url);
			setSelectedBackgroundBlob(file);
		} else {
			setSelectedBackgroundBlob(null);
			setCurrentBackgroundBlob(undefined);
		}
	};

	const handleOnChange2 = async (event: ChangeEvent<HTMLInputElement>) => {
		event.preventDefault();
		if (event.target.files) {
			const file = event.target.files[0];
			var url = URL.createObjectURL(file);
			setCurrentLogoBlob(url);
			setSelectedLogoBlob(file);
		} else {
			setSelectedLogoBlob(null);
			setCurrentLogoBlob(undefined);
		}
	};

	return (
		<div className="flex flex-col gap-4">
			<div className="flex flex-row gap-8">
				<div className="flex flex-col justify-center gap-4">
					<div className="form-control">
						<label className="label">
							<span className="label-text">Background Image</span>
						</label>
						<input type="file" className="input" onChange={handleOnChange} />
					</div>
					<div className="form-control">
						<label className="label">
							<span className="label-text">Logo</span>
						</label>
						<input type="file" className="input" onChange={handleOnChange2} />
					</div>
				</div>
				<div className="mockup-window bg-base-300 flex-1 h-[400px]">
					<div
						className="flex justify-center px-4 py-16 h-full bg-repeat bg-center"
						style={{
							backgroundImage: `url("${currentBackgroundBlob ?? '/bg.svg'}")`,
						}}>
						<div className="card shadow-lg bg-base-200 my-auto mx-4">
							<div className="card-body">
								<h2 className="card-title text-center">One-Time Password</h2>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};

export default BrandingSection;
