/**
 * Ondyxn Browser - Settings Page
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
import {shadcnColors, shadcnRadius} from '../theme/colors';
import {Card, CardHeader, CardTitle, CardContent, Separator, Switch, Badge} from '../components/ui';

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
        {id: 'accentColor', label: 'Accent Color', type: 'select', options: ['Zinc', 'Blue', 'Green', 'Red'], selectedOption: 'Zinc'},
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
          <Card key={section.title} style={styles.section}>
            <CardHeader>
              <CardTitle style={styles.sectionTitle}>
                <Text>{section.icon} </Text>
                {section.title}
              </CardTitle>
            </CardHeader>
            <CardContent style={styles.sectionContent}>
              {section.items.map((item, index) => (
                <View key={item.id}>
                  <View style={styles.settingItem}>
                    <Text style={styles.settingLabel}>{item.label}</Text>

                    {item.type === 'toggle' && (
                      <Switch
                        checked={item.value as boolean}
                        onCheckedChange={() => toggleSetting(item.id)}
                      />
                    )}

                    {item.type === 'select' && (
                      <TouchableOpacity style={styles.selectContainer}>
                        <Text style={styles.selectValue}>
                          {item.selectedOption}
                        </Text>
                        <Text style={styles.selectArrow}>›</Text>
                      </TouchableOpacity>
                    )}

                    {item.type === 'info' && (
                      <View style={styles.infoContainer}>
                        <Badge variant="secondary">
                          {typeof item.value === 'string' ? item.value : ''}
                        </Badge>
                      </View>
                    )}
                  </View>
                  {index < section.items.length - 1 && (
                    <Separator />
                  )}
                </View>
              ))}
            </CardContent>
          </Card>
        ))}

        {/* Version info */}
        <View style={styles.versionInfo}>
          <Text style={styles.versionText}>Ondyxn Browser v1.0.0</Text>
          <Text style={styles.versionSubtext}>
            Built with React Native Windows · shadcn/ui
          </Text>
        </View>
      </ScrollView>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: shadcnColors.background,
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: shadcnColors.border,
    gap: 12,
  },
  backButton: {
    width: 32,
    height: 32,
    borderRadius: shadcnRadius.sm,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: shadcnColors.muted,
  },
  backIcon: {
    fontSize: 18,
    color: shadcnColors.foreground,
    fontWeight: '300',
  },
  headerTitle: {
    fontSize: 16,
    fontWeight: '500',
    color: shadcnColors.foreground,
  },
  content: {
    padding: 16,
    gap: 16,
  },
  section: {
    margin: 0,
  },
  sectionTitle: {
    fontSize: 14,
    fontWeight: '500',
    flexDirection: 'row',
    alignItems: 'center',
  },
  sectionContent: {
    padding: 0,
  },
  settingItem: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 12,
    paddingHorizontal: 24,
  },
  settingLabel: {
    fontSize: 14,
    color: shadcnColors.foreground,
  },
  selectContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
  },
  selectValue: {
    fontSize: 13,
    color: shadcnColors.mutedForeground,
  },
  selectArrow: {
    fontSize: 16,
    color: shadcnColors.mutedForeground,
    fontWeight: '300',
  },
  infoContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  versionInfo: {
    alignItems: 'center',
    paddingVertical: 32,
    gap: 4,
  },
  versionText: {
    fontSize: 12,
    color: shadcnColors.mutedForeground,
    opacity: 0.5,
  },
  versionSubtext: {
    fontSize: 11,
    color: shadcnColors.mutedForeground,
    opacity: 0.3,
  },
});

export default SettingsPage;
