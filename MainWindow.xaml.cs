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
    }

    private void OffAniClick(object sender, RoutedEventArgs e)
    {
        Tweaks.DisableClientAreaAnimation();
        // Остальной код приложения
        MessageBox.Show("Системные анимации отключены.");
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
    const uint SPIF_UPDATEINIFILE = 0x01;
    const uint SPIF_SENDCHANGE = 0x02;

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

    public static void DisableClientAreaAnimation()
    {
        // Передаем uiParam = 0, чтобы отключить анимацию
        if (!SystemParametersInfo(SPI_SETCLIENTAREAANIMATION, 0, IntPtr.Zero, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE))
        {
            Console.WriteLine("Ошибка: " + Marshal.GetLastWin32Error());
        }
    }


}