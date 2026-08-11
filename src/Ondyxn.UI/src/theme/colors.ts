/**
 * Ondyxn Browser - shadcn/ui Color System
 * Dark theme with zinc palette matching shadcn/ui exactly
 */

/**
 * shadcn/ui Dark Theme Colors (Zinc)
 * Based on CSS variables from ui.shadcn.com
 * Converted from oklch to hex for React Native
 */
export const shadcnColors = {
  // Core semantic tokens
  background: '#09090B',       // hsl(240, 10%, 3.9%)
  foreground: '#FAFAFA',       // hsl(0, 0%, 98%)
  
  card: '#09090B',             // hsl(240, 10%, 3.9%)
  cardForeground: '#FAFAFA',   // hsl(0, 0%, 98%)
  
  popover: '#09090B',          // hsl(240, 10%, 3.9%)
  popoverForeground: '#FAFAFA', // hsl(0, 0%, 98%)
  
  primary: '#FAFAFA',          // hsl(0, 0%, 98%)
  primaryForeground: '#18181B', // hsl(240, 5.9%, 10%)
  
  secondary: '#27272A',        // hsl(240, 3.7%, 15.9%)
  secondaryForeground: '#FAFAFA', // hsl(0, 0%, 98%)
  
  muted: '#27272A',            // hsl(240, 3.7%, 15.9%)
  mutedForeground: '#A1A1AA',  // hsl(240, 5%, 64.9%)
  
  accent: '#27272A',           // hsl(240, 3.7%, 15.9%)
  accentForeground: '#FAFAFA', // hsl(0, 0%, 98%)
  
  destructive: '#7F1D1D',      // hsl(0, 62.8%, 30.6%)
  destructiveForeground: '#FAFAFA', // hsl(0, 0%, 98%)
  
  border: '#27272A',           // hsl(240, 3.7%, 15.9%)
  input: '#27272A',            // hsl(240, 3.7%, 15.9%)
  ring: '#D4D4D8',             // hsl(240, 5.9%, 83.9%)
  
  // Chart colors
  chart1: '#FAFAFA',
  chart2: '#A1A1AA',
  chart3: '#52525B',
  chart4: '#3F3F46',
  chart5: '#27272A',
  
  // Sidebar
  sidebar: '#09090B',
  sidebarForeground: '#FAFAFA',
  sidebarPrimary: '#FAFAFA',
  sidebarPrimaryForeground: '#18181B',
  sidebarAccent: '#27272A',
  sidebarAccentForeground: '#FAFAFA',
  sidebarBorder: '#27272A',
  sidebarRing: '#D4D4D8',
} as const;

/**
 * Zinc color scale from shadcn/ui
 * For additional UI elements
 */
export const zinc = {
  50: '#FAFAFA',
  100: '#F4F4F5',
  200: '#E4E4E7',
  300: '#D4D4D8',
  400: '#A1A1AA',
  500: '#71717A',
  600: '#52525B',
  700: '#3F3F46',
  800: '#27272A',
  900: '#18181B',
  950: '#09090B',
} as const;

/**
 * shadcn/ui radius scale
 */
export const shadcnRadius = {
  sm: 6,
  md: 8,
  lg: 12,
  xl: 16,
  '2xl': 20,
  '3xl': 24,
  full: 9999,
} as const;

// Alias for backwards compatibility
export const MaterialColors = shadcnColors;
export const GlassColors = {
  glassBackground: shadcnColors.secondary + '90',
  glassBackgroundLight: shadcnColors.secondary + 'B0',
  glassBackgroundUltra: shadcnColors.secondary + 'CC',
  glassBorder: shadcnColors.border,
  glassBorderLight: shadcnColors.border,
  glassHighlight: zinc[700] + '40',
  glassShadow: '#00000060',
  tabActive: shadcnColors.accent,
  tabHover: zinc[800],
  tabInactive: 'transparent',
  omnibox: shadcnColors.input,
  omniboxFocused: zinc[800],
  omniboxBorder: shadcnColors.border,
  omniboxBorderFocused: zinc[500],
  sidebar: shadcnColors.background,
  sidebarBorder: shadcnColors.border,
  chromeBackground: shadcnColors.background,
  chromeBorder: shadcnColors.border,
};

export type AppColors = typeof shadcnColors;
