/**
 * Ondyxn Browser - Tab Bar Component
 * Ultra-minimal transparent tabs inspired by modern browsers
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
import {MaterialColors, GlassColors} from '../theme/colors';
import {Typography, Spacing, BorderRadius} from '../theme/typography';

const {width: SCREEN_WIDTH} = Dimensions.get('window');
const TAB_HEIGHT = 32;

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

      {/* Tabs - centered */}
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

            {/* Close button - only on hover/active */}
            {tabs.length > 1 && (
              <TouchableOpacity
                onPress={e => {
                  e.stopPropagation?.();
                  onTabClose(tab.id);
                }}
                style={[
                  styles.closeButton,
                  tab.id === activeTabId && styles.closeButtonVisible,
                ]}
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

      {/* Window controls - minimal */}
      <View style={styles.windowControls}>
        <TouchableOpacity style={styles.windowButton}>
          <Text style={styles.windowIcon}>─</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.windowButton}>
          <Text style={styles.windowIcon}>□</Text>
        </TouchableOpacity>
        <TouchableOpacity style={[styles.windowButton, styles.windowClose]}>
          <Text style={[styles.windowIcon, styles.windowCloseIcon]}>×</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
    height: 38,
    backgroundColor: GlassColors.chromeBackground,
    borderBottomWidth: 1,
    borderBottomColor: GlassColors.chromeBorder,
  },
  dragRegion: {
    width: 14,
    height: '100%',
  },
  tabsContainer: {
    flex: 1,
    maxHeight: 38,
  },
  tabsScroll: {
    alignItems: 'center',
    paddingHorizontal: 4,
    gap: 2,
    height: 38,
  },
  tab: {
    flexDirection: 'row',
    alignItems: 'center',
    height: TAB_HEIGHT,
    paddingHorizontal: 12,
    borderRadius: BorderRadius.sm,
    gap: 6,
    maxWidth: 200,
    minWidth: 60,
  },
  tabActive: {
    backgroundColor: GlassColors.tabActive,
  },
  tabTitle: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    fontSize: 12,
    fontWeight: '400',
  },
  tabTitleActive: {
    color: MaterialColors.onSurface,
    fontWeight: '500',
  },
  closeButton: {
    width: 16,
    height: 16,
    borderRadius: 8,
    alignItems: 'center',
    justifyContent: 'center',
    opacity: 0,
  },
  closeButtonVisible: {
    opacity: 0.6,
  },
  closeIcon: {
    fontSize: 14,
    color: MaterialColors.onSurfaceVariant,
    fontWeight: '300',
    marginTop: -1,
  },
  newTabButton: {
    width: 24,
    height: 24,
    borderRadius: 12,
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: 8,
    backgroundColor: 'rgba(255,255,255,0.04)',
  },
  newTabIcon: {
    fontSize: 16,
    color: MaterialColors.onSurfaceVariant,
    fontWeight: '300',
  },
  windowControls: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 0,
    marginRight: 4,
  },
  windowButton: {
    width: 36,
    height: 38,
    alignItems: 'center',
    justifyContent: 'center',
  },
  windowIcon: {
    fontSize: 10,
    color: MaterialColors.onSurfaceVariant,
    fontWeight: '200',
  },
  windowClose: {},
  windowCloseIcon: {
    fontSize: 14,
    fontWeight: '300',
  },
  loadingBar: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    height: 2,
    backgroundColor: 'rgba(255,255,255,0.02)',
  },
  loadingBarFill: {
    height: '100%',
    width: '60%',
    backgroundColor: MaterialColors.primary,
    borderRadius: 1,
  },
});

export default TabBar;
