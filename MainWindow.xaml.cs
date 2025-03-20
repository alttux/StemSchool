using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.Win32;
using System;
using System.IO;

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
        ConfProxy.SetProxy("10.0.50.52", 3128, 1);
        MessageBox.Show("Прокси настроен");
    }

    private void Cert_Click(object sender, RoutedEventArgs e)
    {
        ConfProxy.RunMsiInstaller(msiName);
    }
    private void All_Click(object sender, RoutedEventArgs e)
    {
        ConfProxy.RunMsiInstaller(msiName);
        MessageBox.Show("ESPD настроен");
        globalProxyAddr = ProxyTextBox.Text;
        globalProxyPort = Convert.ToInt32(PortTextBox.Text);
        ConfProxy.SetProxy("10.0.50.52", 3128, 1);
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


}

public class ConfProxy
{
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

            Console.WriteLine($"Запущена установка: {msiPath}");
        }
        else
        {
            Console.WriteLine($"Файл не найден: {msiPath}");
        }
    }

    // 🔹 Отключение анимаций (лучшее быстродействие)
    public static void DisableAnimations()
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
        if (key != null)
        {
            key.SetValue("UserPreferencesMask", new byte[] { 144, 18, 3, 128, 12, 0, 0, 0 }, RegistryValueKind.Binary);
            key.Close();
        }

        key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", true);
        if (key != null)
        {
            key.SetValue("VisualFXSetting", 2, RegistryValueKind.DWord); // 2 = "Лучшее быстродействие"
            key.Close();
        }

        ApplyChanges();
    }

    // 🔹 Включение анимаций (стандартные настройки Windows)
    public static void EnableAnimations()
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
        if (key != null)
        {
            key.SetValue("UserPreferencesMask", new byte[] { 144, 38, 3, 128, 12, 0, 0, 0 }, RegistryValueKind.Binary);
            key.Close();
        }

        key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", true);
        if (key != null)
        {
            key.SetValue("VisualFXSetting", 1, RegistryValueKind.DWord); // 1 = "Обычный режим"
            key.Close();
        }

        ApplyChanges();
    }

    // 🔹 Применение настроек без перезагрузки
    static void ApplyChanges()
    {
        Process.Start("RUNDLL32.EXE", "USER32.DLL,UpdatePerUserSystemParameters ,1 ,True");
    }
}