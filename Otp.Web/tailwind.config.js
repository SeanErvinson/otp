module.exports = {
	content: ['./index.html', './src/**/*.{ts,tsx}'],
	theme: {
		extend: {
			gridTemplateColumns: {
				pricing: 'repeat(auto-fit,minmax(10em,1fr))',
			},
		},
	},
	plugins: [require('daisyui')],
};
