/**
 * Ondyxn Browser - Main Application
 * Material You design with Liquid Glass effects
 * Built with React Native Windows
 */

import React, {useState, useCallback} from 'react';
import {View, StyleSheet, SafeAreaView, StatusBar, Dimensions} from 'react-native';
import {MaterialColors} from './src/theme/colors';
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
        title: getDomainFromUrl(url),
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
    // Simulate back navigation
    updateTab(activeTabId, {canGoBack: false});
  }, [activeTabId, updateTab]);

  const handleForward = useCallback(() => {
    // Simulate forward navigation
    updateTab(activeTabId, {canGoForward: false});
  }, [activeTabId, updateTab]);

  const handleReload = useCallback(() => {
    updateTab(activeTabId, {isLoading: true});
    setTimeout(() => {
      updateTab(activeTabId, {isLoading: false});
    }, 1500);
  }, [activeTabId, updateTab]);

  const handleStop = useCallback(() => {
    updateTab(activeTabId, {isLoading: false});
  }, [activeTabId, updateTab]);

  const renderContent = () => {
    if (showSettings) {
      return <SettingsPage onBack={() => setShowSettings(false)} />;
    }

    if (activeTab.url === 'about:newtab') {
      return <NewTabPage onNavigate={handleNavigate} />;
    }

    // For actual web content, a native WebView2 module would render here
    // This is a placeholder that would be replaced by the native module
    return (
      <View style={styles.webPlaceholder}>
        {/* Native WebView2 would be rendered here by the Windows native module */}
        <View style={styles.webPlaceholderContent}>
          <View style={styles.webPlaceholderIcon}>
            <View style={styles.spinningIcon}>
              {/* Would show the actual webpage via WebView2 native module */}
            </View>
          </View>
        </View>
      </View>
    );
  };

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="light-content" backgroundColor={MaterialColors.surface} />

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
        canGoBack={activeTab.canGoBack}
        canGoForward={activeTab.canGoForward}
      />

      {/* Main content area */}
      <View style={styles.mainContent}>
        {/* Sidebar */}
        <Sidebar
          isVisible={sidebarVisible}
          onNavigate={handleNavigate}
          onClose={() => setSidebarVisible(false)}
        />

        {/* Web content / New tab / Settings */}
        <View style={styles.contentArea}>
          {renderContent()}
        </View>
      </View>
    </SafeAreaView>
  );
};

function getDomainFromUrl(url: string): string {
  try {
    const parsed = new URL(url);
    return parsed.hostname;
  } catch {
    return url;
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: MaterialColors.background,
  },
  mainContent: {
    flex: 1,
    flexDirection: 'row',
  },
  contentArea: {
    flex: 1,
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
  webPlaceholderIcon: {
    width: 48,
    height: 48,
    borderRadius: 24,
    backgroundColor: 'rgba(255,255,255,0.04)',
    alignItems: 'center',
    justifyContent: 'center',
  },
  spinningIcon: {
    width: 24,
    height: 24,
    borderRadius: 12,
    borderWidth: 2,
    borderColor: 'rgba(255,255,255,0.1)',
    borderTopColor: MaterialColors.primary,
  },
});

export default App;
