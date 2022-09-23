import React from 'react';
import Layout from '@theme/Layout';
import HomepageFeatures from '@site/src/components/HomepageFeatures';
import HomepageHeader from '../components/HomepageHeader';

const Home = () => {
	return (
		<Layout
			title={`Simple OTP API integration`}
			description="Keep your application safe with OTP. OHTP is a plug and play otp integration with simple api endpoints.">
			<HomepageHeader />
			<HomepageFeatures />
		</Layout>
	);
};

export default Home;
