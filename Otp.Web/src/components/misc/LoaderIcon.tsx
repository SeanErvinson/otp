import { SVGProps, memo } from 'react';

const SvgComponent = (props: SVGProps<SVGSVGElement>) => (
	<svg viewBox="0 0 100 100" xmlSpace="preserve" xmlns="http://www.w3.org/2000/svg" {...props}>
		<circle cx={6} cy={50} r={6}>
			<animateTransform
				attributeName="transform"
				begin={0.1}
				dur="1s"
				repeatCount="indefinite"
				type="translate"
				values="0 15 ; 0 -15; 0 15"
			/>
		</circle>
		<circle cx={30} cy={50} r={6}>
			<animateTransform
				attributeName="transform"
				begin={0.2}
				dur="1s"
				repeatCount="indefinite"
				type="translate"
				values="0 10 ; 0 -10; 0 10"
			/>
		</circle>
		<circle cx={54} cy={50} r={6}>
			<animateTransform
				attributeName="transform"
				begin={0.3}
				dur="1s"
				repeatCount="indefinite"
				type="translate"
				values="0 5 ; 0 -5; 0 5"
			/>
		</circle>
	</svg>
);

const Memo = memo(SvgComponent);
export default Memo;
