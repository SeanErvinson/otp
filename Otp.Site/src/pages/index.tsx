import React from 'react';
import Layout from '@theme/Layout';
import HomepageFeatures from '@site/src/components/HomepageFeatures';
import HomepageHeader from '../components/HomepageHeader';

const Home = () => {
	return (
		<Layout title={`Welcome`} description="Simple OTP integration">
			<HomepageHeader />
			<HomepageFeatures />
		</Layout>
	);
};

export default Home;
