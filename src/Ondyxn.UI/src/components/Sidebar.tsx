/**
 * Ondyxn Browser - Sidebar Component
 * Transparent Liquid Glass sidebar
 */

import React, {useState} from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
} from 'react-native';
import {MaterialColors, GlassColors} from '../theme/colors';
import {Typography, Spacing, BorderRadius} from '../theme/typography';

const SIDEBAR_WIDTH = 240;

type SidebarSection = 'bookmarks' | 'history' | 'downloads' | 'settings';

interface BookmarkItem {
  id: string;
  title: string;
  url: string;
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
  const [activeSection, setActiveSection] = useState<SidebarSection>('bookmarks');

  if (!isVisible) return null;

  const sections: {key: SidebarSection; icon: string; label: string}[] = [
    {key: 'bookmarks', icon: '★', label: 'Bookmarks'},
    {key: 'history', icon: '◷', label: 'History'},
    {key: 'downloads', icon: '↓', label: 'Downloads'},
    {key: 'settings', icon: '⚙', label: 'Settings'},
  ];

  const renderContent = () => {
    switch (activeSection) {
      case 'bookmarks':
        return bookmarks.length === 0 ? (
          <EmptyState icon="★" text="No bookmarks yet" sub="Press Ctrl+D to bookmark" />
        ) : (
          bookmarks.map(item => (
            <TouchableOpacity key={item.id} onPress={() => onNavigate(item.url)} style={styles.listItem}>
              <Text style={styles.itemIcon}>★</Text>
              <View style={styles.itemText}>
                <Text style={styles.itemTitle} numberOfLines={1}>{item.title}</Text>
                <Text style={styles.itemSubtitle} numberOfLines={1}>{item.url}</Text>
              </View>
            </TouchableOpacity>
          ))
        );
      case 'history':
        return history.length === 0 ? (
          <EmptyState icon="◷" text="No history yet" />
        ) : (
          history.map(item => (
            <TouchableOpacity key={item.id} onPress={() => onNavigate(item.url)} style={styles.listItem}>
              <Text style={styles.itemIcon}>◷</Text>
              <View style={styles.itemText}>
                <Text style={styles.itemTitle} numberOfLines={1}>{item.title}</Text>
                <Text style={styles.itemSubtitle} numberOfLines={1}>
                  {new Date(item.timestamp).toLocaleDateString()}
                </Text>
              </View>
            </TouchableOpacity>
          ))
        );
      case 'downloads':
        return downloads.length === 0 ? (
          <EmptyState icon="↓" text="No downloads" />
        ) : (
          downloads.map(item => (
            <View key={item.id} style={styles.listItem}>
              <Text style={styles.itemIcon}>{item.status === 'completed' ? '✓' : '↓'}</Text>
              <View style={styles.itemText}>
                <Text style={styles.itemTitle} numberOfLines={1}>{item.fileName}</Text>
                {item.size && <Text style={styles.itemSubtitle}>{item.size}</Text>}
              </View>
            </View>
          ))
        );
      case 'settings':
        return (
          <View style={styles.listItem}>
            <Text style={styles.itemIcon}>⚙</Text>
            <View style={styles.itemText}>
              <Text style={styles.itemTitle}>Settings</Text>
              <Text style={styles.itemSubtitle}>Configure browser preferences</Text>
            </View>
          </View>
        );
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.sidebar}>
        {/* Section tabs */}
        <View style={styles.sectionTabs}>
          {sections.map(section => (
            <TouchableOpacity
              key={section.key}
              onPress={() => setActiveSection(section.key)}
              style={[styles.sectionTab, activeSection === section.key && styles.sectionTabActive]}>
              <Text style={[styles.sectionTabIcon, activeSection === section.key && styles.sectionTabIconActive]}>
                {section.icon}
              </Text>
            </TouchableOpacity>
          ))}
        </View>

        {/* Content */}
        <ScrollView style={styles.contentArea}>
          <View style={styles.sectionContent}>
            {renderContent()}
          </View>
        </ScrollView>
      </View>
    </View>
  );
};

const EmptyState: React.FC<{icon: string; text: string; sub?: string}> = ({icon, text, sub}) => (
  <View style={styles.emptyState}>
    <Text style={styles.emptyIcon}>{icon}</Text>
    <Text style={styles.emptyText}>{text}</Text>
    {sub && <Text style={styles.emptySubtext}>{sub}</Text>}
  </View>
);

const styles = StyleSheet.create({
  container: {
    position: 'absolute',
    left: 0,
    top: 0,
    bottom: 0,
    width: SIDEBAR_WIDTH,
    zIndex: 100,
  },
  sidebar: {
    flex: 1,
    backgroundColor: GlassColors.sidebar,
    borderRightWidth: 1,
    borderRightColor: GlassColors.sidebarBorder,
  },
  sectionTabs: {
    flexDirection: 'row',
    borderBottomWidth: 1,
    borderBottomColor: GlassColors.glassBorder,
    paddingHorizontal: Spacing.sm,
    paddingVertical: Spacing.xs,
    gap: 2,
  },
  sectionTab: {
    flex: 1,
    alignItems: 'center',
    paddingVertical: 6,
    borderRadius: BorderRadius.sm,
  },
  sectionTabActive: {
    backgroundColor: 'rgba(6, 182, 212, 0.10)',
  },
  sectionTabIcon: {
    fontSize: 14,
    color: MaterialColors.onSurfaceVariant,
  },
  sectionTabIconActive: {
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
    fontSize: 13,
    color: MaterialColors.primary,
    width: 18,
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
  emptyState: {
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 40,
  },
  emptyIcon: {
    fontSize: 28,
    color: MaterialColors.onSurfaceVariant,
    opacity: 0.3,
    marginBottom: Spacing.sm,
  },
  emptyText: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    opacity: 0.6,
  },
  emptySubtext: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    opacity: 0.4,
    marginTop: 4,
    fontSize: 10,
  },
});

export default Sidebar;
