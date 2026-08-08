using Avalonia.Controls;
using Ondyxn.Core.Models;

namespace Ondyxn.UI.Views;

public partial class SecurityInfoPopup : UserControl
{
    public SecurityInfoPopup()
    {
        InitializeComponent();
    }

    public void UpdateInfo(SecurityInfo info)
    {
        if (SiteUrl is not null)
            SiteUrl.Text = new Uri(info.Url).Host;

        if (SecurityStatus is not null)
        {
            SecurityStatus.Text = info.SecuritySummary;
            SecurityStatus.Foreground = info.IsSecure
                ? new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#4ADE80"))
                : new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#F87171"));
        }

        if (ProtocolText is not null)
            ProtocolText.Text = info.ProtocolVersion ?? (info.IsSecure ? "HTTPS" : "HTTP");

        if (CipherText is not null)
            CipherText.Text = info.CipherSuite ?? "Standard";

        if (CertText is not null)
        {
            CertText.Text = info.IsCertificateValid ? "Valid" : "Invalid";
            CertText.Foreground = info.IsCertificateValid
                ? new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#4ADE80"))
                : new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#F87171"));
        }

        if (TrackersText is not null)
            TrackersText.Text = info.TrackersBlocked.ToString();

        if (AdsText is not null)
            AdsText.Text = info.AdsBlocked.ToString();
    }
}
