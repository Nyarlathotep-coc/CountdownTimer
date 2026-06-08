@echo off
echo ==================================
echo  编译多功能倒计时器
echo ==================================

set SRC=%~dp0
set OUT=%~dp0bin\Release
set RES=%~dp0Resources

if not exist "%OUT%" mkdir "%OUT%"

echo 正在编译...

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /noconfig /target:winexe /out:"%OUT%\CountdownTimer.exe" ^
/r:"System.dll" ^
/r:"System.Windows.Forms.dll" ^
/r:"System.Drawing.dll" ^
/r:"System.Xml.dll" ^
/r:"System.Core.dll" ^
/r:"Microsoft.CSharp.dll" ^
/recurse:"%SRC%*.cs" ^
/win32icon:"%RES%\app.ico" 2>nul

if exist "%OUT%\CountdownTimer.exe" (
    echo.
    echo ? 编译成功！
    echo 输出文件：%OUT%\CountdownTimer.exe
    echo.
    start "" "%OUT%\CountdownTimer.exe"
) else (
    echo.
    echo ? 编译失败，重新尝试无图标编译...
    C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe /noconfig /target:winexe /out:"%OUT%\CountdownTimer.exe" ^
    /r:"System.dll" ^
    /r:"System.Windows.Forms.dll" ^
    /r:"System.Drawing.dll" ^
    /r:"System.Xml.dll" ^
    /r:"System.Core.dll" ^
    /r:"Microsoft.CSharp.dll" ^
    /recurse:"%SRC%*.cs"

    if exist "%OUT%\CountdownTimer.exe" (
        echo.
        echo ? 编译成功（无图标）
        echo 输出文件：%OUT%\CountdownTimer.exe
        echo.
        start "" "%OUT%\CountdownTimer.exe"
    ) else (
        echo ? 编译失败，请检查代码错误。
    )
)

pause
