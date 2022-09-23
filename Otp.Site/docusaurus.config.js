// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

require('dotenv').config();
const lightCodeTheme = require('prism-react-renderer/themes/github');

/** @type {import('@docusaurus/types').Config} */
const config = {
	title: 'OHTP',
	tagline: 'OTP made easy',
	url: 'https://ohtp.dev',
	baseUrl: '/',
	onBrokenLinks: 'throw',
	onBrokenMarkdownLinks: 'warn',
	favicon: 'img/favicon.ico',
	organizationName: 'OHTP',
	projectName: 'OHTP.Site',
	customFields: {
		recaptchaSiteKey: process.env.SITE_RECAPTCHA_KEY,
		supportUrl: 'support@ohtp.dev',
	},
	presets: [
		[
			'classic',
			/** @type {import('@docusaurus/preset-classic').Options} */
			({
				docs: {
					sidebarPath: false,
				},
				gtag: {
					trackingID: 'G-42TQF2SEPP',
				},
				blog: false,
				theme: {
					customCss: require.resolve('./src/css/custom.css'),
				},
			}),
		],
		[
			'redocusaurus',
			{
				specs: [
					{
						id: 'app-api',
						spec: 'https://raw.githubusercontent.com/SeanErvinson/earendel-demo/master/swagger.json',
						route: '/api/',
						layout: {
							title: 'Hello World',
							description: 'Hello World',
						},
					},
				],
				theme: {
					primaryColor: '#ef6060',
				},
			},
		],
	],
	plugins: [
		async function plugins() {
			return {
				name: 'docusaurus-tailwindcss',
				configurePostCss(postcssOptions) {
					postcssOptions.plugins.push(require('tailwindcss'));
					postcssOptions.plugins.push(require('autoprefixer'));
					return postcssOptions;
				},
			};
		},
	],
	themeConfig:
		/** @type {import('@docusaurus/preset-classic').ThemeConfig} */
		({
			colorMode: {
				defaultMode: 'light',
				disableSwitch: true,
			},
			image: 'img/logo.png',
			metadata: [
				{ name: 'keywords', content: 'otp, otp service, otp api, api' },
				{ property: 'og:type', content: 'website' },
				{ property: 'og:site_name', content: 'OHTP' },
			],
			navbar: {
				logo: {
					alt: 'OHTP logo',
					width: 80,
					height: 80,
					src: 'img/logo.svg',
				},
				items: [
					{
						type: 'doc',
						docId: 'api',
						position: 'right',
						label: 'Documentation',
					},
					{ to: '/pricing', label: 'Pricing', position: 'right' },
					{
						to: 'https://app.ohtp.dev',
						position: 'right',
						label: 'Go to App',
					},
				],
			},
			footer: {
				style: 'dark',
				links: [
					{
						title: 'Docs',
						items: [
							{
								label: 'Documentation',
								to: '/docs/api',
							},
						],
					},
					{
						title: 'Support',
						items: [
							{
								label: 'Contact us',
								to: '/contact',
							},
							{
								html: `
                    <a href="mailto:support@ohtp.dev" >
                      <p>support@ohtp.dev</p>
                    </a>
                  `,
							},
						],
					},
					{
						title: 'Legal',
						items: [
							{
								label: 'Terms of Service',
								to: '/tos',
							},
							{
								label: 'Privacy Policy',
								to: '/privacy-policy',
							},
						],
					},
				],
				copyright: `Copyright © ${new Date().getFullYear()} OhTP, LLC.`,
			},
			prism: {
				theme: lightCodeTheme,
			},
		}),
};

module.exports = config;
