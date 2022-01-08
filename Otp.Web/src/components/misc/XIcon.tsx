import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg
		xmlns="http://www.w3.org/2000/svg"
		fill="none"
		viewBox="0 0 24 24"
		className="inline-block w-4 h-4 stroke-current"
		{...props}>
		<path d="M6 18 18 6M6 6l12 12" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
