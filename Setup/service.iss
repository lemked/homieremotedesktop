#define MyAppName "Homie Remotedesktop Service"
#define MyAppVersion "0.1"
#define MyAppPublisher "Daniel Lemke"
#define MyAppURL "https://github.com/lemked/homieremotedesktop"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
AppId={{228EBE83-8850-451A-8C8C-80A54AAE3FC9}
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
OutputBaseFilename=HomieService-Setup
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
Source: "..\Homie.Service\bin\Release\Homie.Service.exe"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\Homie.Service\bin\Release\Homie.Model.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Homie.Service\bin\Release\Homie.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Homie.Service\bin\Release\EntityFramework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Homie.Service\bin\Release\EntityFramework.SqlServer.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[ThirdParty]
UseRelativePaths=True
