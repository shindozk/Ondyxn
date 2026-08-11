/**
 * Ondyxn Browser - Main Application
 * Ultra-transparent Liquid Glass design with Material You
 */

import React, {useState, useCallback} from 'react';
import {View, StyleSheet, StatusBar, Dimensions} from 'react-native';
import {MaterialColors, GlassColors} from './src/theme/colors';
import {TabBar} from './src/components/TabBar';
import {Omnibox} from './src/components/Omnibox';
import {Sidebar} from './src/components/Sidebar';
import {NewTabPage} from './src/screens/NewTabPage';
import {SettingsPage} from './src/screens/SettingsPage';

const {width: SCREEN_WIDTH, height: SCREEN_HEIGHT} = Dimensions.get('window');

interface Tab {
  id: string;
  title: string;
  url: string;
  isLoading: boolean;
  canGoBack: boolean;
  canGoForward: boolean;
}

const generateId = () => Math.random().toString(36).substr(2, 9);

const App: React.FC = () => {
  const [tabs, setTabs] = useState<Tab[]>([
    {
      id: generateId(),
      title: 'New Tab',
      url: 'about:newtab',
      isLoading: false,
      canGoBack: false,
      canGoForward: false,
    },
  ]);
  const [activeTabId, setActiveTabId] = useState(tabs[0].id);
  const [sidebarVisible, setSidebarVisible] = useState(false);
  const [showSettings, setShowSettings] = useState(false);

  const activeTab = tabs.find(t => t.id === activeTabId) || tabs[0];

  const updateTab = useCallback(
    (id: string, updates: Partial<Tab>) => {
      setTabs(prev => prev.map(tab => (tab.id === id ? {...tab, ...updates} : tab)));
    },
    [],
  );

  const handleNavigate = useCallback(
    (url: string) => {
      updateTab(activeTabId, {
        url,
        title: extractHost(url),
        isLoading: true,
      });
    },
    [activeTabId, updateTab],
  );

  const handleNewTab = useCallback(() => {
    const newTab: Tab = {
      id: generateId(),
      title: 'New Tab',
      url: 'about:newtab',
      isLoading: false,
      canGoBack: false,
      canGoForward: false,
    };
    setTabs(prev => [...prev, newTab]);
    setActiveTabId(newTab.id);
  }, []);

  const handleCloseTab = useCallback(
    (id: string) => {
      setTabs(prev => {
        const filtered = prev.filter(t => t.id !== id);
        if (filtered.length === 0) {
          const newTab: Tab = {
            id: generateId(),
            title: 'New Tab',
            url: 'about:newtab',
            isLoading: false,
            canGoBack: false,
            canGoForward: false,
          };
          setActiveTabId(newTab.id);
          return [newTab];
        }
        if (id === activeTabId) {
          const currentIndex = prev.findIndex(t => t.id === id);
          const newIndex = Math.min(currentIndex, filtered.length - 1);
          setActiveTabId(filtered[newIndex].id);
        }
        return filtered;
      });
    },
    [activeTabId],
  );

  const handleBack = useCallback(() => {
    updateTab(activeTabId, {canGoBack: false});
  }, [activeTabId, updateTab]);

  const handleForward = useCallback(() => {
    updateTab(activeTabId, {canGoForward: false});
  }, [activeTabId, updateTab]);

  const handleReload = useCallback(() => {
    updateTab(activeTabId, {isLoading: true});
    setTimeout(() => updateTab(activeTabId, {isLoading: false}), 1500);
  }, [activeTabId, updateTab]);

  const handleStop = useCallback(() => {
    updateTab(activeTabId, {isLoading: false});
  }, [activeTabId, updateTab]);

  const handleHome = useCallback(() => {
    handleNavigate('about:newtab');
  }, [handleNavigate]);

  const renderContent = () => {
    if (showSettings) {
      return <SettingsPage onBack={() => setShowSettings(false)} />;
    }

    if (activeTab.url === 'about:newtab') {
      return <NewTabPage onNavigate={handleNavigate} />;
    }

    // WebView2 native module would render here
    return (
      <View style={styles.webPlaceholder}>
        <View style={styles.webPlaceholderContent}>
          <View style={styles.loadingSpinner} />
        </View>
      </View>
    );
  };

  return (
    <View style={styles.container}>
      <StatusBar barStyle="light-content" translucent backgroundColor="transparent" />

      {/* Tab bar */}
      <TabBar
        tabs={tabs}
        activeTabId={activeTabId}
        onTabSelect={setActiveTabId}
        onTabClose={handleCloseTab}
        onNewTab={handleNewTab}
      />

      {/* Omnibox / Address bar */}
      <Omnibox
        url={activeTab.url}
        title={activeTab.title}
        isLoading={activeTab.isLoading}
        isSecure={activeTab.url.startsWith('https://')}
        onNavigate={handleNavigate}
        onBack={handleBack}
        onForward={handleForward}
        onReload={handleReload}
        onStop={handleStop}
        onHome={handleHome}
        canGoBack={activeTab.canGoBack}
        canGoForward={activeTab.canGoForward}
      />

      {/* Main content area with rounded corners */}
      <View style={styles.contentWrapper}>
        <View style={styles.contentArea}>
          {/* Sidebar overlay */}
          {sidebarVisible && (
            <Sidebar
              isVisible={sidebarVisible}
              onNavigate={handleNavigate}
              onClose={() => setSidebarVisible(false)}
            />
          )}

          {/* Web content / New tab / Settings */}
          {renderContent()}
        </View>
      </View>
    </View>
  );
};

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

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: MaterialColors.background,
  },
  contentWrapper: {
    flex: 1,
    paddingHorizontal: 8,
    paddingBottom: 8,
  },
  contentArea: {
    flex: 1,
    backgroundColor: MaterialColors.surface,
    borderRadius: 12,
    overflow: 'hidden',
  },
  webPlaceholder: {
    flex: 1,
    backgroundColor: MaterialColors.background,
    alignItems: 'center',
    justifyContent: 'center',
  },
  webPlaceholderContent: {
    alignItems: 'center',
    gap: 16,
  },
  loadingSpinner: {
    width: 24,
    height: 24,
    borderRadius: 12,
    borderWidth: 2,
    borderColor: 'rgba(255,255,255,0.08)',
    borderTopColor: MaterialColors.primary,
  },
});

export default App;
