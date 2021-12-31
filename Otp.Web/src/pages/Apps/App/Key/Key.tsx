import { useOutletContext } from 'react-router-dom';

import { GetAppResponse } from '@/api/otpApi';

import RegenerateApiButton from './RegenerateApiButton';

const Key = () => {
	const app = useOutletContext<GetAppResponse | null>();

	return (
		<article id="key">
			<RegenerateApiButton appId={app!.id} />
		</article>
	);
};

export default Key;
// TODO: Remove !
