# FocusTest
Keeps track of Windows apps that have focus.

| Project   | Build status            | Latest release |
| --------- | ----------------------- | -------------- |
| FocusTest | ![.NET Core Desktop][1] | [Binaries][2]  |

## Requirements
- .NET Core Desktop Runtime 3.1 or later ([64 bit][5] | [32 bit][6])
- Windows (only Windows 10 tested). FocusTest uses Windows APIs to detect change in focus and retrieve the PID (Process ID) associated to the window that received focus.

## How to use
1. Launch FocusTest
2. Work as usual
3. FocusTest will list all apps that have taken focus since it launched (newest apps with focus are on top)  
![Screenshot of FocusTest][3]
4. Compare PID with Task Manager (Ctrl + Shift + Esc)  
![Screenshot of Task Manager][4]

## How to build
### Requirements
- Visual Studio Community (not to be confused with Visual Studio Code)
- .NET Core SDK 3.1 or later

Simply open the `FocusTest.sln` file with Visual Studio, then click on "Build" > "Build Solution". The binaries will be located in `\bin\Debug` or `\bin\Release`.


  [1]: https://github.com/Nolonar/FocusTest/workflows/.NET%20Core%20Desktop/badge.svg
  [2]: https://github.com/Nolonar/FocusTest/releases/latest/download/Binaries.zip
  [3]: 2020-08-02_005555.png
  [4]: 2020-08-02_005626.png
  [5]: https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.6-windows-x64-installer
  [6]: https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.6-windows-x86-installer
