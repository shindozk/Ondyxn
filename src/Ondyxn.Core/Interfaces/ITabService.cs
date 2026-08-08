using Ondyxn.Core.Enums;
using Ondyxn.Core.Models;

namespace Ondyxn.Core.Interfaces;

/// <summary>
/// Manages browser tabs lifecycle and state.
/// </summary>
public interface ITabService
{
    IReadOnlyList<TabModel> Tabs { get; }
    TabModel? ActiveTab { get; }
    event EventHandler<TabModel>? TabCreated;
    event EventHandler<TabModel>? TabClosed;
    event EventHandler<TabModel>? TabChanged;
    event EventHandler<TabModel>? ActiveTabChanged;

    TabModel CreateTab(string? url = null, TabGroup group = TabGroup.None);
    void CloseTab(Guid tabId);
    void SetActiveTab(Guid tabId);
    void MoveTab(Guid tabId, int newIndex);
    void PinTab(Guid tabId);
    void MuteTab(Guid tabId);
    void SetTabGroup(Guid tabId, TabGroup group);
}
