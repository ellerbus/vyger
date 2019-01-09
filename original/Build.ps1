param (
    [string] $target = "default"
)

#Install-Module -Name psake -Scope CurrentUser

Invoke-psake .\Build.psake.ps1 -taskList $target
