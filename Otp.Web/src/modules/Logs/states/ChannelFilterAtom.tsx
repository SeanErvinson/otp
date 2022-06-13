import { atomWithReset } from 'jotai/utils';

import { Channel } from '@/types/types';

export const channelFilterAtom = atomWithReset<Channel[]>([]);
