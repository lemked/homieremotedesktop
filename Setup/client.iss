#define MyAppName "Homie Remotedesktop"
#define MyAppVersion "0.1"
#define MyAppPublisher "Daniel Lemke"
#define MyAppURL "https://github.com/lemked/homieremotedesktop"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
AppId={{EB48ACFA-BBCD-4CAC-B5D0-367EC0B6F7C7}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=..\LICENSE
OutputDir=..\Setup
OutputBaseFilename=HomieRemoteDesktop-Setup
SetupIconFile=..\Setup\setup.ico
Compression=lzma
SolidCompression=yes
WizardImageFile=compiler:WizModernImage-IS.bmp
WizardSmallImageFile=compiler:WizModernSmallImage-IS.bmp
AppReadmeFile=..\README.md

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Files]
Source: "..\Homie.Client\bin\Release\Homie.Client.exe"; DestDir: "{app}"; Flags: ignoreversion; Components: Client
Source: "..\Homie.Client\bin\Release\Homie.Resources.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Homie.Client\bin\Release\Homie.Model.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Homie.Client\bin\Release\Homie.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Homie.Client\bin\Release\MVVMLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Homie.Client\bin\Release\EntityFramework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Homie.Client\bin\Release\de-DE\Homie.Resources.resources.dll"; DestDir: "{app}\de-DE"; Flags: ignoreversion; Languages: german

[Icons]
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[ThirdParty]
UseRelativePaths=True