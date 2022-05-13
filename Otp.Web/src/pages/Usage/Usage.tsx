import { LastSpanSelect } from '@/components/LastSpanSelect';
import PageHeader from '@/components/PageHeader/PageHeader';

const Usage = () => {
	return (
		<main id="usage">
			<PageHeader title="Usage" />
			<article>
				<LastSpanSelect onChange={e => console.log(e)} />
			</article>
		</main>
	);
};

export default Usage;
