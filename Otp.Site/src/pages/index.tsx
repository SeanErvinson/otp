import React from 'react';
import Layout from '@theme/Layout';
import HomepageFeatures from '@site/src/components/HomepageFeatures';
import HomepageHeader from '../components/HomepageHeader';
import Head from '@docusaurus/Head';

const Home = () => {
	return (
		<Layout title={`Welcome`} description="Simple OTP integration">
			<Head>
				<script src="https://www.google.com/recaptcha/api.js" async defer></script>
			</Head>
			<HomepageHeader />
			<HomepageFeatures />
		</Layout>
	);
};

export default Home;
