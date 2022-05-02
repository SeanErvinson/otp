import * as React from 'react';
import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg viewBox="0 0 40 40" xmlns="http://www.w3.org/2000/svg" {...props}>
		<path d="M20 3.367c-9.183 0-16.633 7.45-16.633 16.633S10.817 36.633 20 36.633 36.633 29.183 36.633 20 29.183 3.367 20 3.367Zm-.867 29.966V22.9h-5.8l8.334-16.233V17.1h5.583l-8.117 16.233Z" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
