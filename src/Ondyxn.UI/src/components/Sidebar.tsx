/**
 * Ondyxn Browser - Sidebar Component
 * Material You design with Liquid Glass effect
 */

import React, {useState} from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  Dimensions,
} from 'react-native';
import {MaterialColors, GlassColors} from '../theme/colors';
import {Typography, Spacing, BorderRadius} from '../theme/typography';
import {Glass} from './Glass';

const SIDEBAR_WIDTH = 260;

type SidebarSection = 'bookmarks' | 'history' | 'downloads' | 'settings';

interface BookmarkItem {
  id: string;
  title: string;
  url: string;
  favicon?: string;
}

interface HistoryItem {
  id: string;
  title: string;
  url: string;
  timestamp: number;
}

interface DownloadItem {
  id: string;
  fileName: string;
  progress: number;
  status: 'downloading' | 'completed' | 'failed';
  size?: string;
}

interface SidebarProps {
  isVisible: boolean;
  onNavigate: (url: string) => void;
  onClose: () => void;
  bookmarks?: BookmarkItem[];
  history?: HistoryItem[];
  downloads?: DownloadItem[];
}

export const Sidebar: React.FC<SidebarProps> = ({
  isVisible,
  onNavigate,
  onClose,
  bookmarks = [],
  history = [],
  downloads = [],
}) => {
  const [activeSection, setActiveSection] =
    useState<SidebarSection>('bookmarks');

  if (!isVisible) {
    return null;
  }

  const sections: {key: SidebarSection; icon: string; label: string}[] = [
    {key: 'bookmarks', icon: '★', label: 'Bookmarks'},
    {key: 'history', icon: '◷', label: 'History'},
    {key: 'downloads', icon: '↓', label: 'Downloads'},
    {key: 'settings', icon: '⚙', label: 'Settings'},
  ];

  const renderBookmarks = () => (
    <View style={styles.sectionContent}>
      {bookmarks.length === 0 ? (
        <View style={styles.emptyState}>
          <Text style={styles.emptyIcon}>★</Text>
          <Text style={styles.emptyText}>No bookmarks yet</Text>
          <Text style={styles.emptySubtext}>
            Press Ctrl+D to bookmark this page
          </Text>
        </View>
      ) : (
        bookmarks.map(item => (
          <TouchableOpacity
            key={item.id}
            onPress={() => onNavigate(item.url)}
            style={styles.listItem}>
            <Text style={styles.itemIcon}>★</Text>
            <View style={styles.itemText}>
              <Text style={styles.itemTitle} numberOfLines={1}>
                {item.title}
              </Text>
              <Text style={styles.itemSubtitle} numberOfLines={1}>
                {item.url}
              </Text>
            </View>
          </TouchableOpacity>
        ))
      )}
    </View>
  );

  const renderHistory = () => (
    <View style={styles.sectionContent}>
      {history.length === 0 ? (
        <View style={styles.emptyState}>
          <Text style={styles.emptyIcon}>◷</Text>
          <Text style={styles.emptyText}>No history yet</Text>
        </View>
      ) : (
        history.map(item => (
          <TouchableOpacity
            key={item.id}
            onPress={() => onNavigate(item.url)}
            style={styles.listItem}>
            <Text style={styles.itemIcon}>◷</Text>
            <View style={styles.itemText}>
              <Text style={styles.itemTitle} numberOfLines={1}>
                {item.title}
              </Text>
              <Text style={styles.itemSubtitle} numberOfLines={1}>
                {new Date(item.timestamp).toLocaleDateString()}
              </Text>
            </View>
          </TouchableOpacity>
        ))
      )}
    </View>
  );

  const renderDownloads = () => (
    <View style={styles.sectionContent}>
      {downloads.length === 0 ? (
        <View style={styles.emptyState}>
          <Text style={styles.emptyIcon}>↓</Text>
          <Text style={styles.emptyText}>No downloads</Text>
        </View>
      ) : (
        downloads.map(item => (
          <View key={item.id} style={styles.downloadItem}>
            <Text style={styles.itemIcon}>
              {item.status === 'completed' ? '✓' : '↓'}
            </Text>
            <View style={styles.itemText}>
              <Text style={styles.itemTitle} numberOfLines={1}>
                {item.fileName}
              </Text>
              {item.status === 'downloading' && (
                <View style={styles.progressBar}>
                  <View
                    style={[
                      styles.progressFill,
                      {width: `${item.progress * 100}%`},
                    ]}
                  />
                </View>
              )}
              {item.size && (
                <Text style={styles.itemSubtitle}>{item.size}</Text>
              )}
            </View>
          </View>
        ))
      )}
    </View>
  );

  const renderSettings = () => (
    <View style={styles.sectionContent}>
      <Text style={[styles.itemTitle, {paddingHorizontal: 12}]}>
        Settings
      </Text>
      <Text
        style={[
          styles.emptySubtext,
          {paddingHorizontal: 12, marginTop: 8},
        ]}>
        Settings page will open in the main view
      </Text>
    </View>
  );

  const renderContent = () => {
    switch (activeSection) {
      case 'bookmarks':
        return renderBookmarks();
      case 'history':
        return renderHistory();
      case 'downloads':
        return renderDownloads();
      case 'settings':
        return renderSettings();
    }
  };

  return (
    <View style={styles.container}>
      <Glass variant="sidebar" style={styles.sidebar}>
        {/* Section tabs */}
        <View style={styles.sectionTabs}>
          {sections.map(section => (
            <TouchableOpacity
              key={section.key}
              onPress={() => setActiveSection(section.key)}
              style={[
                styles.sectionTab,
                activeSection === section.key && styles.sectionTabActive,
              ]}>
              <Text
                style={[
                  styles.sectionTabIcon,
                  activeSection === section.key &&
                    styles.sectionTabIconActive,
                ]}>
                {section.icon}
              </Text>
              <Text
                style={[
                  styles.sectionTabLabel,
                  activeSection === section.key &&
                    styles.sectionTabLabelActive,
                ]}>
                {section.label}
              </Text>
            </TouchableOpacity>
          ))}
        </View>

        {/* Content */}
        <ScrollView style={styles.contentArea}>
          {renderContent()}
        </ScrollView>
      </Glass>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    position: 'absolute',
    left: 0,
    top: 42,
    bottom: 0,
    width: SIDEBAR_WIDTH,
    zIndex: 100,
  },
  sidebar: {
    flex: 1,
    borderRightWidth: 1,
    borderRightColor: GlassColors.glassBorder,
  },
  sectionTabs: {
    flexDirection: 'row',
    borderBottomWidth: 1,
    borderBottomColor: GlassColors.glassBorder,
    paddingHorizontal: Spacing.sm,
    paddingVertical: Spacing.xs,
    gap: 4,
  },
  sectionTab: {
    flex: 1,
    flexDirection: 'column',
    alignItems: 'center',
    paddingVertical: Spacing.xs,
    borderRadius: BorderRadius.sm,
    gap: 2,
  },
  sectionTabActive: {
    backgroundColor: 'rgba(6, 182, 212, 0.12)',
  },
  sectionTabIcon: {
    fontSize: 14,
    color: MaterialColors.onSurfaceVariant,
  },
  sectionTabIconActive: {
    color: MaterialColors.primary,
  },
  sectionTabLabel: {
    ...Typography.labelSmall,
    color: MaterialColors.onSurfaceVariant,
    fontSize: 9,
  },
  sectionTabLabelActive: {
    color: MaterialColors.primary,
  },
  contentArea: {
    flex: 1,
  },
  sectionContent: {
    padding: Spacing.sm,
    gap: 2,
  },
  listItem: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: Spacing.sm,
    borderRadius: BorderRadius.sm,
    gap: Spacing.sm,
  },
  itemIcon: {
    fontSize: 14,
    color: MaterialColors.primary,
    width: 20,
    textAlign: 'center',
  },
  itemText: {
    flex: 1,
  },
  itemTitle: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurface,
    fontSize: 12,
  },
  itemSubtitle: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    fontSize: 10,
    marginTop: 1,
  },
  downloadItem: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: Spacing.sm,
    borderRadius: BorderRadius.sm,
    gap: Spacing.sm,
  },
  progressBar: {
    height: 3,
    backgroundColor: 'rgba(255,255,255,0.06)',
    borderRadius: 1.5,
    marginTop: 4,
    overflow: 'hidden',
  },
  progressFill: {
    height: '100%',
    backgroundColor: MaterialColors.primary,
    borderRadius: 1.5,
  },
  emptyState: {
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: Spacing.xxxl,
  },
  emptyIcon: {
    fontSize: 32,
    color: MaterialColors.onSurfaceVariant,
    opacity: 0.4,
    marginBottom: Spacing.md,
  },
  emptyText: {
    ...Typography.titleMedium,
    color: MaterialColors.onSurfaceVariant,
    marginBottom: Spacing.xs,
  },
  emptySubtext: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    opacity: 0.6,
  },
});

export default Sidebar;
