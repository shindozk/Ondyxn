/**
 * Ondyxn Browser - Omnibox (Address Bar) Component
 * shadcn/ui design with minimal, clean aesthetic
 */

import React, {useState, useRef, useEffect} from 'react';
import {
  View,
  TextInput,
  Text,
  StyleSheet,
  TouchableOpacity,
} from 'react-native';
import {shadcnColors, shadcnRadius} from '../theme/colors';
import {Button, Separator} from './ui';

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
  onHome?: () => void;
  canGoBack?: boolean;
  canGoForward?: boolean;
}

/** Extract hostname from URL without URL API */
function extractHost(url: string): string {
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

/** Extract path from URL without URL API */
function extractPath(url: string): string {
  try {
    let rest = url.replace(/^https?:\/\//, '');
    rest = rest.split(':')[0];
    const pathIdx = rest.indexOf('/');
    return pathIdx >= 0 ? rest.slice(pathIdx) : '/';
  } catch {
    return '/';
  }
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
  onHome,
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

  const hostname = extractHost(url);
  const pathname = extractPath(url);
  const isAboutPage = url.startsWith('about:');

  return (
    <View style={styles.container}>
      {/* Left: Navigation buttons */}
      <View style={styles.navButtons}>
        <TouchableOpacity
          onPress={onBack}
          disabled={!canGoBack}
          style={[styles.navBtn, !canGoBack && styles.navBtnDisabled]}
          hitSlop={{top: 6, bottom: 6, left: 6, right: 6}}>
          <Text style={[styles.navArrow, !canGoBack && styles.navArrowDisabled]}>
            ‹
          </Text>
        </TouchableOpacity>

        <TouchableOpacity
          onPress={onForward}
          disabled={!canGoForward}
          style={[styles.navBtn, !canGoForward && styles.navBtnDisabled]}
          hitSlop={{top: 6, bottom: 6, left: 6, right: 6}}>
          <Text style={[styles.navArrow, !canGoForward && styles.navArrowDisabled]}>
            ›
          </Text>
        </TouchableOpacity>

        <TouchableOpacity
          onPress={isLoading ? onStop : onReload}
          style={styles.navBtn}
          hitSlop={{top: 6, bottom: 6, left: 6, right: 6}}>
          <Text style={styles.navArrow}>
            {isLoading ? '×' : '↻'}
          </Text>
        </TouchableOpacity>

        <TouchableOpacity
          onPress={onHome}
          style={styles.navBtn}
          hitSlop={{top: 6, bottom: 6, left: 6, right: 6}}>
          <Text style={styles.navArrow}>⌂</Text>
        </TouchableOpacity>
      </View>

      <Separator orientation="vertical" style={styles.navSeparator} />

      {/* Center: Omnibox input */}
      <TouchableOpacity
        onPress={() => {
          setIsFocused(true);
          inputRef.current?.focus();
        }}
        activeOpacity={0.9}
        style={[
          styles.omnibox,
          isFocused && styles.omniboxFocused,
        ]}>
        {/* Lock / site icon */}
        {!isAboutPage && (
          <Text style={styles.lockIcon}>
            {isSecure ? '🔒' : '🌐'}
          </Text>
        )}

        {isFocused ? (
          <TextInput
            ref={inputRef}
            style={styles.input}
            value={inputValue}
            onChangeText={setInputValue}
            onSubmitEditing={handleSubmit}
            onFocus={() => setIsFocused(true)}
            onBlur={() => setIsFocused(false)}
            placeholder="Search or enter URL"
            placeholderTextColor={shadcnColors.mutedForeground}
            autoCapitalize="none"
            autoCorrect={false}
            selectTextOnFocus
            returnKeyType="go"
          />
        ) : (
          <View style={styles.urlDisplay}>
            <Text style={styles.urlHost} numberOfLines={1}>
              {isAboutPage ? title || 'New Tab' : hostname}
            </Text>
            {!isAboutPage && (
              <Text style={styles.urlPath} numberOfLines={1}>
                {pathname}
              </Text>
            )}
          </View>
        )}

        {/* Loading spinner */}
        {isLoading && (
          <View style={styles.loadingIndicator}>
            <View style={styles.loadingDot} />
          </View>
        )}
      </TouchableOpacity>

      {/* Right: Window controls spacer */}
      <View style={styles.rightSpacer} />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 12,
    paddingVertical: 8,
    gap: 8,
  },
  navButtons: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 2,
  },
  navSeparator: {
    height: 20,
    marginHorizontal: 4,
  },
  navBtn: {
    width: 32,
    height: 32,
    borderRadius: shadcnRadius.sm,
    alignItems: 'center',
    justifyContent: 'center',
  },
  navBtnDisabled: {
    opacity: 0.3,
  },
  navArrow: {
    fontSize: 18,
    color: shadcnColors.foreground,
    fontWeight: '300',
    marginTop: -1,
  },
  navArrowDisabled: {
    color: shadcnColors.mutedForeground,
  },
  omnibox: {
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    height: 36,
    paddingHorizontal: 14,
    borderRadius: shadcnRadius.md,
    backgroundColor: shadcnColors.muted,
    borderWidth: 1,
    borderColor: shadcnColors.border,
    gap: 8,
  },
  omniboxFocused: {
    backgroundColor: shadcnColors.background,
    borderColor: shadcnColors.ring,
  },
  lockIcon: {
    fontSize: 12,
  },
  input: {
    flex: 1,
    fontSize: 14,
    color: shadcnColors.foreground,
    fontWeight: '400',
    padding: 0,
  },
  urlDisplay: {
    flex: 1,
    flexDirection: 'row',
    alignItems: 'center',
    gap: 0,
  },
  urlHost: {
    fontSize: 14,
    color: shadcnColors.foreground,
    fontWeight: '400',
  },
  urlPath: {
    fontSize: 14,
    color: shadcnColors.mutedForeground,
    fontWeight: '400',
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
    backgroundColor: shadcnColors.primary,
  },
  rightSpacer: {
    width: 108,
  },
});

export default Omnibox;
