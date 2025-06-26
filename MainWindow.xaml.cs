using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace StemSchool
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private string globalProxyAddr;
        private int globalProxyPort;
        const string msiName = "cert_install_v2.msi";
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetProxyClick(object sender, RoutedEventArgs e)
        {

            globalProxyAddr = ProxyTextBox.Text;
            globalProxyPort = Convert.ToInt32(PortTextBox.Text);
            Tweaks.SetProxy(globalProxyAddr, globalProxyPort, 1);
            MessageBox.Show(Globals.Message);
        }

        private void CertClick(object sender, RoutedEventArgs e)
        {
            Tweaks.RunMsiInstaller(msiName);
        }

        private void AllClick(object sender, RoutedEventArgs e)
        {
            Tweaks.RunMsiInstaller(msiName);
            globalProxyAddr = ProxyTextBox.Text;
            globalProxyPort = Convert.ToInt32(PortTextBox.Text);
            Tweaks.SetProxy(globalProxyAddr, globalProxyPort, 1);
            Tweaks.TransparencyEffects(true);
            Tweaks.AllAnimations(true);
            Tweaks.DisableWallpaperChanging(true);
        }

        private void GitHubClick(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/alttux")
            {
                UseShellExecute = true
            });
        }

        private void OnAniClick(object sender, RoutedEventArgs e)
        {
            Tweaks.AllAnimations(false);
            MessageBox.Show(Globals.Message);
        }

        private void OffAniClick(object sender, RoutedEventArgs e)
        {
            Tweaks.AllAnimations(true);
            MessageBox.Show(Globals.Message);
        }
        private void OnTransClick(object sender, RoutedEventArgs e)
        {
            Tweaks.TransparencyEffects(false);
            MessageBox.Show(Globals.Message);
        }
        private void OffTransClick(object sender, RoutedEventArgs e)
        {
            Tweaks.TransparencyEffects(true);
            MessageBox.Show(Globals.Message);
        }
        private void OnWalpaperClick(object sender, RoutedEventArgs e)
        {
            Tweaks.DisableWallpaperChanging(false);
            MessageBox.Show(Globals.Message);
        }
        private void OffWalpaperClick(object sender, RoutedEventArgs e)
        {
            Tweaks.DisableWallpaperChanging(true);
            MessageBox.Show(Globals.Message);
        }

        private void KMSServClick(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe", "/wait /c \"slmgr.vbs /skms kms.digiboy.ir\""); // 1. Activate KMS Server
        }
        private void ProKeyClick(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe", "/wait /c \"slmgr.vbs /ipk W269N-WFGWX-YVC9B-4J6C9-T83GX\""); // 2. Set Pro Activation key
        }

        private void HomeKeyClick(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe", "/wait /c \"slmgr.vbs /ipk TX9XD-98N7V-6WMQ6-BX7FG-H8Q99\"");
        }
        private void EducationKeyClick(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe", "/wait /c \"slmgr.vbs /ipk NW6C2-QMPVW-D7KKK-3GKT6-VCFB2\"");
        }
        private void EnterpriceKeyClick(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe", "/wait /c \"slmgr.vbs /ipk NPPR9-FWDCX-D2C8J-H872K-2YT43\"");
        }

        private void ActivateClick(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe", "/wait /c \"slmgr.vbs /ato\""); // 3. activation
        }

        private void MASClick(object sender, RoutedEventArgs e)
        {
            Process.Start("powershell.exe", "irm https://get.activated.win | iex");
        }

        private void ExplorerRestartClick(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe", "/C taskkill /f /im explorer.exe & start explorer.exe");
        }

        public static class Globals
        {
            public static string Message = "";
        }

        public class Tweaks
        {
            // Настройка прокси сервера
            public static void SetProxy(string proxyAddress, int port, int proxyEnable)
            {
                const string registryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKey, true))
                    {
                        if (key != null)
                        {
                            key.SetValue("ProxyEnable", proxyEnable);
                            key.SetValue("ProxyServer", $"{proxyAddress}:{port}");
                        }
                    }
                    Globals.Message = "Прокси настроен";
                }
                catch (Exception ex)
                {
                    Globals.Message = $"Ошибка настройки прокси: {ex.Message}";
                }
            }

            // Запуск сертификата
            public static void RunMsiInstaller(string msiName)
            {
                string msiPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, msiName);

                if (File.Exists(msiPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "msiexec.exe",
                        Arguments = $"/i \"{msiPath}\" /qb", // /qn - тихая установка без окон
                        UseShellExecute = false
                    });
                    Globals.Message = $"Запущена установка: {msiPath}";
                }
                else
                {
                    Globals.Message = $"Файл не найден: {msiPath}";
                }
            }

            // Хз как работает. Онимации
            const uint SPI_SETCLIENTAREAANIMATION = 0x1043;
            const uint SPI_SETANIMATION = 0x0049;
            const uint SPIF_UPDATEINIFILE = 0x01;
            const uint SPIF_SENDCHANGE = 0x02;

            [StructLayout(LayoutKind.Sequential)]
            private struct ANIMATIONINFO
            {
                public uint cbSize;
                public int iMinAnimate;
            }

            [DllImport("user32.dll", SetLastError = true)]
            static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

            public static void UpdateSettings()
            {
                Process.Start("RUNDLL32.EXE", "USER32.DLL,UpdatePerUserSystemParameters ,1 ,True");
            }

            public static void AllAnimations(bool disable)
            {
                try
                {
                    // 1. Управление анимацией клиентской области окон
                    SystemParametersInfo(SPI_SETCLIENTAREAANIMATION, disable ? 0u : 1u, IntPtr.Zero, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);

                    // 2. Управление анимациями окон (разворачивание/сворачивание)
                    ANIMATIONINFO animInfo = new ANIMATIONINFO
                    {
                        cbSize = (uint)Marshal.SizeOf(typeof(ANIMATIONINFO)),
                        iMinAnimate = disable ? 0 : 1
                    };
                    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ANIMATIONINFO)));
                    Marshal.StructureToPtr(animInfo, ptr, false);
                    SystemParametersInfo(SPI_SETANIMATION, (uint)Marshal.SizeOf(typeof(ANIMATIONINFO)), ptr, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
                    Marshal.FreeHGlobal(ptr);

                    // 3. Управление анимациями через реестр
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop\WindowMetrics", true);
                    if (key != null)
                    {
                        key.SetValue("MinAnimate", disable ? "0" : "1", RegistryValueKind.String);
                        key.Close();
                    }

                    UpdateSettings();

                    Globals.Message = disable ? "Все анимации отключены!" : "Все анимации включены!";
                }
                catch (Exception ex)
                {
                    Globals.Message = $"Ошибка при изменении настроек анимации: {ex.Message}";
                }
            }

            public static void TransparencyEffects(bool disable)
            {
                try
                {
                    // Открываем ключ реестра, который отвечает за прозрачность
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);
                    if (key != null)
                    {
                        key.SetValue("EnableTransparency", disable ? 0 : 1, RegistryValueKind.DWord); // 0 = Отключить прозрачность
                        key.Close();
                    }

                    UpdateSettings();

                    Globals.Message = disable ? "Все прозрачности отключены!" : "Все прозрачности включены!";

                }
                catch (Exception ex)
                {
                    Globals.Message = $"Ошибка {ex}";
                }
            }

            public static void DisableWallpaperChanging(bool disable)
            {
                try
                {
                    // Путь к ключу реестра, который управляет сменой обоев
                    string registryPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop";
                    string valueName = "NoChangingWallpaper";

                    // Устанавливаем значение: 1 - запретить смену, 0 - разрешить
                    int value = disable ? 1 : 0;

                    Registry.SetValue(registryPath, valueName, value, RegistryValueKind.DWord);

                    // Также можно установить политику в другом месте реестра (для некоторых версий Windows)
                    registryPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\System";
                    Registry.SetValue(registryPath, valueName, value, RegistryValueKind.DWord);

                    if (disable)
                    {
                        Globals.Message = "Смена обоев запрещена.";
                    }
                    else
                    {
                        Globals.Message = "Смена обоев разрешена.";
                    }
                }
                catch (Exception ex)
                {
                    Globals.Message = $"Ошибка при изменении настроек: {ex.Message}";
                }
            }
        }
    }
}