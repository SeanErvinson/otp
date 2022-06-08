import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" {...props}>
		<path d="M16 12a2 2 0 1 0 0-4 2 2 0 0 0 0 4ZM10 12a2 2 0 1 0 0-4 2 2 0 0 0 0 4ZM2 14h7v2H2v-2Zm2 3h6v2H4v-2Zm3 3h4v2H7v-2Z" />
		<path d="M20.1 3H4.9C3.852 3 3 3.862 3 4.922V13h1.9V4.922h15.2v11.535h-4.75v2.148L12 16.57v2.242L17.25 22v-3.62h2.85c1.048 0 1.9-.862 1.9-1.922V4.921C22 3.862 21.148 3 20.1 3Z" />
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
