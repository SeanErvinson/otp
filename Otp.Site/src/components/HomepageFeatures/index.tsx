import React from 'react';

type FeatureItem = {
	title: string;
	Svg: React.ComponentType<React.ComponentProps<'svg'>>;
	description: JSX.Element;
};

interface Props {
	item: FeatureItem;
}

const FeatureList: FeatureItem[] = [
	{
		title: 'Easy to Use',
		Svg: require('@site/static/img/easy.svg').default,
		description: (
			<>OHTP provides a simple, yet flexible API and a customizable page so you can make it feel personal.</>
		),
	},
	{
		title: 'Focus on What Matters',
		Svg: require('@site/static/img/focus.svg').default,
		description: (
			<>
				OHTP lets you focus on what's really important, your product, your idea, your goal. Don't waste time on
				unnecessary features, let us take care of it.
			</>
		),
	},
	{
		title: 'Multiple OTP channels',
		Svg: require('@site/static/img/otp.svg').default,
		description: <>SMS? Email? Both? We got you covered, choose whatever you like and we'll deliver it for you.</>,
	},
];

const Feature = (props: Props) => {
	return (
		<div className="flex flex-col items-center p-6 space-y-3 text-center bg-gray-100 rounded-xl dark:bg-gray-800">
			<span className="inline-block p-3 text-rose-500 bg-rose-100 rounded-full dark:text-white dark:bg-rose-500">
				<props.item.Svg className="w-8 h-8" role="img" />
			</span>

			<h1 className="text-2xl font-semibold text-gray-700 capitalize dark:text-white">{props.item.title}</h1>

			<p className="text-gray-500 dark:text-gray-300">{props.item.description}</p>
		</div>
	);
};

const HomepageFeatures = () => {
	return (
		<section className="bg-white dark:bg-gray-900">
			<div className="container px-6 py-12 mx-auto">
				<div className="grid grid-cols-1 gap-8 mt-8 xl:mt-12 xl:gap-16 md:grid-cols-2 xl:grid-cols-3">
					{FeatureList.map((feature, index) => (
						<Feature item={feature} key={index} />
					))}
				</div>
			</div>
		</section>
	);
};

export default HomepageFeatures;
