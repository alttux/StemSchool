using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace StemSchool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string globalProxyAddr;
    private int globalProxyPort;
    const string msiName = "cert_install_v2.msi";

    public MainWindow()
    {
        InitializeComponent();
    }

    // Заглушки для кнопок
    private void SetProxy(object sender, RoutedEventArgs e)
    {
        globalProxyAddr = ProxyTextBox.Text;
        globalProxyPort = Convert.ToInt32(PortTextBox.Text);
        Tweaks.SetProxy(globalProxyAddr, globalProxyPort, 1);
        MessageBox.Show("Прокси настроен");
    }

    private void Cert_Click(object sender, RoutedEventArgs e)
    {
        Tweaks.RunMsiInstaller(msiName);
    }
    private void All_Click(object sender, RoutedEventArgs e)
    {
        Tweaks.RunMsiInstaller(msiName);
        MessageBox.Show("ESPD настроен");
        globalProxyAddr = ProxyTextBox.Text;
        globalProxyPort = Convert.ToInt32(PortTextBox.Text);
        Tweaks.SetProxy(globalProxyAddr, globalProxyPort, 1);
    }
    private void GitHub_Click(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/CodeCraftsman89")
        {
            UseShellExecute = true
        });
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void OnAniClick(object sender, RoutedEventArgs e)
    {
        Tweaks.EnableAllAnimations();
        MessageBox.Show("Системные анимции включены");
    }

    private void OffAniClick(object sender, RoutedEventArgs e)
    {
        Tweaks.DisableAllAnimations();
        // Остальной код приложения
        MessageBox.Show("Системные анимации отключены.");
    }
    private void OnTransClick(object sender, RoutedEventArgs e)
    {
        Tweaks.EnableTransparencyEffects();
        // Остальной код приложения
        MessageBox.Show("Эффекты прозрачности включены.");
    }
    private void OffTransClick (object sender, RoutedEventArgs e)
    {
        Tweaks.DisableTransparencyEffects();
        // Остальной код приложения
        MessageBox.Show("Эффекты прозрачности отключены.");
    }
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
            MessageBox.Show("Прокси настроен");
        }
        catch (Exception ex)
        {
             MessageBox.Show("Ошибка настройки прокси: " + ex.Message);
        }
    }


    // Запуск сертефиката
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

            MessageBox.Show($"Запущена установка: {msiPath}");
        }
        else
        {
            MessageBox.Show($"Файл не найден: {msiPath}");
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

    public static void EnableAllAnimations()
    {
        try
        {
            // 1. Включаем анимацию клиентской области окон
            if (!SystemParametersInfo(SPI_SETCLIENTAREAANIMATION, 1, IntPtr.Zero, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE))
            {
                Console.WriteLine("⚠ Ошибка включения анимации клиентской области: " + Marshal.GetLastWin32Error());
            }

            // 2. Включаем анимации окон (разворачивание/сворачивание)
            ANIMATIONINFO animInfo = new ANIMATIONINFO { cbSize = (uint)Marshal.SizeOf(typeof(ANIMATIONINFO)), iMinAnimate = 1 };
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ANIMATIONINFO)));
            Marshal.StructureToPtr(animInfo, ptr, false);
            if (!SystemParametersInfo(SPI_SETANIMATION, (uint)Marshal.SizeOf(typeof(ANIMATIONINFO)), ptr, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE))
            {
                Console.WriteLine("⚠ Ошибка включения анимации окон: " + Marshal.GetLastWin32Error());
            }
            Marshal.FreeHGlobal(ptr);

            // 3. Включаем анимации через реестр
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop\WindowMetrics", true);
            if (key != null)
            {
                key.SetValue("MinAnimate", "1", RegistryValueKind.String);
                key.Close();
            }

            // 4. Применяем изменения без перезагрузки
            ApplyChanges();

            Console.WriteLine("✅ Все анимации включены!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Ошибка: " + ex.Message);
        }
    }

    public static void DisableAllAnimations()
    {
        try
        {
            // 1. Отключаем анимацию клиентской области окон
            SystemParametersInfo(SPI_SETCLIENTAREAANIMATION, 0, IntPtr.Zero, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);

            // 2. Отключаем анимации окон (разворачивание/сворачивание)
            ANIMATIONINFO animInfo = new ANIMATIONINFO { cbSize = (uint)Marshal.SizeOf(typeof(ANIMATIONINFO)), iMinAnimate = 0 };
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ANIMATIONINFO)));
            Marshal.StructureToPtr(animInfo, ptr, false);
            SystemParametersInfo(SPI_SETANIMATION, (uint)Marshal.SizeOf(typeof(ANIMATIONINFO)), ptr, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            Marshal.FreeHGlobal(ptr);

            // 3. Отключаем анимации через реестр
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop\WindowMetrics", true);
            if (key != null)
            {
                key.SetValue("MinAnimate", "0", RegistryValueKind.String);
                key.Close();
            }

            // 4. Применяем изменения без перезагрузки
            ApplyChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }

    // 🔹 Применение настроек без перезагрузки
    private static void ApplyChanges()
    {
        Process.Start("RUNDLL32.EXE", "USER32.DLL,UpdatePerUserSystemParameters ,1 ,True");
    }

    public static void DisableTransparencyEffects()
    {
        try
        {
            // Открываем ключ реестра, который отвечает за прозрачность
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);
            if (key != null)
            {
                key.SetValue("EnableTransparency", 0, RegistryValueKind.DWord); // 0 = Отключить прозрачность
                key.Close();
            }

            // Применяем изменения без перезагрузки
            ApplyChangesTrans();

        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Ошибка: " + ex.Message);
        }
    }

    // 🔹 Применение настроек без перезагрузки
    private static void ApplyChangesTrans()
    {
        Process.Start("RUNDLL32.EXE", "USER32.DLL,UpdatePerUserSystemParameters ,1 ,True");
    }


    // ✅ Метод для включения прозрачности обратно
    public static void EnableTransparencyEffects()
    {
        try
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", true);
            if (key != null)
            {
                key.SetValue("EnableTransparency", 1, RegistryValueKind.DWord); // 1 = Включить прозрачность
                key.Close();
            }

            ApplyChangesTrans();

        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Ошибка: " + ex.Message);
        }
    }

}