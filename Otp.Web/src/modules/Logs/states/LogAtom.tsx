import { atomWithReset } from 'jotai/utils';

import { Log } from '@/types/types';

export const logAtom = atomWithReset<Log>({} as Log);
