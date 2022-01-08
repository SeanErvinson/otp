import { LastSpanSelect } from '@/components/LastSpanSelect';

const Usage = () => {
	return (
		<main id="usage">
			<div className="flex flex-row justify-between">
				<h1 className="text-4xl font-bold mb-6">Usage</h1>
			</div>
			<article>
				<LastSpanSelect onChange={e => console.log(e)} />
			</article>
		</main>
	);
};

export default Usage;
