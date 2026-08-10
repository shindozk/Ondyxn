/**
 * Ondyxn Browser - New Tab Page
 * Material You design with bento grid layout
 */

import React, {useState} from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  ScrollView,
  TextInput,
  Dimensions,
} from 'react-native';
import {MaterialColors, GlassColors} from '../theme/colors';
import {Typography, Spacing, BorderRadius} from '../theme/typography';
import {Glass} from '../components/Glass';

const {width: SCREEN_WIDTH} = Dimensions.get('window');

interface QuickLink {
  id: string;
  title: string;
  url: string;
  icon: string;
  color: string;
}

interface NewTabPageProps {
  onNavigate: (url: string) => void;
  quickLinks?: QuickLink[];
}

const defaultQuickLinks: QuickLink[] = [
  {id: '1', title: 'Google', url: 'https://google.com', icon: 'G', color: '#4285F4'},
  {id: '2', title: 'YouTube', url: 'https://youtube.com', icon: '▶', color: '#FF0000'},
  {id: '3', title: 'GitHub', url: 'https://github.com', icon: '⌂', color: '#F0F0F0'},
  {id: '4', title: 'Twitter', url: 'https://twitter.com', icon: '𝕏', color: '#1DA1F2'},
  {id: '5', title: 'Reddit', url: 'https://reddit.com', icon: 'R', color: '#FF5700'},
  {id: '6', title: 'Wikipedia', url: 'https://wikipedia.org', icon: 'W', color: '#636466'},
  {id: '7', title: 'Stack Overflow', url: 'https://stackoverflow.com', icon: 'S', color: '#F48024'},
  {id: '8', title: 'Amazon', url: 'https://amazon.com', icon: 'a', color: '#FF9900'},
];

export const NewTabPage: React.FC<NewTabPageProps> = ({
  onNavigate,
  quickLinks = defaultQuickLinks,
}) => {
  const [searchValue, setSearchValue] = useState('');

  const handleSearch = () => {
    if (searchValue.trim()) {
      const query = encodeURIComponent(searchValue.trim());
      onNavigate(`https://www.google.com/search?q=${query}`);
    }
  };

  return (
    <ScrollView
      style={styles.container}
      contentContainerStyle={styles.contentContainer}>
      {/* Hero section */}
      <View style={styles.hero}>
        <Text style={styles.greeting}>Ondyxn</Text>
        <Text style={styles.subtitle}>Fast · Private · Beautiful</Text>
      </View>

      {/* Search bar */}
      <Glass variant="light" rounded="lg" style={styles.searchContainer}>
        <View style={styles.searchInner}>
          <Text style={styles.searchIcon}>🔍</Text>
          <TextInput
            style={styles.searchInput}
            value={searchValue}
            onChangeText={setSearchValue}
            onSubmitEditing={handleSearch}
            placeholder="Search the web..."
            placeholderTextColor={MaterialColors.onSurfaceVariant}
            autoCapitalize="none"
            autoCorrect={false}
            returnKeyType="search"
          />
        </View>
      </Glass>

      {/* Quick links grid */}
      <View style={styles.quickLinksSection}>
        <Text style={styles.sectionTitle}>Quick Links</Text>
        <View style={styles.quickLinksGrid}>
          {quickLinks.map(link => (
            <TouchableOpacity
              key={link.id}
              onPress={() => onNavigate(link.url)}
              activeOpacity={0.7}
              style={styles.quickLinkWrapper}>
              <Glass variant="default" rounded="lg" style={styles.quickLink}>
                <View
                  style={[
                    styles.quickLinkIcon,
                    {backgroundColor: link.color + '20'},
                  ]}>
                  <Text
                    style={[styles.quickLinkIconText, {color: link.color}]}>
                    {link.icon}
                  </Text>
                </View>
                <Text style={styles.quickLinkTitle} numberOfLines={1}>
                  {link.title}
                </Text>
                <Text style={styles.quickLinkUrl} numberOfLines={1}>
                  {new URL(link.url).hostname}
                </Text>
              </Glass>
            </TouchableOpacity>
          ))}
        </View>
      </View>

      {/* Footer */}
      <View style={styles.footer}>
        <Text style={styles.footerText}>
          Built with React Native Windows · Material You
        </Text>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: MaterialColors.background,
  },
  contentContainer: {
    padding: Spacing.xl,
    alignItems: 'center',
  },
  hero: {
    alignItems: 'center',
    marginTop: Spacing.xxxl,
    marginBottom: Spacing.xl,
  },
  greeting: {
    ...Typography.displaySmall,
    color: MaterialColors.onSurface,
    fontWeight: '600',
  },
  subtitle: {
    ...Typography.bodyLarge,
    color: MaterialColors.onSurfaceVariant,
    marginTop: Spacing.xs,
  },
  searchContainer: {
    width: '100%',
    maxWidth: 600,
    height: 48,
    marginBottom: Spacing.xxl,
  },
  searchInner: {
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 16,
    gap: 10,
  },
  searchIcon: {
    fontSize: 16,
  },
  searchInput: {
    flex: 1,
    ...Typography.bodyLarge,
    color: MaterialColors.onSurface,
    padding: 0,
  },
  quickLinksSection: {
    width: '100%',
    maxWidth: 600,
  },
  sectionTitle: {
    ...Typography.titleMedium,
    color: MaterialColors.onSurfaceVariant,
    marginBottom: Spacing.md,
    paddingHorizontal: Spacing.xs,
  },
  quickLinksGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: Spacing.sm,
  },
  quickLinkWrapper: {
    width: (SCREEN_WIDTH - 64 - Spacing.sm * 2) / 4,
  },
  quickLink: {
    alignItems: 'center',
    paddingVertical: Spacing.lg,
    paddingHorizontal: Spacing.sm,
    gap: Spacing.sm,
  },
  quickLinkIcon: {
    width: 40,
    height: 40,
    borderRadius: 20,
    alignItems: 'center',
    justifyContent: 'center',
  },
  quickLinkIconText: {
    fontSize: 18,
    fontWeight: '600',
  },
  quickLinkTitle: {
    ...Typography.labelMedium,
    color: MaterialColors.onSurface,
    fontSize: 11,
  },
  quickLinkUrl: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    fontSize: 9,
  },
  footer: {
    marginTop: Spacing.xxxl,
    paddingBottom: Spacing.xl,
    alignItems: 'center',
  },
  footerText: {
    ...Typography.bodySmall,
    color: MaterialColors.onSurfaceVariant,
    opacity: 0.4,
  },
});

export default NewTabPage;
