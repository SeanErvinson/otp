import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg
		xmlns="http://www.w3.org/2000/svg"
		fill="none"
		viewBox="0 0 24 24"
		className="inline-block w-4 h-4 stroke-current"
		{...props}>
		<path d="M19 11h-6V5h-2v6H5v2h6v6h2v-6h6z" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
