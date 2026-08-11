/**
 * shadcn/ui Tabs Component for React Native
 * Based on https://ui.shadcn.com/docs/components/tabs
 */

import React, {createContext, useContext, useState} from 'react';
import {View, Text, TouchableOpacity, StyleSheet, ViewStyle, TextStyle} from 'react-native';
import {shadcnColors, shadcnRadius} from '../../theme/colors';

interface TabsContextValue {
  value: string;
  onValueChange: (value: string) => void;
}

const TabsContext = createContext<TabsContextValue>({
  value: '',
  onValueChange: () => {},
});

interface TabsProps {
  children: React.ReactNode;
  defaultValue: string;
  value?: string;
  onValueChange?: (value: string) => void;
  style?: ViewStyle;
}

export const Tabs: React.FC<TabsProps> = ({
  children,
  defaultValue,
  value: controlledValue,
  onValueChange,
  style,
}) => {
  const [internalValue, setInternalValue] = useState(defaultValue);
  const value = controlledValue ?? internalValue;
  
  const handleValueChange = (newValue: string) => {
    setInternalValue(newValue);
    onValueChange?.(newValue);
  };
  
  return (
    <TabsContext.Provider value={{value, onValueChange: handleValueChange}}>
      <View style={[styles.tabs, style]}>
        {children}
      </View>
    </TabsContext.Provider>
  );
};

interface TabsListProps {
  children: React.ReactNode;
  style?: ViewStyle;
}

export const TabsList: React.FC<TabsListProps> = ({children, style}) => {
  return (
    <View style={[styles.list, style]}>
      {children}
    </View>
  );
};

interface TabsTriggerProps {
  children: React.ReactNode;
  value: string;
  style?: ViewStyle;
}

export const TabsTrigger: React.FC<TabsTriggerProps> = ({
  children,
  value,
  style,
}) => {
  const {value: selectedValue, onValueChange} = useContext(TabsContext);
  const isSelected = selectedValue === value;
  
  return (
    <TouchableOpacity
      onPress={() => onValueChange(value)}
      style={[
        styles.trigger,
        isSelected && styles.triggerSelected,
        style,
      ]}>
      <Text style={[styles.triggerText, isSelected && styles.triggerTextSelected]}>
        {children}
      </Text>
    </TouchableOpacity>
  );
};

interface TabsContentProps {
  children: React.ReactNode;
  value: string;
  style?: ViewStyle;
}

export const TabsContent: React.FC<TabsContentProps> = ({
  children,
  value,
  style,
}) => {
  const {value: selectedValue} = useContext(TabsContext);
  
  if (selectedValue !== value) {
    return null;
  }
  
  return (
    <View style={[styles.content, style]}>
      {children}
    </View>
  );
};

const styles = StyleSheet.create({
  tabs: {
    width: '100%',
  },
  list: {
    flexDirection: 'row',
    backgroundColor: shadcnColors.muted,
    borderRadius: shadcnRadius.md,
    padding: 4,
    gap: 4,
  },
  trigger: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: shadcnRadius.sm - 2,
    paddingVertical: 8,
    paddingHorizontal: 12,
  },
  triggerSelected: {
    backgroundColor: shadcnColors.background,
    shadowColor: '#000',
    shadowOffset: {width: 0, height: 1},
    shadowOpacity: 0.1,
    shadowRadius: 2,
    elevation: 2,
  },
  triggerText: {
    fontSize: 14,
    fontWeight: '500',
    color: shadcnColors.mutedForeground,
  },
  triggerTextSelected: {
    color: shadcnColors.foreground,
  },
  content: {
    marginTop: 8,
  },
});

export default Tabs;
