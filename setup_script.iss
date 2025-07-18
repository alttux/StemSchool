; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Stemschool Tweaker"
#define MyAppVersion "1.4"
#define MyAppPublisher "AltTux, Inc."
#define MyAppURL "https://github.com/alttux/StemSchool/releases"
#define MyAppExeName "StemSchool.exe"
#define MyAppDir "C:\Users\roma-win\Desktop\StemSchool\bin\Release\net8.0-windows\win-x64\"
#define SourceDir "C:\Users\roma-win\Desktop\StemSchool\"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{AFF9D255-AAC0-4606-84F8-9E2F097FC322}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
; "ArchitecturesAllowed=x64compatible" specifies that Setup cannot run
; on anything but x64 and Windows 11 on Arm.
ArchitecturesAllowed=x64compatible
; "ArchitecturesInstallIn64BitMode=x64compatible" requests that the
; install be done in "64-bit mode" on x64 or Windows 11 on Arm,
; meaning it should use the native 64-bit Program Files directory and
; the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=yes
LicenseFile=C:\Users\roma-win\Desktop\StemSchool\LICENSE.txt
; Uncomment the following line to run in non administrative install mode (install for current user only).
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputDir=C:\Users\roma-win\Desktop\StemSchool\bin\Release
OutputBaseFilename=Stemschool Tweaker Setup
SetupIconFile=C:\Users\roma-win\Desktop\StemSchool\image_2025-03-18_07-00-51.ico
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#MyAppDir}{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MyAppDir}*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#SourceDir}windowsdesktop-runtime-8.0.18-win-x86.exe"; DestDir: "{app}"; Flags: deleteafterinstall
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
Filename: "{app}\windowsdesktop-runtime-8.0.18-win-x86.exe"; Parameters: "/silent"; StatusMsg: "Устанавливаем необходимые компоненты..."; BeforeInstall: PrepareDependency; Check: IsDependencyInstallationNeeded

[Code]
var
  DependencyPage: TOutputProgressWizardPage;

procedure PrepareDependency;
begin
  DependencyPage := CreateOutputProgressPage(
    'Установка зависимостей', 
    'Пожалуйста, подождите, пока устанавливаются необходимые компоненты...');
  DependencyPage.Show;
  try
    // Здесь можно добавить дополнительную подготовку
  finally
    DependencyPage.Hide;
  end;
end;

function IsDependencyInstallationNeeded: Boolean;
begin
  // Здесь реализуйте проверку, установлена ли зависимость
  // Например, проверка в реестре или наличие файла
  Result := not RegKeyExists(HKLM, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{Dependency-GUID}');
  
  // Альтернативная проверка по наличию файла:
  // Result := not FileExists(ExpandConstant('{pf}\Dependency\program.exe'));
end;

function InitializeSetup: Boolean;
begin
  // Дополнительная проверка при запуске установщика
  if IsDependencyInstallationNeeded then
  begin
    if MsgBox('Для работы программы требуется .NET 8.0 Desktop Runtime. Установить сейчас?', 
      mbConfirmation, MB_YESNO) = IDNO then
    begin
      Result := False;
      Exit;
    end;
  end;
  Result := True;
end;

