/**
 * Ondyxn Browser - Tab Bar Component
 * Material You design with Liquid Glass tabs
 */

import React, {useState} from 'react';
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
import {Glass} from './Glass';

const {width: SCREEN_WIDTH} = Dimensions.get('window');
const TAB_MAX_WIDTH = 200;
const TAB_MIN_WIDTH = 60;
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
  const tabCount = tabs.length;
  const tabWidth = Math.min(
    TAB_MAX_WIDTH,
    Math.max(TAB_MIN_WIDTH, (SCREEN_WIDTH - 100) / Math.min(tabCount, 8)),
  );

  return (
    <View style={styles.container}>
      {/* Traffic light area (window controls) */}
      <View style={styles.windowControls}>
        <View style={[styles.trafficLight, {backgroundColor: '#EF4444'}]} />
        <View style={[styles.trafficLight, {backgroundColor: '#F59E0B'}]} />
        <View style={[styles.trafficLight, {backgroundColor: '#10B981'}]} />
      </View>

      {/* Tabs scroll area */}
      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={styles.tabsScroll}
        style={styles.tabsContainer}>
        {tabs.map(tab => (
          <TouchableOpacity
            key={tab.id}
            onPress={() => onTabSelect(tab.id)}
            activeOpacity={0.7}
            style={[
              styles.tab,
              {
                width: tabWidth,
                backgroundColor:
                  tab.id === activeTabId
                    ? 'rgba(255,255,255,0.08)'
                    : 'transparent',
                borderColor:
                  tab.id === activeTabId
                    ? 'rgba(255,255,255,0.12)'
                    : 'transparent',
              },
            ]}>
            {/* Loading indicator bar */}
            {tab.isLoading && (
              <View style={styles.loadingBar}>
                <View style={styles.loadingBarFill} />
              </View>
            )}

            <Text
              style={[
                styles.tabTitle,
                {
                  color:
                    tab.id === activeTabId
                      ? MaterialColors.onSurface
                      : MaterialColors.onSurfaceVariant,
                },
              ]}
              numberOfLines={1}
              ellipsizeMode="tail">
              {tab.title || 'New Tab'}
            </Text>

            {/* Close button */}
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
          </TouchableOpacity>
        ))}
      </ScrollView>

      {/* New tab button */}
      <TouchableOpacity onPress={onNewTab} style={styles.newTabButton}>
        <Text style={styles.newTabIcon}>+</Text>
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
    height: 42,
    backgroundColor: MaterialColors.surface,
    borderBottomWidth: 1,
    borderBottomColor: GlassColors.glassBorder,
  },
  windowControls: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 7,
    paddingHorizontal: 14,
  },
  trafficLight: {
    width: 12,
    height: 12,
    borderRadius: 6,
  },
  tabsContainer: {
    flex: 1,
  },
  tabsScroll: {
    alignItems: 'center',
    paddingHorizontal: 4,
    gap: 2,
  },
  tab: {
    flexDirection: 'row',
    alignItems: 'center',
    height: TAB_HEIGHT,
    paddingHorizontal: 10,
    borderRadius: BorderRadius.sm,
    borderWidth: 1,
    marginHorizontal: 1,
    overflow: 'hidden',
  },
  tabTitle: {
    ...Typography.bodySmall,
    flex: 1,
    fontSize: 12,
  },
  closeButton: {
    width: 16,
    height: 16,
    borderRadius: 8,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'rgba(255,255,255,0.06)',
    marginLeft: 6,
    opacity: 0,
  },
  closeButtonVisible: {
    opacity: 1,
  },
  closeIcon: {
    fontSize: 12,
    color: MaterialColors.onSurfaceVariant,
    fontWeight: '600',
    marginTop: -1,
  },
  newTabButton: {
    width: 28,
    height: 28,
    borderRadius: 14,
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: 12,
    backgroundColor: 'rgba(255,255,255,0.04)',
  },
  newTabIcon: {
    fontSize: 18,
    color: MaterialColors.onSurfaceVariant,
    fontWeight: '300',
  },
  loadingBar: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    height: 2,
    backgroundColor: 'rgba(255,255,255,0.04)',
  },
  loadingBarFill: {
    height: '100%',
    width: '60%',
    backgroundColor: MaterialColors.primary,
    borderRadius: 1,
  },
});

export default TabBar;
