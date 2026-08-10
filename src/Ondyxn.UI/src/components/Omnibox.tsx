/**
 * Ondyxn Browser - Omnibox (Address Bar) Component
 * Material You design with Liquid Glass effect
 */

import React, {useState, useRef, useEffect} from 'react';
import {
  View,
  TextInput,
  Text,
  StyleSheet,
  TouchableOpacity,
  Dimensions,
} from 'react-native';
import {MaterialColors, GlassColors} from '../theme/colors';
import {Typography, Spacing, BorderRadius} from '../theme/typography';
import {Glass} from './Glass';

const {width: SCREEN_WIDTH} = Dimensions.get('window');

interface OmniboxProps {
  url: string;
  title?: string;
  isLoading?: boolean;
  isSecure?: boolean;
  onNavigate: (url: string) => void;
  onBack?: () => void;
  onForward?: () => void;
  onReload?: () => void;
  onStop?: () => void;
  canGoBack?: boolean;
  canGoForward?: boolean;
}

export const Omnibox: React.FC<OmniboxProps> = ({
  url,
  title,
  isLoading = false,
  isSecure = false,
  onNavigate,
  onBack,
  onForward,
  onReload,
  onStop,
  canGoBack = false,
  canGoForward = false,
}) => {
  const [inputValue, setInputValue] = useState(url);
  const [isFocused, setIsFocused] = useState(false);
  const inputRef = useRef<TextInput>(null);

  useEffect(() => {
    if (!isFocused) {
      setInputValue(url);
    }
  }, [url, isFocused]);

  const handleSubmit = () => {
    let navigateUrl = inputValue.trim();

    // Auto-add https if no protocol
    if (
      !navigateUrl.startsWith('http://') &&
      !navigateUrl.startsWith('https://') &&
      !navigateUrl.startsWith('about:')
    ) {
      if (navigateUrl.includes('.') && !navigateUrl.includes(' ')) {
        navigateUrl = `https://${navigateUrl}`;
      } else {
        navigateUrl = `https://www.google.com/search?q=${encodeURIComponent(navigateUrl)}`;
      }
    }

    setIsFocused(false);
    inputRef.current?.blur();
    onNavigate(navigateUrl);
  };

  const getDisplayUrl = () => {
    if (isFocused) {
      return inputValue;
    }
    try {
      const parsed = new URL(url);
      return parsed.hostname + parsed.pathname;
    } catch {
      return url;
    }
  };

  const isGoogle = url?.includes('google');
  const isYouTube = url?.includes('youtube');
  const isGitHub = url?.includes('github');

  const getSiteIcon = () => {
    if (isSecure || isGoogle || isYouTube || isGitHub) {
      return '🔒';
    }
    return '🌐';
  };

  return (
    <View style={styles.container}>
      {/* Navigation buttons */}
      <View style={styles.navButtons}>
        <TouchableOpacity
          onPress={onBack}
          disabled={!canGoBack}
          style={[styles.navButton, !canGoBack && styles.navButtonDisabled]}>
          <Text
            style={[
              styles.navIcon,
              !canGoBack && styles.navIconDisabled,
            ]}>
            ‹
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          onPress={onForward}
          disabled={!canGoForward}
          style={[styles.navButton, !canGoForward && styles.navButtonDisabled]}>
          <Text
            style={[
              styles.navIcon,
              !canGoForward && styles.navIconDisabled,
            ]}>
            ›
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          onPress={isLoading ? onStop : onReload}
          style={styles.navButton}>
          <Text style={styles.navIcon}>
            {isLoading ? '✕' : '↻'}
          </Text>
        </TouchableOpacity>
      </View>

      {/* Omnibox input */}
      <Glass variant="omnibox" rounded="full" borderWidth={0} style={styles.omniboxContainer}>
        <TouchableOpacity
          onPress={() => {
            setIsFocused(true);
            inputRef.current?.focus();
          }}
          activeOpacity={0.9}
          style={styles.omniboxInner}>
          {/* Site security icon */}
          <Text style={styles.siteIcon}>{getSiteIcon()}</Text>

          {/* URL input */}
          <TextInput
            ref={inputRef}
            style={styles.input}
            value={isFocused ? inputValue : getDisplayUrl()}
            onChangeText={setInputValue}
            onSubmitEditing={handleSubmit}
            onFocus={() => {
              setIsFocused(true);
              setInputValue(url);
            }}
            onBlur={() => setIsFocused(false)}
            placeholder="Search or enter URL"
            placeholderTextColor={MaterialColors.onSurfaceVariant}
            autoCapitalize="none"
            autoCorrect={false}
            selectTextOnFocus
            returnKeyType="go"
          />

          {/* Loading indicator */}
          {isLoading && (
            <View style={styles.loadingIndicator}>
              <View style={styles.loadingDot} />
            </View>
          )}
        </TouchableOpacity>
      </Glass>

      {/* Action buttons */}
      <View style={styles.actionButtons}>
        <TouchableOpacity style={styles.actionButton}>
          <Text style={styles.actionIcon}>⋯</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: Spacing.md,
    paddingVertical: Spacing.sm,
    gap: Spacing.sm,
  },
  navButtons: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 2,
  },
  navButton: {
    width: 30,
    height: 30,
    borderRadius: 15,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'rgba(255,255,255,0.04)',
  },
  navButtonDisabled: {
    opacity: 0.3,
  },
  navIcon: {
    fontSize: 18,
    color: MaterialColors.onSurface,
    fontWeight: '300',
    marginTop: -2,
  },
  navIconDisabled: {
    color: MaterialColors.onSurfaceVariant,
  },
  omniboxContainer: {
    flex: 1,
    height: 40,
  },
  omniboxInner: {
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 14,
    gap: 8,
  },
  siteIcon: {
    fontSize: 13,
  },
  input: {
    flex: 1,
    ...Typography.bodyMedium,
    color: MaterialColors.onSurface,
    fontSize: 13,
    padding: 0,
  },
  loadingIndicator: {
    width: 16,
    height: 16,
    alignItems: 'center',
    justifyContent: 'center',
  },
  loadingDot: {
    width: 6,
    height: 6,
    borderRadius: 3,
    backgroundColor: MaterialColors.primary,
  },
  actionButtons: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
  },
  actionButton: {
    width: 30,
    height: 30,
    borderRadius: 15,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'rgba(255,255,255,0.04)',
  },
  actionIcon: {
    fontSize: 16,
    color: MaterialColors.onSurfaceVariant,
  },
});

export default Omnibox;
