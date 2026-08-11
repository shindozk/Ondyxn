/**
 * shadcn/ui Button Component for React Native
 * Based on https://ui.shadcn.com/docs/components/button
 */

import React from 'react';
import {TouchableOpacity, Text, StyleSheet, ViewStyle, TextStyle, ActivityIndicator} from 'react-native';
import {shadcnColors, zinc, shadcnRadius} from '../../theme/colors';

type ButtonVariant = 'default' | 'destructive' | 'outline' | 'secondary' | 'ghost' | 'link';
type ButtonSize = 'default' | 'sm' | 'lg' | 'icon';

interface ButtonProps {
  children: React.ReactNode;
  variant?: ButtonVariant;
  size?: ButtonSize;
  onPress?: () => void;
  disabled?: boolean;
  loading?: boolean;
  style?: ViewStyle;
}

const variantStyles: Record<ButtonVariant, {container: ViewStyle; text: TextStyle}> = {
  default: {
    container: {
      backgroundColor: shadcnColors.primary,
      borderWidth: 0,
    },
    text: {
      color: shadcnColors.primaryForeground,
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
  secondary: {
    container: {
      backgroundColor: shadcnColors.secondary,
      borderWidth: 0,
    },
    text: {
      color: shadcnColors.secondaryForeground,
    },
  },
  ghost: {
    container: {
      backgroundColor: 'transparent',
      borderWidth: 0,
    },
    text: {
      color: shadcnColors.foreground,
    },
  },
  link: {
    container: {
      backgroundColor: 'transparent',
      borderWidth: 0,
    },
    text: {
      color: shadcnColors.foreground,
      textDecorationLine: 'underline',
    },
  },
};

const sizeStyles: Record<ButtonSize, ViewStyle> = {
  default: {
    height: 40,
    paddingHorizontal: 16,
    paddingVertical: 8,
  },
  sm: {
    height: 36,
    paddingHorizontal: 12,
    paddingVertical: 6,
  },
  lg: {
    height: 44,
    paddingHorizontal: 24,
    paddingVertical: 10,
  },
  icon: {
    height: 40,
    width: 40,
    paddingHorizontal: 0,
    paddingVertical: 0,
  },
};

export const Button: React.FC<ButtonProps> = ({
  children,
  variant = 'default',
  size = 'default',
  onPress,
  disabled = false,
  loading = false,
  style,
}) => {
  const [pressed, setPressed] = React.useState(false);
  
  const variantStyle = variantStyles[variant];
  const sizeStyle = sizeStyles[size];
  
  return (
    <TouchableOpacity
      onPress={onPress}
      disabled={disabled || loading}
      activeOpacity={0.7}
      onPressIn={() => setPressed(true)}
      onPressOut={() => setPressed(false)}
      style={[
        styles.button,
        sizeStyle,
        variantStyle.container,
        pressed && styles.pressed,
        disabled && styles.disabled,
        style,
      ]}>
      {loading ? (
        <ActivityIndicator 
          size="small" 
          color={variantStyle.text.color} 
        />
      ) : (
        <Text style={[styles.text, variantStyle.text, disabled && styles.textDisabled]}>
          {children}
        </Text>
      )}
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  button: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: shadcnRadius.md,
  },
  text: {
    fontSize: 14,
    fontWeight: '500',
    letterSpacing: 0.1,
  },
  pressed: {
    opacity: 0.9,
  },
  disabled: {
    opacity: 0.5,
  },
  textDisabled: {
    opacity: 0.7,
  },
});

export default Button;
