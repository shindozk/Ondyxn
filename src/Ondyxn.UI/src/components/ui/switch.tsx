/**
 * shadcn/ui Switch Component for React Native
 * Based on https://ui.shadcn.com/docs/components/switch
 */

import React from 'react';
import {TouchableOpacity, View, StyleSheet, ViewStyle} from 'react-native';
import {shadcnColors, shadcnRadius} from '../../theme/colors';

interface SwitchProps {
  checked: boolean;
  onCheckedChange?: (checked: boolean) => void;
  disabled?: boolean;
  style?: ViewStyle;
}

export const Switch: React.FC<SwitchProps> = ({
  checked,
  onCheckedChange,
  disabled = false,
  style,
}) => {
  return (
    <TouchableOpacity
      onPress={() => onCheckedChange?.(!checked)}
      disabled={disabled}
      activeOpacity={0.8}
      style={[
        styles.switch,
        checked && styles.switchChecked,
        disabled && styles.switchDisabled,
        style,
      ]}>
      <View style={[styles.thumb, checked && styles.thumbChecked]} />
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  switch: {
    width: 44,
    height: 24,
    borderRadius: 12,
    backgroundColor: shadcnColors.input,
    padding: 2,
    justifyContent: 'center',
  },
  switchChecked: {
    backgroundColor: shadcnColors.primary,
  },
  switchDisabled: {
    opacity: 0.5,
  },
  thumb: {
    width: 20,
    height: 20,
    borderRadius: 10,
    backgroundColor: shadcnColors.background,
    shadowColor: '#000',
    shadowOffset: {width: 0, height: 1},
    shadowOpacity: 0.2,
    shadowRadius: 1,
    elevation: 2,
  },
  thumbChecked: {
    alignSelf: 'flex-end',
  },
});

export default Switch;
