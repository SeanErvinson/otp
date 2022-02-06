/**
 * Otp
 */

const Channel = ['SMS', 'Email'] as const;
export type Channel = typeof Channel[number];
