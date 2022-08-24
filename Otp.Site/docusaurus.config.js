// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require('prism-react-renderer/themes/github');

/** @type {import('@docusaurus/types').Config} */
const config = {
	title: 'OHTP',
	tagline: 'OTP made easy',
	url: 'https://darling-cajeta-d4181d.netlify.app',
	baseUrl: '/',
	onBrokenLinks: 'throw',
	onBrokenMarkdownLinks: 'warn',
	favicon: 'img/favicon.ico',
	organizationName: 'facebook', // Usually your GitHub org/user name.
	projectName: 'docusaurus', // Usually your repo name.

	presets: [
		[
			'classic',
			/** @type {import('@docusaurus/preset-classic').Options} */
			({
				docs: {
					sidebarPath: false,
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
							title: 'Hello WOrld',
							description: 'Hello WOrld',
						},
					},
				],
				theme: {
					// Change with your site colors
					primaryColor: '#1890ff',
				},
			},
		],
	],
	plugins: [
		async function myPlugin(context, options) {
			return {
				name: 'docusaurus-tailwindcss',
				configurePostCss(postcssOptions) {
					// Appends TailwindCSS and AutoPrefixer.
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
						href: 'https://github.com/facebook/docusaurus',
						label: 'Sign Up',
						position: 'right',
						className: 'button button--lg',
					},
					{
						href: 'https://github.com/facebook/docusaurus',
						label: 'Login',
						position: 'right',
						className: 'button button--secondary button--lg',
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
                    <a href="mailto:support@ohtp.xyz" >
                      <p>support@ohtp.xyz</p>
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
