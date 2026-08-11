/**
 * shadcn/ui Badge Component for React Native
 * Based on https://ui.shadcn.com/docs/components/badge
 */

import React from 'react';
import {View, Text, StyleSheet, ViewStyle, TextStyle} from 'react-native';
import {shadcnColors, shadcnRadius} from '../../theme/colors';

type BadgeVariant = 'default' | 'secondary' | 'destructive' | 'outline';

interface BadgeProps {
  children: React.ReactNode;
  variant?: BadgeVariant;
  style?: ViewStyle;
}

const variantStyles: Record<BadgeVariant, {container: ViewStyle; text: TextStyle}> = {
  default: {
    container: {
      backgroundColor: shadcnColors.primary,
      borderWidth: 0,
    },
    text: {
      color: shadcnColors.primaryForeground,
    },
  },
  secondary: {
    container: {
      backgroundColor: shadcnColors.secondary,
      borderWidth: 0,
    },
    text: {
      color: shadcnColors.secondaryForeground,
    },
  },
  destructive: {
    container: {
      backgroundColor: shadcnColors.destructive,
      borderWidth: 0,
    },
    text: {
      color: shadcnColors.destructiveForeground,
    },
  },
  outline: {
    container: {
      backgroundColor: 'transparent',
      borderWidth: 1,
      borderColor: shadcnColors.border,
    },
    text: {
      color: shadcnColors.foreground,
    },
  },
};

export const Badge: React.FC<BadgeProps> = ({
  children,
  variant = 'default',
  style,
}) => {
  const variantStyle = variantStyles[variant];
  
  return (
    <View style={[styles.badge, variantStyle.container, style]}>
      <Text style={[styles.text, variantStyle.text]}>
        {children}
      </Text>
    </View>
  );
};

const styles = StyleSheet.create({
  badge: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: shadcnRadius.full,
    paddingHorizontal: 10,
    paddingVertical: 2,
  },
  text: {
    fontSize: 12,
    fontWeight: '600',
    letterSpacing: 0.1,
  },
});

export default Badge;
