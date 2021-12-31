const Home = () => {
	return (
		<div>
			<div className="w-full mt-2 border stats border-base-300">
				<div className="stat">
					<div className="stat-figure text-primary">
						<button className="btn loading btn-circle btn-lg bg-base-200 btn-ghost"></button>
					</div>
					<div className="stat-value">5,000/10,000</div>
					<div className="stat-title">Email sent</div>
					<div className="stat-desc">
						<progress
							value="50"
							max="100"
							className="progress progress-secondary"></progress>
					</div>
				</div>
			</div>
			<div className="shadow stats">
				<div className="stat">
					<div className="stat-title">SMS sent</div>
					<div className="stat-value">89,400</div>
					<div className="stat-desc">21% more than last month</div>
				</div>
			</div>
		</div>
	);
};

export default Home;
