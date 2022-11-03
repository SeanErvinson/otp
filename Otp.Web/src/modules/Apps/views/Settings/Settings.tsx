import { useOutletContext } from 'react-router-dom';

import LoadingIndicator from '@/components/LoadingIndicator/LoadingIndicator';
import { AppDetail } from '@/types/types';

import ApiKeySection from './ApiKeySection';
import BrandingSection from './BrandingSection';
import CallbackSection from './CallbackSection';
import SettingsSection from './SettingsSection';
import DangerSection from './DangerSection';

const Settings = () => {
	const app = useOutletContext<AppDetail | null>();

	return (
		<article id="settings">
			{!app && <LoadingIndicator />}
			{app && (
				<>
					<SettingsSection
						title="API Key"
						description="Regenerate your API Key in case the key has been compromised or leaked.">
						<ApiKeySection appId={app.id} />
					</SettingsSection>
					<SettingsSection
						title="Callback"
						description="Callback URL functions like a notification for your application. Every
						time a user submits an OTP authentication we use this callback URL to
						send relevant information to you.">
						<CallbackSection appId={app.id} />
					</SettingsSection>
					<SettingsSection
						title="Branding"
						description="Let your customer feel special with a custom background and logo.">
						<BrandingSection />
					</SettingsSection>
					<SettingsSection
						title="Caution"
						description="Irreversible and destructive actions. Careful now">
						<DangerSection appId={app.id} />
					</SettingsSection>
				</>
			)}
		</article>
	);
};

export default Settings;
