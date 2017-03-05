using SteamVR_Notifier.Ancs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamVR_Notifier
{
    class ApplicationContext : System.Windows.Forms.ApplicationContext
    {
        NotifyIcon notifyIcon;
        Thread overlayThread;
        Ancs.Advertiser Advertiser;
        Ancs.AncsManager AncsManager;

        public ApplicationContext()
        {
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));
            overlayThread = new Thread(new ThreadStart(OverlayThread.Run));
            overlayThread.Start();

            notifyIcon = new NotifyIcon();
            notifyIcon.ContextMenu = new ContextMenu(
                new MenuItem[] { exitMenuItem }
                );

            notifyIcon.Visible = true;

            this.Advertiser = new Advertiser();
            this.AncsManager = new AncsManager();

            this.AncsManager.OnNotification += AncsManager_OnNotification;
            this.AncsManager.OnStatusChange += AncsManager_OnStatusChange;
            this.AncsManager.Connect();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void AncsManager_OnStatusChange(string obj)
        {
            Console.WriteLine("Status Change.");
        }

        private void AncsManager_OnNotification(PlainNotification obj)
        {
            OverlayThread.NewNotifications.Add(obj);
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            SteamVR_WebKit.SteamVR_WebKit.Stop();
        }

        private void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }


    }
}
