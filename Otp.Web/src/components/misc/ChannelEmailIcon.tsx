import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" {...props}>
		<path d="M20 4H6c-1.103 0-2 .897-2 2v5h2V8l6.4 4.8a1.001 1.001 0 0 0 1.2 0L20 8v9h-8v2h8c1.103 0 2-.897 2-2V6c0-1.103-.897-2-2-2Zm-7 6.75L6.666 6h12.668L13 10.75Z" />
		<path d="M2 12h7v2H2v-2Zm2 3h6v2H4v-2Zm3 3h4v2H7v-2Z" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
