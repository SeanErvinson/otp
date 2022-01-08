const BrandingSection = () => {
	return (
		<div className="flex flex-col gap-4">
			<div className="flex flex-row gap-8">
				<div className="flex flex-col justify-center gap-4">
					<div className="form-control">
						<label className="label">
							<span className="label-text">Background Image</span>
						</label>
						<input type="file" className="input" />
					</div>
					<div className="form-control">
						<label className="label">
							<span className="label-text">Logo</span>
						</label>
						<input type="file" className="input" />
					</div>
				</div>
				<div className="mockup-window bg-base-300 flex-1">
					<div className="flex justify-center px-4 py-16 bg-neutral h-full">Hello!</div>
				</div>
			</div>
		</div>
	);
};

export default BrandingSection;
