/**
 * Ondyxn Browser - Settings Page
 * Material You design with form controls
 */

import React, {useState} from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  TouchableOpacity,
  Switch,
  Dimensions,
} from 'react-native';
import {MaterialColors, GlassColors} from '../theme/colors';
import {Typography, Spacing, BorderRadius} from '../theme/typography';
import {Glass} from '../components/Glass';

interface SettingsProps {
  onBack: () => void;
}

interface SettingItem {
  id: string;
  label: string;
  type: 'toggle' | 'select' | 'info';
  value?: boolean | string;
  options?: string[];
  selectedOption?: string;
}

interface SettingSection {
  title: string;
  icon: string;
  items: SettingItem[];
}

export const SettingsPage: React.FC<SettingsProps> = ({onBack}) => {
  const [settings, setSettings] = useState<Record<string, any>>({
    theme: 'dark',
    searchEngine: 'Google',
    homepage: 'ondyxn://newtab',
    adBlockEnabled: true,
    trackerBlockEnabled: true,
    hardwareAcceleration: true,
    smoothScrolling: true,
    restoreSession: true,
    showSidebar: false,
  });

  const toggleSetting = (key: string) => {
    setSettings(prev => ({...prev, [key]: !prev[key]}));
  };

  const sections: SettingSection[] = [
    {
      title: 'Appearance',
      icon: '🎨',
      items: [
        {id: 'theme', label: 'Theme', type: 'select', options: ['System', 'Light', 'Dark'], selectedOption: settings.theme === 'dark' ? 'Dark' : settings.theme === 'light' ? 'Light' : 'System'},
        {id: 'accentColor', label: 'Accent Color', type: 'select', options: ['Cyan', 'Purple', 'Rose', 'Green', 'Orange'], selectedOption: 'Cyan'},
      ],
    },
    {
      title: 'Search',
      icon: '🔍',
      items: [
        {id: 'searchEngine', label: 'Default Search Engine', type: 'select', options: ['Google', 'DuckDuckGo', 'Bing', 'Brave'], selectedOption: settings.searchEngine},
        {id: 'homepage', label: 'Homepage', type: 'info', value: settings.homepage},
      ],
    },
    {
      title: 'Privacy & Security',
      icon: '🔒',
      items: [
        {id: 'adBlockEnabled', label: 'Ad Blocker', type: 'toggle', value: settings.adBlockEnabled},
        {id: 'trackerBlockEnabled', label: 'Tracker Blocker', type: 'toggle', value: settings.trackerBlockEnabled},
        {id: 'clearData', label: 'Clear Browsing Data', type: 'info', value: 'History, Cookies, Cache'},
      ],
    },
    {
      title: 'Performance',
      icon: '⚡',
      items: [
        {id: 'hardwareAcceleration', label: 'Hardware Acceleration', type: 'toggle', value: settings.hardwareAcceleration},
        {id: 'smoothScrolling', label: 'Smooth Scrolling', type: 'toggle', value: settings.smoothScrolling},
      ],
    },
    {
      title: 'Startup',
      icon: '🚀',
      items: [
        {id: 'restoreSession', label: 'Restore Previous Session', type: 'toggle', value: settings.restoreSession},
        {id: 'showSidebar', label: 'Show Sidebar on Start', type: 'toggle', value: settings.showSidebar},
      ],
    },
  ];

  return (
    <View style={styles.container}>
      {/* Header */}
      <View style={styles.header}>
        <TouchableOpacity onPress={onBack} style={styles.backButton}>
          <Text style={styles.backIcon}>‹</Text>
        </TouchableOpacity>
        <Text style={styles.headerTitle}>Settings</Text>
      </View>

      <ScrollView contentContainerStyle={styles.content}>
        {sections.map(section => (
          <View key={section.title} style={styles.section}>
            <View style={styles.sectionHeader}>
              <Text style={styles.sectionIcon}>{section.icon}</Text>
              <Text style={styles.sectionTitle}>{section.title}</Text>
            </View>

            <Glass variant="default" rounded="lg" style={styles.sectionContent}>
              {section.items.map((item, index) => (
                <View
                  key={item.id}
                  style={[
                    styles.settingItem,
                    index < section.items.length - 1 && styles.settingItemBorder,
                  ]}>
                  <Text style={styles.settingLabel}>{item.label}</Text>

                  {item.type === 'toggle' && (
                    <Switch
                      value={item.value as boolean}
                      onValueChange={() => toggleSetting(item.id)}
                      trackColor={{
                        false: '#3f3f46',
                        true: MaterialColors.primary + '60',
                      }}
                      thumbColor={
                        item.value ? MaterialColors.primary : '#71717a'
                      }
                    />
                  )}

                  {item.type === 'select' && (
                    <View style={styles.selectContainer}>
                      <Text style={styles.selectValue}>
                        {item.selectedOption}
                      </Text>
                      <Text style={styles.selectArrow}>›</Text>
                    </View>
                  )}

                  {item.type === 'info' && (
                    <View style={styles.infoContainer}>
                      <Text style={styles.infoValue} numberOfLines={1}>
                        {typeof item.value === 'string' ? item.value : ''}
                      </Text>
                    </View>
                  )}
                </View>
              ))}
            </Glass>
          </View>
        ))}

        {/* Version info */}
        <View style={styles.versionInfo}>
          <Text style={styles.versionText}>Ondyxn Browser v1.0.0</Text>
          <Text style={styles.versionSubtext}>
            Built with React Native Windows
          </Text>
        </View>
      </ScrollView>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: MaterialColors.background,
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: Spacing.lg,
    paddingVertical: Spacing.md,
    borderBottomWidth: 1,
    borderBottomColor: GlassColors.glassBorder,
    gap: Spacing.md,
  },
  backButton: {
    width: 32,
    height: 32,
    borderRadius: 16,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'rgba(255,255,255,0.04)',
  },
  backIcon: {
    fontSize: 20,
    color: MaterialColors.onSurface,
    fontWeight: '300',
  },
  headerTitle: {
    ...Typography.titleLarge,
    color: MaterialColors.onSurface,
  },
  content: {
    padding: Spacing.lg,
    gap: Spacing.xl,
  },
  section: {
    gap: Spacing.sm,
  },
  sectionHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: Spacing.xs,
    gap: Spacing.sm,
  },
  sectionIcon: {
    fontSize: 14,
  },
  sectionTitle: {
    ...Typography.titleSmall,
    color: MaterialColors.onSurfaceVariant,
  },
  sectionContent: {
    padding: 0,
  },
  settingItem: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingHorizontal: Spacing.lg,
    paddingVertical: Spacing.md,
  },
  settingItemBorder: {
    borderBottomWidth: 1,
    borderBottomColor: GlassColors.glassBorder,
  },
  settingLabel: {
    ...Typography.bodyMedium,
    color: MaterialColors.onSurface,
  },
  selectContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: Spacing.xs,
  },
  selectValue: {
    ...Typography.bodySmall,
    color: MaterialColors.primary,
  },
  selectArrow: {
    fontSize: 16,
    color: MaterialColors.onSurfaceVariant,
  },
  infoContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  infoValue: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    maxWidth: 200,
  },
  versionInfo: {
    alignItems: 'center',
    paddingVertical: Spacing.xxl,
    gap: Spacing.xs,
  },
  versionText: {
    ...Typography.bodyMedium,
    color: MaterialColors.onSurfaceVariant,
    opacity: 0.5,
  },
  versionSubtext: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    opacity: 0.3,
  },
});

export default SettingsPage;
