param($buildNumber)

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
echo $dir
cd $dir
dotnet publish -c Debug -o C:/NiisBuild/Niis_$buildNumber