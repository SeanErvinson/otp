import { AppDetail } from '@/types/types';
import { atomWithReset } from 'jotai/utils';

export const selectedAppAtom = atomWithReset<AppDetail>({} as AppDetail);
