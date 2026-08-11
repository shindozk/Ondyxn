/**
 * shadcn/ui Separator Component for React Native
 * Based on https://ui.shadcn.com/docs/components/separator
 */

import React from 'react';
import {View, StyleSheet, ViewStyle} from 'react-native';
import {shadcnColors} from '../../theme/colors';

interface SeparatorProps {
  orientation?: 'horizontal' | 'vertical';
  style?: ViewStyle;
}

export const Separator: React.FC<SeparatorProps> = ({
  orientation = 'horizontal',
  style,
}) => {
  return (
    <View
      style={[
        styles.separator,
        orientation === 'horizontal' ? styles.horizontal : styles.vertical,
        style,
      ]}
    />
  );
};

const styles = StyleSheet.create({
  separator: {
    backgroundColor: shadcnColors.border,
  },
  horizontal: {
    height: 1,
    width: '100%',
  },
  vertical: {
    width: 1,
    height: '100%',
  },
});

export default Separator;
