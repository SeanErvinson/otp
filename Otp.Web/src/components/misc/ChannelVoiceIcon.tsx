import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" {...props}>
		<path d="M1 14h7v2H1v-2Zm2 3h6v2H3v-2Zm3 3h4v2H6v-2Z" />
		<path d="M19.344 19.859a2 2 0 0 1-.775.151A15.28 15.28 0 0 1 11 17.795v-2.41a13.28 13.28 0 0 0 7.59 2.625l2-2L18 13.42l-1.29 1.32a1 1 0 0 1-.91.27 10.08 10.08 0 0 1-4.5-2.3A10.12 10.12 0 0 1 9 8.21a1 1 0 0 1 .3-.91l1.29-1.29L8 3.42l-2 2A13.28 13.28 0 0 0 8.617 13H6.209A15.28 15.28 0 0 1 4 5.44a2 2 0 0 1 .59-1.43l2.7-2.72a1 1 0 0 1 1.41 0l4 4a1 1 0 0 1 0 1.41l-1.59 1.6a7.618 7.618 0 0 0 1.59 3 7.55 7.55 0 0 0 3 1.59l1.6-1.59a1 1 0 0 1 1.41 0l4 4a1 1 0 0 1 0 1.41L20 19.42c-.188.188-.41.337-.656.439Z" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
