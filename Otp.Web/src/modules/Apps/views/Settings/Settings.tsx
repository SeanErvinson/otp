import { useOutletContext } from 'react-router-dom';

import LoadingIndicator from '@/components/LoadingIndicator/LoadingIndicator';
import { AppDetail } from '@/types/types';

import ApiKeySection from './ApiKeySection';
import BrandingSection from './BrandingSection';
import CallbackSection from './CallbackSection';
import SectionContainer from '../../../../components/Layouts/SettingsSection';
import DangerSection from './DangerSection';
import useSubscription from '@/hooks/useSubscription';

const Settings = () => {
	const app = useOutletContext<AppDetail | null>();
	const consumptionOnly = useSubscription('Consumption');

	return (
		<article id="settings">
			{!app && <LoadingIndicator />}
			{app && (
				<>
					<SectionContainer
						title="API Key"
						description="Regenerate your API Key in case the key has been compromised or leaked.">
						<ApiKeySection appId={app.id} />
					</SectionContainer>
					<SectionContainer
						title="Callback"
						description="Callback functions like a notification for your application. Every
						time a user submits an OTP authentication we use this callback URL to
						send relevant information to you.">
						<CallbackSection appId={app.id} />
					</SectionContainer>
					{consumptionOnly && (
						<SectionContainer
							title="Branding"
							description="Customize the look which best suits your company">
							<BrandingSection />
						</SectionContainer>
					)}
					<SectionContainer
						title="Caution"
						description="Irreversible and destructive actions. Careful now">
						<DangerSection appId={app.id} />
					</SectionContainer>
				</>
			)}
		</article>
	);
};

export default Settings;
