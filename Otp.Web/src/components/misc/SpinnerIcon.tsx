import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg
		viewBox="0 0 100 100"
		xmlSpace="preserve"
		xmlns="http://www.w3.org/2000/svg"
		{...props}
		width="8rem"
		fill="hsl(var(--b3, var(--b2)))">
		<path d="M73 50c0-12.7-10.3-23-23-23S27 37.3 27 50m3.9 0c0-10.5 8.5-19.1 19.1-19.1S69.1 39.5 69.1 50">
			<animateTransform
				attributeName="transform"
				attributeType="XML"
				dur="1s"
				from="0 50 50"
				repeatCount="indefinite"
				to="360 50 50"
				type="rotate"
			/>
		</path>
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
