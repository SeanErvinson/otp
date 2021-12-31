import { useMemo } from 'react';

let counter = 0;

const useGeneratedId = () => {
	const seed = useMemo(() => 'id-' + counter++, []);
	return (suffix: string) => `${seed}-${suffix}`;
};

export default useGeneratedId;
