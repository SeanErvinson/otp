import { atomWithReset } from 'jotai/utils';

import { Status } from '@/types/types';

export const statusFilterAtom = atomWithReset<Status[]>([]);
