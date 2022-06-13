import { atomWithReset } from 'jotai/utils';

export const appIdAtom = atomWithReset<string>('');
