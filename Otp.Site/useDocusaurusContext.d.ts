import useDocusaurusContext from '@docusaurus/useDocusaurusContext';

declare module '@docusaurus/useDocusaurusContext' {
	export default function useDocusaurusContext(): DocusaurusContext;

	export type DocusaurusContext = {
		readonly siteConfig: DocusaurusConfig;
	};

	export type DocusaurusConfig = {
		customFields: {
			recaptchaSiteKey: string;
		};
	};
}
