/**
 * Ondyxn Browser - New Tab Page
 * shadcn/ui design with minimal, clean aesthetic
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
import {shadcnColors, zinc, shadcnRadius} from '../theme/colors';
import {Card, CardContent, Input, Button} from '../components/ui';

const {width: SCREEN_WIDTH} = Dimensions.get('window');

/** Extract hostname from URL string without using URL API */
function extractHostname(url: string): string {
  try {
    let host = url.replace(/^https?:\/\//, '');
    host = host.split('/')[0];
    host = host.split(':')[0];
    if (host.startsWith('www.')) host = host.slice(4);
    return host;
  } catch {
    return url;
  }
}

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
      <View style={styles.searchContainer}>
        <Input
          value={searchValue}
          onChangeText={setSearchValue}
          placeholder="Search the web..."
          onSubmitEditing={handleSearch}
          returnKeyType="search"
        />
      </View>

      {/* Quick links grid */}
      <View style={styles.quickLinksSection}>
        <View style={styles.quickLinksGrid}>
          {quickLinks.map(link => (
            <TouchableOpacity
              key={link.id}
              onPress={() => onNavigate(link.url)}
              activeOpacity={0.7}
              style={styles.quickLinkWrapper}>
              <Card style={styles.quickLink}>
                <CardContent style={styles.quickLinkContent}>
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
                    {extractHostname(link.url)}
                  </Text>
                </CardContent>
              </Card>
            </TouchableOpacity>
          ))}
        </View>
      </View>

      {/* Footer */}
      <View style={styles.footer}>
        <Text style={styles.footerText}>
          Built with React Native Windows · shadcn/ui
        </Text>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: shadcnColors.background,
  },
  contentContainer: {
    padding: 24,
    alignItems: 'center',
  },
  hero: {
    alignItems: 'center',
    marginTop: 60,
    marginBottom: 24,
  },
  greeting: {
    fontSize: 36,
    fontWeight: '600',
    color: shadcnColors.foreground,
    letterSpacing: -0.5,
  },
  subtitle: {
    fontSize: 16,
    color: shadcnColors.mutedForeground,
    marginTop: 8,
  },
  searchContainer: {
    width: '100%',
    maxWidth: 560,
    marginBottom: 48,
  },
  quickLinksSection: {
    width: '100%',
    maxWidth: 560,
  },
  quickLinksGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 12,
  },
  quickLinkWrapper: {
    width: (560 - 12 * 3) / 4,
  },
  quickLink: {
    margin: 0,
  },
  quickLinkContent: {
    alignItems: 'center',
    paddingVertical: 16,
    paddingHorizontal: 8,
    gap: 8,
  },
  quickLinkIcon: {
    width: 40,
    height: 40,
    borderRadius: 20,
    alignItems: 'center',
    justifyContent: 'center',
  },
  quickLinkIconText: {
    fontSize: 16,
    fontWeight: '600',
  },
  quickLinkTitle: {
    fontSize: 12,
    fontWeight: '500',
    color: shadcnColors.foreground,
  },
  quickLinkUrl: {
    fontSize: 10,
    color: shadcnColors.mutedForeground,
  },
  footer: {
    marginTop: 60,
    paddingBottom: 24,
    alignItems: 'center',
  },
  footerText: {
    fontSize: 12,
    color: shadcnColors.mutedForeground,
    opacity: 0.5,
  },
});

export default NewTabPage;
