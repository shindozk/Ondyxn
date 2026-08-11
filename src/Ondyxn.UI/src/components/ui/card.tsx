/**
 * shadcn/ui Card Component for React Native
 * Based on https://ui.shadcn.com/docs/components/card
 */

import React from 'react';
import {View, Text, StyleSheet, ViewStyle, TextStyle} from 'react-native';
import {shadcnColors, shadcnRadius} from '../../theme/colors';

interface CardProps {
  children: React.ReactNode;
  style?: ViewStyle;
}

interface CardHeaderProps {
  children: React.ReactNode;
  style?: ViewStyle;
}

interface CardTitleProps {
  children: React.ReactNode;
  style?: TextStyle;
}

interface CardDescriptionProps {
  children: React.ReactNode;
  style?: TextStyle;
}

interface CardContentProps {
  children: React.ReactNode;
  style?: ViewStyle;
}

interface CardFooterProps {
  children: React.ReactNode;
  style?: ViewStyle;
}

export const Card: React.FC<CardProps> = ({children, style}) => {
  return (
    <View style={[styles.card, style]}>
      {children}
    </View>
  );
};

export const CardHeader: React.FC<CardHeaderProps> = ({children, style}) => {
  return (
    <View style={[styles.header, style]}>
      {children}
    </View>
  );
};

export const CardTitle: React.FC<CardTitleProps> = ({children, style}) => {
  return (
    <Text style={[styles.title, style]}>
      {children}
    </Text>
  );
};

export const CardDescription: React.FC<CardDescriptionProps> = ({children, style}) => {
  return (
    <Text style={[styles.description, style]}>
      {children}
    </Text>
  );
};

export const CardContent: React.FC<CardContentProps> = ({children, style}) => {
  return (
    <View style={[styles.content, style]}>
      {children}
    </View>
  );
};

export const CardFooter: React.FC<CardFooterProps> = ({children, style}) => {
  return (
    <View style={[styles.footer, style]}>
      {children}
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: shadcnColors.card,
    borderRadius: shadcnRadius.lg,
    borderWidth: 1,
    borderColor: shadcnColors.border,
  },
  header: {
    flexDirection: 'column',
    gap: 6,
    padding: 24,
    paddingBottom: 0,
  },
  title: {
    fontSize: 24,
    fontWeight: '600',
    color: shadcnColors.cardForeground,
    letterSpacing: -0.5,
  },
  description: {
    fontSize: 14,
    color: shadcnColors.mutedForeground,
  },
  content: {
    padding: 24,
    paddingTop: 16,
  },
  footer: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 24,
    paddingTop: 0,
  },
});

export default Card;
