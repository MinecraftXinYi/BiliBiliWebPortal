using System;

namespace Microsoft.Web.WebView2;

using Core;

public static class WebView2Tweak
{
    public static void CoreWebView2NewWindowReflectToCurrent(object sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        if (sender is CoreWebView2)
        {
            (sender as CoreWebView2).Navigate(e.Uri);
            e.Handled = true;
        }
        else throw new NotSupportedException("The sender is not a CoreWebView2 object.");
    }
}
