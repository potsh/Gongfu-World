#!/bin/bash

$computer=Get-WMIObject  Win32_ComputerSystem
$Cname=$computer.name

Msg=$1
git add .
if [ "$Msg" == "" ]
then
    git commit -m "co and push @ "+"$Cname" 
else 
    git commit -m "$Msg"
fi
git push
