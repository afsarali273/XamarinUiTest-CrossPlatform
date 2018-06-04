using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace VodaiOS
{
    class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.EnableLocalScreenshots().DeviceSerial(Properties.Resources.Emulator1).ApkFile(VodaiOS.Properties.Resources.apkPath).StartApp();
            }
            else
                return ConfigureApp.iOS.EnableLocalScreenshots().AppBundle(Properties.Resources.apkPath).StartApp();

        }
    }
}
