#!/usr/bin/env bash
set -e

scriptDir=$(pwd)
scriptName="Deploy.ps1"
scriptPath=$scriptDir/$scriptName
echo "Reminder: p7zip and PowerShell Core are required."
pwsh -NoProfile -ExecutionPolicy Bypass -Command "& '$scriptPath'"
