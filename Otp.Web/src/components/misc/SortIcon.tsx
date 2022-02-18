import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg
		xmlns="http://www.w3.org/2000/svg"
		viewBox="0 0 24 24"
		style={{
			fill: '#000',
		}}
		{...props}>
		<path d="M8 16H4l6 6V2H8zm6-11v17h2V8h4l-6-6z" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
