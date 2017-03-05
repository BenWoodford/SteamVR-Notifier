using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamVR_WebKit;
using CefSharp;

namespace SteamVR_Notifier
{
    public static class OverlayThread
    {
        static WebKitOverlay notificationsOverlay;
        public static List<Ancs.PlainNotification> NewNotifications;

        public static void Run()
        {
            NewNotifications = new List<Ancs.PlainNotification>();
            SteamVR_WebKit.SteamVR_WebKit.Init();
            SteamVR_WebKit.SteamVR_WebKit.FPS = 30;

            notificationsOverlay = new WebKitOverlay(new Uri("file://" + Environment.CurrentDirectory + "/Resources/index.html"), 1200, 1000, "notifications.ancs", "Notifications", OverlayType.Dashboard);
            notificationsOverlay.DashboardOverlay.Width = 2f;
            notificationsOverlay.BrowserPreInit += NotificationsOverlay_BrowserPreInit;
            notificationsOverlay.PreUpdateCallback += NotificationsOverlay_PreUpdateCallback;
            notificationsOverlay.BrowserReady += NotificationsOverlay_BrowserReady;
            notificationsOverlay.StartBrowser();

            SteamVR_WebKit.SteamVR_WebKit.RunOverlays();
        }

        private static void NotificationsOverlay_BrowserReady(object sender, EventArgs e)
        {
            notificationsOverlay.Browser.ShowDevTools();
        }

        private static void NotificationsOverlay_PreUpdateCallback(object sender, EventArgs e)
        {
            Ancs.PlainNotification[] queue = NewNotifications.ToArray();
            NewNotifications.Clear();
            foreach (Ancs.PlainNotification notification in queue)
            {
                notificationsOverlay.Browser.ExecuteScriptAsync(@"newNotification('" + notification.Title.Replace("'", "\\'") + "', '" + notification.Message.Replace("'", "\\'") + "');");
            }
        }

        private static void NotificationsOverlay_BrowserPreInit(object sender, EventArgs e)
        {
            notificationsOverlay.Browser.RegisterJsObject("notifications", new SteamVR_WebKit.JsInterop.Notifications(notificationsOverlay.DashboardOverlay));
        }
    }
}
