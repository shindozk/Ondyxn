/**
 * shadcn/ui Input Component for React Native
 * Based on https://ui.shadcn.com/docs/components/input
 */

import React from 'react';
import {TextInput, View, Text, StyleSheet, ViewStyle, TextInputProps} from 'react-native';
import {shadcnColors, shadcnRadius} from '../../theme/colors';

interface InputProps extends TextInputProps {
  label?: string;
  error?: string;
  containerStyle?: ViewStyle;
}

export const Input = React.forwardRef<TextInput, InputProps>(
  ({label, error, containerStyle, style, ...props}, ref) => {
    return (
      <View style={[styles.container, containerStyle]}>
        {label && (
          <Text style={styles.label}>{label}</Text>
        )}
        <TextInput
          ref={ref}
          style={[
            styles.input,
            error && styles.inputError,
            style,
          ]}
          placeholderTextColor={shadcnColors.mutedForeground}
          {...props}
        />
        {error && (
          <Text style={styles.error}>{error}</Text>
        )}
      </View>
    );
  }
);

Input.displayName = 'Input';

const styles = StyleSheet.create({
  container: {
    gap: 6,
  },
  label: {
    fontSize: 14,
    fontWeight: '500',
    color: shadcnColors.foreground,
  },
  input: {
    height: 40,
    width: '100%',
    borderRadius: shadcnRadius.md,
    borderWidth: 1,
    borderColor: shadcnColors.input,
    backgroundColor: shadcnColors.background,
    paddingHorizontal: 12,
    paddingVertical: 8,
    fontSize: 14,
    color: shadcnColors.foreground,
  },
  inputError: {
    borderColor: '#EF4444',
  },
  error: {
    fontSize: 12,
    color: '#EF4444',
  },
});

export default Input;
