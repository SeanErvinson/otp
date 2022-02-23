import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" {...props}>
		<path d="M4 6h16v2H4zm0 5h16v2H4zm0 5h16v2H4z" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
