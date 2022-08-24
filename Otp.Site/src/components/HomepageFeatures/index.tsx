import React from 'react';

type FeatureItem = {
	title: string;
	Svg: React.ComponentType<React.ComponentProps<'svg'>>;
	description: JSX.Element;
};

interface Props {
	item: FeatureItem;
	isLast: boolean;
}

const FeatureList: FeatureItem[] = [
	{
		title: 'Easy to Use',
		Svg: require('@site/static/img/easy.svg').default,
		description: (
			<>
				OHTP provides a simple, yet flexible API and a customizable page so you can make it
				feel personal.
			</>
		),
	},
	{
		title: 'Focus on What Matters',
		Svg: require('@site/static/img/focus.svg').default,
		description: (
			<>
				OHTP lets you focus on what's really important, your product, your idea, your goal.
				Don't waste time on unnecessary features, let us take care of it.
			</>
		),
	},
	{
		title: 'Multiple OTP channels',
		Svg: require('@site/static/img/otp.svg').default,
		description: (
			<>
				SMS? Email? Both? We got you covered, choose whatever you like and we'll deliver it
				for you.
			</>
		),
	},
];

const Feature = (props: Props) => {
	return (
		<div
			className={`flex flex-col lg:col-span-1 items-center p-6 space-y-3 text-center bg-gray-100 rounded-xl dark:bg-gray-800 ${
				props.isLast ? 'md:col-span-2' : ''
			}`}>
			<span className="inline-block p-3 text-blue-500 bg-blue-100 rounded-full dark:text-white dark:bg-blue-500">
				<props.item.Svg className="w-[200px] h-[200px]" role="img" />
			</span>

			<h1 className="text-2xl font-semibold text-gray-700 capitalize dark:text-white">
				{props.item.title}
			</h1>

			<p className="text-gray-500 dark:text-gray-300">{props.item.description}</p>
		</div>
	);
};

const HomepageFeatures = () => {
	return (
		<section className="flex items-center w-full pb-8">
			<div className="container">
				<div className="grid grid-cols-1 gap-8 mt-8 xl:mt-12 xl:gap-16 sm: md:grid-cols-2 lg:grid-cols-3">
					{FeatureList.map((props, idx) => (
						<Feature key={idx} item={props} isLast={FeatureList.length - 1 === idx} />
					))}
				</div>
			</div>
		</section>
	);
};

export default HomepageFeatures;
