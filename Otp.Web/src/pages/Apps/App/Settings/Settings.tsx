import { useOutletContext } from 'react-router-dom';

import { AppDetail } from '@/common/types';
import Spinner from '@/components/misc/Spinner';

import SettingsSection from './SettingsSection';
import BrandingSection from './BrandingSection';
import ApiKeySection from './ApiKeySection';
import CallbackSection from './CallbackSection';

const Settings = () => {
	const app = useOutletContext<AppDetail | null>();

	return (
		<article id="settings">
			{!app && <Spinner />}
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
						<CallbackSection appId={app.id} callbackUrl={app.callbackUrl} />
					</SettingsSection>
					<SettingsSection
						title="Branding"
						description="Let your customer feel special with a custom background and logo.">
						<BrandingSection />
					</SettingsSection>
				</>
			)}
		</article>
	);
};

export default Settings;
