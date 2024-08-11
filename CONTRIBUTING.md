
# Environment setup

## RimWorld dependencies
The `.csproj` file has been configured to automatically detect standard Windows, Linux, and macOS Steam install 
locations, and should link dependencies regardless of the project's location on your hard drive. If that works for you, 
you don't need to read any farther.

These are the paths RimJobWorld will look for the RimWorld dlls:
- `../_RimWorldData/Managed/` (Custom)
- `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed\` (Windows)
- `$(HOME)/.steam/steam/SteamApps/common/RimWorld/RimWorldLinux_Data/Managed/` (Linux)
- `$(HOME)/Library/Application Support/Steam/steamapps/common/RimWorld/RimWorldMac.app/Contents/Resources/Data/Managed/`
  (macOS)

### Custom project location
If you choose to place the RimJobWorld project somewhere other than your RimWorld mods directory, you'll need to create 
a symlink in the RimWorld mods directory that points to the RimJobWorld project. Here are some examples of creating 
symlinks on different platforms:
- Linux and macOS: Open the terminal and run this command (filling in the actual paths): `ln -s "~/where/you/put/rjw" "/your/RimWorld/Mods/rjw"`
- Windows: Run the Command Prompt as administrator, then run this command: `mklink /D "C:\Your\Rimworld\Mods\rjw" "C:\Where\you\put\rjw"`

### Custom RimWorld install
If you're using a custom RimWorld install, you'll want to create a symlink called `_RimWorldData` next to the `rjw` 
folder that points `RimWorldWin64_Data`, `RimWorldLinux_Data`, or `RimWorldMac.app/Contents/Resources/Data`, as 
appropriate. Refer to the previous section for how to create a symlink on your platform. In the end your file structure
should look like this:

- `where/you/put/`
  - `rjw/`
    - `RimJobWorld.Main.csproj`
    - ...
  - `_RimWorldData/` *(symlink)*
    - `Managed/`
      - `Assembly-CSharp.dll`
      - ...

## Deployment Notice
Deployment scripts now make use of PowerShell 7+, instead of Windows PowerShell 5.1, allowing for cross platform execution.
Please see [this document](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2) if you don't have the new PowerShell on your device.


