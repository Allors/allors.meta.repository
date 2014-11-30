@SET PATH=%PATH%;C:\Windows\Microsoft.NET\Framework64\v4.0.30319;C:\Windows\Microsoft.NET\Framework\v4.0.30319

@msbuild Allors.R1.Development.sln /verbosity:minimal /p:Configuration=Release /TARGET:REBUILD

@SET BUILDRESULT=%ERRORLEVEL%

@ECHO Build result = %BUILDRESULT%

@IF %BUILDRESULT% EQU 0 robocopy Allors.R1.Development\bin\Release export
@IF %BUILDRESULT% EQU 0 robocopy Allors.R1.Development.Winforms.Program\bin\Release export\winforms
@IF %BUILDRESULT% EQU 0 robocopy Allors.R1.Development.GtkSharp.Program\bin\Release export\gtk

@pause