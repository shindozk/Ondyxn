/**
 * Ondyxn Browser - Material You Color System
 * Ultra-transparent Liquid Glass design
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

  // Neutral palette - ultra dark
  background: '#0A0A0C',
  onBackground: '#FAFAFA',
  surface: '#111114',
  onSurface: '#F4F4F5',
  surfaceVariant: '#18181B',
  onSurfaceVariant: '#A1A1AA',
  surfaceContainer: '#1C1C1F',
  surfaceContainerLow: '#141416',
  surfaceContainerHigh: '#232326',
  surfaceContainerHighest: '#2A2A2D',

  // Outline
  outline: '#27272A',
  outlineVariant: '#3F3F46',

  // Success / Error
  success: '#10B981',
  error: '#EF4444',

  // Warning
  warning: '#F59E0B',
};

/**
 * Liquid Glass effect colors - ultra transparent
 * Designed to show the desktop/page content through the chrome
 */
export const GlassColors = {
  // Chrome glass (tab bar, omnibox area)
  chromeBackground: 'rgba(18, 18, 22, 0.65)',
  chromeBorder: 'rgba(255, 255, 255, 0.06)',

  // Glass surfaces
  glassBackground: 'rgba(15, 15, 20, 0.55)',
  glassBackgroundLight: 'rgba(25, 25, 30, 0.50)',
  glassBackgroundUltra: 'rgba(15, 15, 20, 0.70)',
  glassBorder: 'rgba(255, 255, 255, 0.06)',
  glassBorderLight: 'rgba(255, 255, 255, 0.10)',
  glassHighlight: 'rgba(255, 255, 255, 0.03)',
  glassShadow: 'rgba(0, 0, 0, 0.3)',

  // Tab glass
  tabActive: 'rgba(255, 255, 255, 0.08)',
  tabHover: 'rgba(255, 255, 255, 0.05)',
  tabInactive: 'transparent',

  // Omnibox glass
  omnibox: 'rgba(255, 255, 255, 0.06)',
  omniboxFocused: 'rgba(255, 255, 255, 0.10)',
  omniboxBorder: 'rgba(255, 255, 255, 0.08)',
  omniboxBorderFocused: 'rgba(6, 182, 212, 0.40)',

  // Sidebar glass
  sidebar: 'rgba(12, 12, 16, 0.80)',
  sidebarBorder: 'rgba(255, 255, 255, 0.04)',

  // Gradient overlays
  glassGradientTop: 'rgba(255, 255, 255, 0.04)',
  glassGradientBottom: 'rgba(0, 0, 0, 0.08)',
};

export type AppColors = typeof MaterialColors;
export type GlassColorSet = typeof GlassColors;
