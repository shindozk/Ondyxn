/**
 * Ondyxn Browser - Material You Color System
 * Based on Material Design 3 color tokens with Liquid Glass support
 */

export const MaterialColors = {
  // Primary palette (Cyan)
  primary: '#06B6D4',
  primaryLight: '#22D3EE',
  primaryDark: '#0891B2',
  onPrimary: '#FFFFFF',
  primaryContainer: '#0E7490',
  onPrimaryContainer: '#CFFAFE',

  // Secondary palette (Purple)
  secondary: '#8B5CF6',
  secondaryLight: '#A78BFA',
  secondaryDark: '#7C3AED',
  onSecondary: '#FFFFFF',
  secondaryContainer: '#6D28D9',
  onSecondaryContainer: '#EDE9FE',

  // Tertiary palette (Rose)
  tertiary: '#F43F5E',
  tertiaryLight: '#FB7185',
  tertiaryDark: '#E11D48',
  onTertiary: '#FFFFFF',
  tertiaryContainer: '#BE123C',
  onTertiaryContainer: '#FFE4E6',

  // Neutral palette
  background: '#09090B',
  onBackground: '#FAFAFA',
  surface: '#0F0F12',
  onSurface: '#F4F4F5',
  surfaceVariant: '#18181B',
  onSurfaceVariant: '#A1A1AA',
  surfaceContainer: '#1C1C1F',
  surfaceContainerLow: '#141416',
  surfaceContainerHigh: '#232326',
  surfaceContainerHighest: '#2A2A2D',

  // Error
  error: '#EF4444',
  errorLight: '#F87171',
  onError: '#FFFFFF',
  errorContainer: '#B91C1C',
  onErrorContainer: '#FEE2E2',

  // Outline
  outline: '#27272A',
  outlineVariant: '#3F3F46',

  // Success
  success: '#10B981',
  successLight: '#34D399',
  onSuccess: '#FFFFFF',

  // Warning
  warning: '#F59E0B',
  warningLight: '#FBBF24',
  onWarning: '#000000',
};

export const GlassColors = {
  // Liquid Glass effect colors
  glassBackground: 'rgba(15, 15, 18, 0.72)',
  glassBackgroundLight: 'rgba(30, 30, 35, 0.65)',
  glassBackgroundUltra: 'rgba(15, 15, 18, 0.85)',
  glassBorder: 'rgba(255, 255, 255, 0.08)',
  glassBorderLight: 'rgba(255, 255, 255, 0.12)',
  glassHighlight: 'rgba(255, 255, 255, 0.04)',
  glassShadow: 'rgba(0, 0, 0, 0.4)',

  // Gradient overlays
  glassGradientTop: 'rgba(255, 255, 255, 0.06)',
  glassGradientBottom: 'rgba(0, 0, 0, 0.1)',
};

export type AppColors = typeof MaterialColors;
export type GlassColorSet = typeof GlassColors;
