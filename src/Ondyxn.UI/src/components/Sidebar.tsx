/**
 * Ondyxn Browser - Sidebar Component
 * shadcn/ui design with minimal, clean aesthetic
 */

import React, {useState} from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
} from 'react-native';
import {shadcnColors, zinc, shadcnRadius} from '../theme/colors';
import {Badge, Separator, ScrollArea} from './ui';

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

        <Separator />

        {/* Content */}
        <ScrollArea style={styles.contentArea}>
          <View style={styles.sectionContent}>
            {renderContent()}
          </View>
        </ScrollArea>
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
    backgroundColor: shadcnColors.background,
    borderRightWidth: 1,
    borderRightColor: shadcnColors.border,
  },
  sectionTabs: {
    flexDirection: 'row',
    padding: 8,
    gap: 4,
  },
  sectionTab: {
    flex: 1,
    alignItems: 'center',
    paddingVertical: 8,
    borderRadius: shadcnRadius.sm,
  },
  sectionTabActive: {
    backgroundColor: shadcnColors.muted,
  },
  sectionTabIcon: {
    fontSize: 14,
    color: shadcnColors.mutedForeground,
  },
  sectionTabIconActive: {
    color: shadcnColors.foreground,
  },
  contentArea: {
    flex: 1,
  },
  sectionContent: {
    padding: 8,
    gap: 2,
  },
  listItem: {
    flexDirection: 'row',
    alignItems: 'center',
    padding: 10,
    borderRadius: shadcnRadius.sm,
    gap: 10,
  },
  itemIcon: {
    fontSize: 13,
    color: shadcnColors.mutedForeground,
    width: 18,
    textAlign: 'center',
  },
  itemText: {
    flex: 1,
  },
  itemTitle: {
    fontSize: 13,
    color: shadcnColors.foreground,
    fontWeight: '400',
  },
  itemSubtitle: {
    fontSize: 11,
    color: shadcnColors.mutedForeground,
    marginTop: 2,
  },
  emptyState: {
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 40,
  },
  emptyIcon: {
    fontSize: 24,
    color: shadcnColors.mutedForeground,
    opacity: 0.4,
    marginBottom: 12,
  },
  emptyText: {
    fontSize: 13,
    color: shadcnColors.mutedForeground,
    opacity: 0.7,
  },
  emptySubtext: {
    fontSize: 11,
    color: shadcnColors.mutedForeground,
    opacity: 0.5,
    marginTop: 4,
  },
});

export default Sidebar;
