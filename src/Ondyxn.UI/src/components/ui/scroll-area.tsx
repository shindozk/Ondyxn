/**
 * shadcn/ui ScrollArea Component for React Native
 * Based on https://ui.shadcn.com/docs/components/scroll-area
 */

import React from 'react';
import {ScrollView, StyleSheet, ViewStyle} from 'react-native';

interface ScrollAreaProps {
  children: React.ReactNode;
  horizontal?: boolean;
  showsVerticalScrollIndicator?: boolean;
  showsHorizontalScrollIndicator?: boolean;
  style?: ViewStyle;
  contentContainerStyle?: ViewStyle;
}

export const ScrollArea: React.FC<ScrollAreaProps> = ({
  children,
  horizontal = false,
  showsVerticalScrollIndicator = false,
  showsHorizontalScrollIndicator = false,
  style,
  contentContainerStyle,
}) => {
  return (
    <ScrollView
      horizontal={horizontal}
      showsVerticalScrollIndicator={showsVerticalScrollIndicator}
      showsHorizontalScrollIndicator={showsHorizontalScrollIndicator}
      style={[styles.scrollArea, style]}
      contentContainerStyle={contentContainerStyle}>
      {children}
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  scrollArea: {
    flex: 1,
  },
});

export default ScrollArea;
