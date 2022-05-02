import * as React from 'react';
import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg viewBox="0 0 40 40" xmlns="http://www.w3.org/2000/svg" {...props}>
		<path d="M20 3.333C10.8 3.333 3.333 10.8 3.333 20c0 9.2 7.467 16.667 16.667 16.667 9.2 0 16.667-7.467 16.667-16.667C36.667 10.8 29.2 3.333 20 3.333Zm-3.333 25L8.333 20l2.35-2.35 5.984 5.967 12.65-12.65 2.35 2.366-15 15Z" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
