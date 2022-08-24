import { Log } from '@/types/types';
import { atomWithReset } from 'jotai/utils';

export const logAtom = atomWithReset<Log>({} as Log);
