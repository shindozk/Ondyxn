/**
 * Ondyxn Browser - Liquid Glass Effect Component
 * Ultra-transparent glass for browser chrome
 */

import React from 'react';
import {View, ViewStyle, StyleSheet} from 'react-native';
import {GlassColors} from '../theme/colors';
import {BorderRadius} from '../theme/typography';

interface GlassProps {
  children: React.ReactNode;
  style?: ViewStyle;
  variant?: 'default' | 'light' | 'ultra' | 'tab' | 'sidebar' | 'omnibox';
  borderWidth?: number;
  rounded?: 'sm' | 'md' | 'lg' | 'xl' | 'full';
}

const variantStyles: Record<string, ViewStyle> = {
  default: {
    backgroundColor: GlassColors.glassBackground,
    borderColor: GlassColors.glassBorder,
  },
  light: {
    backgroundColor: GlassColors.glassBackgroundLight,
    borderColor: GlassColors.glassBorderLight,
  },
  ultra: {
    backgroundColor: GlassColors.glassBackgroundUltra,
    borderColor: GlassColors.glassBorder,
  },
  tab: {
    backgroundColor: GlassColors.tabActive,
    borderColor: 'transparent',
  },
  sidebar: {
    backgroundColor: GlassColors.sidebar,
    borderColor: GlassColors.sidebarBorder,
  },
  omnibox: {
    backgroundColor: GlassColors.omnibox,
    borderColor: GlassColors.omniboxBorder,
  },
};

const roundedStyles: Record<string, number> = {
  sm: BorderRadius.sm,
  md: BorderRadius.md,
  lg: BorderRadius.lg,
  xl: BorderRadius.xl,
  full: BorderRadius.full,
};

export const Glass: React.FC<GlassProps> = ({
  children,
  style,
  variant = 'default',
  borderWidth = 1,
  rounded = 'md',
}) => {
  return (
    <View
      style={[
        styles.container,
        variantStyles[variant],
        {
          borderRadius: roundedStyles[rounded],
          borderWidth: borderWidth,
        },
        style,
      ]}>
      {/* Subtle top highlight for glass refraction */}
      <View
        style={[styles.highlight, {borderRadius: roundedStyles[rounded] - 1}]}
        pointerEvents="none"
      />
      {/* Content */}
      <View style={styles.content}>{children}</View>
    </View>
  );
};

// Glass Button
interface GlassButtonProps {
  children: React.ReactNode;
  onPress?: () => void;
  style?: ViewStyle;
  variant?: 'default' | 'light' | 'primary' | 'danger';
  size?: 'sm' | 'md' | 'lg' | 'icon';
  active?: boolean;
}

export const GlassButton: React.FC<GlassButtonProps> = ({
  children,
  style,
  variant = 'default',
  size = 'md',
  active = false,
}) => {
  const [pressed, setPressed] = React.useState(false);

  const sizeStyles: Record<string, ViewStyle> = {
    sm: {paddingHorizontal: 10, paddingVertical: 5},
    md: {paddingHorizontal: 14, paddingVertical: 8},
    lg: {paddingHorizontal: 20, paddingVertical: 12},
    icon: {paddingHorizontal: 8, paddingVertical: 8, aspectRatio: 1},
  };

  const variantMap: Record<string, ViewStyle> = {
    default: {
      backgroundColor: pressed
        ? 'rgba(255,255,255,0.08)'
        : 'rgba(255,255,255,0.04)',
      borderColor: pressed
        ? 'rgba(255,255,255,0.12)'
        : 'rgba(255,255,255,0.06)',
    },
    light: {
      backgroundColor: pressed
        ? 'rgba(255,255,255,0.12)'
        : 'rgba(255,255,255,0.06)',
      borderColor: pressed
        ? 'rgba(255,255,255,0.16)'
        : 'rgba(255,255,255,0.10)',
    },
    primary: {
      backgroundColor: pressed ? '#0891B2' : '#06B6D4',
      borderColor: pressed ? '#0891B2' : '#22D3EE',
    },
    danger: {
      backgroundColor: pressed ? '#DC2626' : '#EF4444',
      borderColor: pressed ? '#DC2626' : '#F87171',
    },
  };

  return (
    <View
      style={[
        styles.button,
        sizeStyles[size],
        variantMap[variant],
        active && styles.activeButton,
        style,
      ]}>
      {children}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    overflow: 'hidden',
  },
  highlight: {
    ...StyleSheet.absoluteFillObject,
    backgroundColor: 'rgba(255,255,255,0.02)',
    borderBottomWidth: 1,
    borderBottomColor: 'rgba(255,255,255,0.04)',
  },
  content: {
    flex: 1,
  },
  button: {
    borderWidth: 1,
    borderRadius: BorderRadius.sm,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 6,
  },
  activeButton: {
    backgroundColor: 'rgba(6, 182, 212, 0.15)',
    borderColor: 'rgba(6, 182, 212, 0.3)',
  },
});

export default Glass;
