# Set PowerShell ExecutionPolicy
$ep = Get-ExecutionPolicy -Scope CurrentUser
Set-ExecutionPolicy Unrestricted -Scope CurrentUser

# Import MsBuild helper module
Import-Module .\build\Invoke-MsBuild.psm1

# Set version using GitVersion - https://gitversion.readthedocs.io/en/latest/
gitversion /updateassemblyinfo

$solutionPath = Join-Path -Path $(pwd) -ChildPath "Sumo.Nop.sln"

# Restore NuGet Packages
nuget restore

# Build the solution
Invoke-MsBuild -Path $solutionPath -Params "/target:Clean;Build /property:Configuration=Release;BuildInParallel=true /verbosity:Detailed /maxcpucount" -ShowBuildOutputInCurrentWindow

# Reset PowerShell ExecutionPolicy
Set-ExecutionPolicy $ep -Scope CurrentUser

# Undo changes to AssemblyInfo as we don't care to commit version changes
git checkout -- .\Sumo.Nop.MediaTools\Properties\AssemblyInfo.cs