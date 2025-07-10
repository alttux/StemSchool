using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace StemSchool
{

    /// <summary>
    /// Главное окно приложения StemSchool Tweaker
    /// </summary>
    /// <remarks>
    /// <para>Это приложение предоставляет набор инструментов для настройки системы:</para>
    /// <list type="bullet">
    ///   <item>Настройка прокси-сервера</item>
    ///   <item>Установка сертификатов</item>
    ///   <item>Оптимизация производительности (анимации, эффекты прозрачности)</item>
    ///   <item>Управление групповыми политиками (ограничение смены обоев)< /item>
    ///   <item>Активация Windows через KMS</item>
    /// </list>
    /// <para>Приложение использует системный реестр и Windows API для изменения настроек.</para>
    /// </remarks>
    public partial class MainWindow : Window
    {
        private string globalProxyAddr;
        private int globalProxyPort;
        const string msiName = "cert_install_v2.msi";
        private string wallPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wall.png");

        public MainWindow()
        {
            InitializeComponent();
            WallTextBox.Text = wallPath;
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
            // Завершаем процесс explorer.exe
            foreach (var process in Process.GetProcessesByName("explorer"))
            {
                process.Kill();
            }

            // Запускаем explorer.exe снова
            Process.Start("explorer.exe");
        }

        private void OffTelemetryClick(object sender, RoutedEventArgs e)
        { 
            Tweaks.Telemetry(true);
            if (MessageBox.Show($"{Globals.Message}",
                "Reboot now?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start("shutdown.exe", "/r /t 0");
            }
        }

        private void OnTelemetryClick(object sender, RoutedEventArgs e)
        {
            Tweaks.Telemetry(false);
            MessageBox.Show(Globals.Message);
        }

        private void OpenWallPathClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OpenWallDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Выберите файл обоев",
                DefaultExt = ".png",
                Filter = "Изображения (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Все файлы (*.*)|*.*",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
            Nullable<bool> result = OpenWallDialog.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = OpenWallDialog.FileName;
                WallTextBox.Text = filename;
            }
        }

        private void ResetWallPathClick(object sender, RoutedEventArgs e)
        {
            WallTextBox.Text = wallPath;
        }

        private void SetWallPathClick(object sender, RoutedEventArgs e)
        {
            // применяем обои без перезагрузки
            Tweaks.WallpaperSetter.SetWallpaper(WallTextBox.Text);
        }

        public static class Globals
        {
            public static string Message = "Неизвестное сообщение";
        }

        /// <summary>
        /// Класс, содержащий методы для настройки системы
        /// </summary>
        /// <remarks>
        /// Содержит методы для работы с:
        /// <list type="bullet">
        ///   <item>Настройками прокси</item>
        ///   <item>Анимациями и визуальными эффектами</item>
        ///   <item>Параметрами прозрачности</item>
        ///   <item>Ограничениями на смену обоев</item>
        /// </list>
        /// Использует Windows API и системный реестр для применения изменений.
        /// </remarks>
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
                }
                catch (Exception ex)
                {
                    Globals.Message = $"Ошибка настройки прокси: {ex.Message}";
                }
                finally
                {
                    Globals.Message = "Прокси настроен";
                }
            }

            // Запуск сертификата
            public static void RunMsiInstaller(string msiName)
            {
                string msiPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, msiName);

                if (File.Exists(msiPath))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "msiexec.exe",
                            Arguments = $"/i \"{msiPath}\" /qb", // /qn - тихая установка без окон
                            UseShellExecute = false
                        });
                    }
                    catch (Exception ex)
                    {
                        Globals.Message = $"Ошибка при запуске установки: {ex.Message}";
                    }
                    finally 
                    {
                        Globals.Message = $"Запущена установка: {msiPath}";
                    }
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


                }
                catch (Exception ex)
                {
                    Globals.Message = $"Ошибка при изменении настроек анимации: {ex.Message}";
                }
                finally
                {
                    UpdateSettings();
                    Globals.Message = disable ? "Все анимации отключены!" : "Все анимации включены!";
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

                }
                catch (Exception ex)
                {
                    Globals.Message = $"Ошибка {ex}";
                }
                finally
                {
                    UpdateSettings();
                    Globals.Message = disable ? "Все прозрачности отключены!" : "Все прозрачности включены!";
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
                }
                catch (Exception ex)
                {
                    Globals.Message = $"Ошибка при изменении настроек: {ex.Message}";
                }
                finally
                {
                    Globals.Message = disable ? "Смена обоев запрещена." : "Смена обоев разрешена.";
                }
            }

            public static void Telemetry(bool disable)
            {
                if (disable)
                {
                    try
                    {
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("AllowTelemetry", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("AllowTelemetry", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("MaxTelemetryAllowed", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform").SetValue("NoGenTicket", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("DoNotShowFeedbackNotifications", 1);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("AITEnable", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("AllowTelemetry", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableEngine", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableInventory", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisablePCA", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableUAR", 1);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\appDiagnostics").SetValue("Value", "Deny", RegistryValueKind.String);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("UploadUserActivities", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("PublishUserActivities", 0);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}").SetValue("ScenarioExecutionEnabled", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\DeviceHealthAttestationService").SetValue("EnableDeviceHealthAttestationService", 0);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitTextCollection", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitInkCollection", 0);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Speech").SetValue("AllowSpeechModelUpdate", 0);
                    }
                    catch (Exception ex)
                    {
                        Globals.Message = $"Ошибка при отключении телеметрии: {ex.Message}";
                    }
                    finally
                    {
                        Globals.Message = "Телеметрия отключена!";
                    }
                }
                else
                {
                    try
                    {
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("AllowTelemetry", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("AllowTelemetry", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("MaxTelemetryAllowed", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform").SetValue("NoGenTicket", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection").SetValue("DoNotShowFeedbackNotifications", 0);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("AITEnable", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("AllowTelemetry", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableEngine", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableInventory", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisablePCA", 0);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppCompat").SetValue("DisableUAR", 0);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("UploadUserActivities", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System").SetValue("PublishUserActivities", 2);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}").SetValue("ScenarioExecutionEnabled", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\DeviceHealthAttestationService").SetValue("EnableDeviceHealthAttestationService", 1);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitTextCollection", 1);
                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\InputPersonalization").SetValue("RestrictImplicitInkCollection", 1);

                        Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Speech").SetValue("AllowSpeechModelUpdate", 1);
                    }
                    catch (Exception ex)
                                        {
                        Globals.Message = $"Ошибка при включении телеметрии: {ex.Message}";
                    }
                    finally
                    {
                        Globals.Message = "Телеметрия включена!";
                    }

                }
            }

            public class WallpaperSetter
            {
                public enum WallpaperStyle
                {
                    Tiled,
                    Centered,
                    Stretched,
                    Fit,
                    Fill
                }

                public static void SetWallpaper(string imagePath, WallpaperStyle style = WallpaperStyle.Fill)
                {
                    try
                    {
                        // Устанавливаем стиль обоев
                        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

                        switch (style)
                        {
                            case WallpaperStyle.Tiled:
                                key.SetValue(@"WallpaperStyle", "0");
                                key.SetValue(@"TileWallpaper", "1");
                                break;
                            case WallpaperStyle.Centered:
                                key.SetValue(@"WallpaperStyle", "0");
                                key.SetValue(@"TileWallpaper", "0");
                                break;
                            case WallpaperStyle.Stretched:
                                key.SetValue(@"WallpaperStyle", "2");
                                key.SetValue(@"TileWallpaper", "0");
                                break;
                            case WallpaperStyle.Fit:
                                key.SetValue(@"WallpaperStyle", "6");
                                key.SetValue(@"TileWallpaper", "0");
                                break;
                            case WallpaperStyle.Fill:
                            default:
                                key.SetValue(@"WallpaperStyle", "10");
                                key.SetValue(@"TileWallpaper", "0");
                                break;
                        }

                        // Устанавливаем обои
                        SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath,
                            SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при установке обоев: {ex.Message}");
                    }
                }

                // Импорт функции SystemParametersInfo
                [DllImport("user32.dll", CharSet = CharSet.Auto)]
                private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

                private const int SPI_SETDESKWALLPAPER = 0x0014;
                private const int SPIF_UPDATEINIFILE = 0x01;
                private const int SPIF_SENDWININICHANGE = 0x02;
            }
        }
    }
}