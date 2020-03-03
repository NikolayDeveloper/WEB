$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
echo $dir
cd $dir
yarn build:prod:ru