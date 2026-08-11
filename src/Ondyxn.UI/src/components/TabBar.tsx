/**
 * Ondyxn Browser - Tab Bar Component
 * shadcn/ui design with minimal, clean aesthetic
 */

import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  ScrollView,
  Dimensions,
} from 'react-native';
import {shadcnColors, zinc, shadcnRadius} from '../theme/colors';
import {Badge} from './ui';

const {width: SCREEN_WIDTH} = Dimensions.get('window');
const TAB_HEIGHT = 36;

interface Tab {
  id: string;
  title: string;
  url: string;
  favicon?: string;
  isLoading?: boolean;
}

interface TabBarProps {
  tabs: Tab[];
  activeTabId: string;
  onTabSelect: (id: string) => void;
  onTabClose: (id: string) => void;
  onNewTab: () => void;
}

export const TabBar: React.FC<TabBarProps> = ({
  tabs,
  activeTabId,
  onTabSelect,
  onTabClose,
  onNewTab,
}) => {
  return (
    <View style={styles.container}>
      {/* Drag region / title bar area */}
      <View style={styles.dragRegion} />

      {/* Tabs */}
      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={styles.tabsScroll}
        style={styles.tabsContainer}>
        {tabs.map(tab => (
          <TouchableOpacity
            key={tab.id}
            onPress={() => onTabSelect(tab.id)}
            activeOpacity={0.8}
            style={[
              styles.tab,
              tab.id === activeTabId && styles.tabActive,
            ]}>
            {/* Loading indicator */}
            {tab.isLoading && (
              <View style={styles.loadingBar}>
                <View style={styles.loadingBarFill} />
              </View>
            )}

            <Text
              style={[
                styles.tabTitle,
                tab.id === activeTabId && styles.tabTitleActive,
              ]}
              numberOfLines={1}
              ellipsizeMode="tail">
              {tab.title || 'New Tab'}
            </Text>

            {/* Close button - only on active */}
            {tabs.length > 1 && tab.id === activeTabId && (
              <TouchableOpacity
                onPress={e => {
                  e.stopPropagation?.();
                  onTabClose(tab.id);
                }}
                style={styles.closeButton}
                hitSlop={{top: 8, bottom: 8, left: 8, right: 8}}>
                <Text style={styles.closeIcon}>×</Text>
              </TouchableOpacity>
            )}
          </TouchableOpacity>
        ))}
      </ScrollView>

      {/* New tab button */}
      <TouchableOpacity onPress={onNewTab} style={styles.newTabButton}>
        <Text style={styles.newTabIcon}>+</Text>
      </TouchableOpacity>

      {/* Window controls */}
      <View style={styles.windowControls}>
        <TouchableOpacity style={styles.windowButton}>
          <View style={styles.windowMinimize} />
        </TouchableOpacity>
        <TouchableOpacity style={styles.windowButton}>
          <View style={styles.windowMaximize} />
        </TouchableOpacity>
        <TouchableOpacity style={[styles.windowButton, styles.windowClose]}>
          <View style={styles.windowCloseIcon} />
        </TouchableOpacity>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
    height: 44,
    backgroundColor: shadcnColors.background,
    borderBottomWidth: 1,
    borderBottomColor: shadcnColors.border,
  },
  dragRegion: {
    width: 14,
    height: '100%',
  },
  tabsContainer: {
    flex: 1,
    maxHeight: 44,
  },
  tabsScroll: {
    alignItems: 'center',
    paddingHorizontal: 4,
    gap: 2,
    height: 44,
  },
  tab: {
    flexDirection: 'row',
    alignItems: 'center',
    height: TAB_HEIGHT,
    paddingHorizontal: 14,
    borderRadius: shadcnRadius.sm,
    gap: 8,
    maxWidth: 220,
    minWidth: 80,
  },
  tabActive: {
    backgroundColor: shadcnColors.muted,
  },
  tabTitle: {
    fontSize: 13,
    color: shadcnColors.mutedForeground,
    fontWeight: '400',
  },
  tabTitleActive: {
    color: shadcnColors.foreground,
    fontWeight: '500',
  },
  closeButton: {
    width: 18,
    height: 18,
    borderRadius: 9,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'transparent',
  },
  closeIcon: {
    fontSize: 14,
    color: shadcnColors.mutedForeground,
    fontWeight: '300',
  },
  newTabButton: {
    width: 28,
    height: 28,
    borderRadius: 14,
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: 8,
    backgroundColor: shadcnColors.muted,
  },
  newTabIcon: {
    fontSize: 16,
    color: shadcnColors.mutedForeground,
    fontWeight: '300',
  },
  windowControls: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 0,
    marginRight: 8,
  },
  windowButton: {
    width: 36,
    height: 44,
    alignItems: 'center',
    justifyContent: 'center',
  },
  windowMinimize: {
    width: 10,
    height: 1,
    backgroundColor: shadcnColors.mutedForeground,
  },
  windowMaximize: {
    width: 10,
    height: 10,
    borderRadius: 2,
    borderWidth: 1,
    borderColor: shadcnColors.mutedForeground,
  },
  windowClose: {},
  windowCloseIcon: {
    width: 10,
    height: 10,
    position: 'absolute',
    borderTopWidth: 1.5,
    borderLeftWidth: 1.5,
    borderColor: shadcnColors.mutedForeground,
    transform: [{rotate: '45deg'}],
    marginTop: 0,
  },
  loadingBar: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    height: 2,
    backgroundColor: zinc[800],
  },
  loadingBarFill: {
    height: '100%',
    width: '60%',
    backgroundColor: shadcnColors.foreground,
    borderRadius: 1,
  },
});

export default TabBar;
