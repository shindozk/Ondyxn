namespace Ondyxn.Core.Enums;

public enum TabState
{
    Loading,
    Ready,
    Error,
    Crashed
}

public enum TabGroup
{
    None,
    Work,
    Personal,
    Shopping,
    Research
}

public enum BrowserMode
{
    Normal,
    Private
}

public enum DownloadState
{
    Pending,
    InProgress,
    Paused,
    Completed,
    Failed,
    Cancelled
}

public enum ThemeMode
{
    Dark,
    Light,
    System
}

public enum NavigationDirection
{
    Back,
    Forward,
    Reload,
    Stop
}
