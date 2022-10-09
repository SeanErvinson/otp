import Layout from '@theme/Layout';
import React from 'react';

const TermsOfService = () => {
	return (
		<Layout title="Terms of service" description="OHTP Terms of service">
			<main className="bg-white dark:bg-gray-900">
				<div className="container px-6 py-10 mx-auto">
					<h1 className="text-3xl font-semibold text-center text-gray-800 capitalize xl:text-5xl lg:text-4xl dark:text-white">
						Website Terms and Conditions of Use
					</h1>

					<div className="flex justify-center mx-auto mt-6">
						<span className="inline-block w-40 h-1 bg-rose-500 rounded-full"></span>
						<span className="inline-block w-3 h-1 mx-1 bg-rose-500 rounded-full"></span>
						<span className="inline-block w-1 h-1 bg-rose-500 rounded-full"></span>
					</div>

					<div className="flex items-start max-w-6xl mx-auto mt-16">
						<div>
							<h2>1. Terms</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								By accessing this Website, accessible from https://www.ohtp.dev/, you are agreeing to be
								bound by these Website Terms and Conditions of Use and agree that you are responsible
								for the agreement with any applicable local laws. If you disagree with any of these
								terms, you are prohibited from accessing this site. The materials contained in this
								Website are protected by copyright and trade mark law.
							</p>

							<h2>2. Use License</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								Permission is granted to temporarily download one copy of the materials on
								https://www.ohtp.dev/'s Website for personal, non-commercial transitory viewing only.
								This is the grant of a license, not a transfer of title, and under this license you may
								not:
							</p>

							<ul className="list-disc text-gray-500 lg:mx-8">
								<li>modify or copy the materials;</li>
								<li>use the materials for any commercial purpose or for any public display;</li>
								<li>
									attempt to reverse engineer any software contained on https://www.ohtp.dev/'s
									Website;
								</li>
								<li>remove any copyright or other proprietary notations from the materials; or</li>
								<li>
									transferring the materials to another person or "mirror" the materials on any other
									server.
								</li>
							</ul>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								This will let https://www.ohtp.dev/ to terminate upon violations of any of these
								restrictions. Upon termination, your viewing right will also be terminated and you
								should destroy any downloaded materials in your possession whether it is printed or
								electronic format. These Terms of Service has been created with the help of the{' '}
								<a href="https://www.termsofservicegenerator.net">Terms Of Service Generator</a>.
							</p>

							<h2>3. Disclaimer</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								All the materials on https://www.ohtp.dev/’s Website are provided "as is".
								https://www.ohtp.dev/ makes no warranties, may it be expressed or implied, therefore
								negates all other warranties. Furthermore, https://www.ohtp.dev/ does not make any
								representations concerning the accuracy or reliability of the use of the materials on
								its Website or otherwise relating to such materials or any sites linked to this Website.
							</p>

							<h2>4. Limitations</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								https://www.ohtp.dev/ or its suppliers will not be hold accountable for any damages that
								will arise with the use or inability to use the materials on https://www.ohtp.dev/’s
								Website, even if https://www.ohtp.dev/ or an authorize representative of this Website
								has been notified, orally or written, of the possibility of such damage. Some
								jurisdiction does not allow limitations on implied warranties or limitations of
								liability for incidental damages, these limitations may not apply to you.
							</p>

							<h2>5. Revisions and Errata</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								The materials appearing on https://www.ohtp.dev/’s Website may include technical,
								typographical, or photographic errors. https://www.ohtp.dev/ will not promise that any
								of the materials in this Website are accurate, complete, or current.
								https://www.ohtp.dev/ may change the materials contained on its Website at any time
								without notice. https://www.ohtp.dev/ does not make any commitment to update the
								materials.
							</p>

							<h2>6. Links</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								https://www.ohtp.dev/ has not reviewed all of the sites linked to its Website and is not
								responsible for the contents of any such linked site. The presence of any link does not
								imply endorsement by https://www.ohtp.dev/ of the site. The use of any linked website is
								at the user’s own risk.
							</p>

							<h2>7. Site Terms of Use Modifications</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								https://www.ohtp.dev/ may revise these Terms of Use for its Website at any time without
								prior notice. By using this Website, you are agreeing to be bound by the current version
								of these Terms and Conditions of Use.
							</p>

							<h2>8. Your Privacy</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								Please read our Privacy Policy.
							</p>

							<h2>9. Governing Law</h2>

							<p className="flex items-center text-center text-gray-500 lg:mx-8">
								Any claim related to https://www.ohtp.dev/'s Website shall be governed by the laws of ph
								without regards to its conflict of law provisions.
							</p>
						</div>
					</div>
				</div>
			</main>
		</Layout>
	);
};

export default TermsOfService;
