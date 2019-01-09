

Import-Module .\ftp.psm1

$psake.use_exit_on_error = $true

Framework 4.5.1

Properties {
    $namespace = "StockSniper"
    $build_dir = Split-Path $psake.build_script_file
    $output_dir = "C:\Publish\$namespace"
    $code_dir = "$build_dir"

    $ftp_site = "ftp://ftp.thestocksniper.com/thestocksniper.com/wwwroot/"
    $ftp_user = "ellerbus"
    $ftp_pass = "r3sults1"
}

FormatTaskName (("-" * 25) + "[{0}]" + ("-" * 25))

Task Default -Depends Build, DeployDatabase, Deploy

Task Clean {
    Write-Host "Creating Publication directory" -ForegroundColor Green

    if (Test-Path $output_dir) {
        Remove-Item $output_dir -Recurse -Force
    }

    mkdir $output_dir

    Write-Host "Cleaning $namespace.sln" -ForegroundColor Yellow
    Exec {
        msbuild "$code_dir\$namespace.sln" `
            /t:Clean `
            /v:minimal `
            /p:VisualStudioVersion=14.0 `
            /p:Configuration=Release
    }
}

Task Build -Depends Clean {
    Write-Host "Building $namespace.sln" -ForegroundColor Green

    Exec {

        msbuild "$code_dir\$namespace.sln" `
            /t:Build `
            /v:minimal `
            /p:VisualStudioVersion=14.0 `
            /p:Configuration=Release `
            /p:GenerateProjectSpecificOutputFolder=true `
            /p:OutDir=$output_dir
    
    }
}

Task CompressJs -Depends Build {
    Write-Host "Compressing JS $namespace.WebApplication" -ForegroundColor Green

    # Notes
    # google closure compiler
    
    $path = "$output_dir\$namespace.WebApplication\_PublishedWebsites\$namespace.WebApplication"
        
    $out_file = "$path\Content\Sniper.js"
    
    Get-ChildItem "$path\App" -Include "*.js" -Recurse | Get-Content -Raw | Out-File -FilePath $out_file
    
    Remove-Item "$path\App" -Recurse -Force

    $js = Get-Content -Path $out_file -Raw
    
    $body = @{
        js_code           = $js
        compilation_level = 'SIMPLE_OPTIMIZATIONS'
        output_info       = 'compiled_code'
        output_format     = 'text'
    }
    
    $response = Invoke-WebRequest -Uri "https://closure-compiler.appspot.com/compile" -Method Post -Body $body

    if ($response.StatusCode -ne 200) {
        throw "Unable to Minify Sniper.js"
    }

    $compressed_js = $response.Content

    $min_file = $out_file -Replace "\.js", ".min.js"

    Set-Content -Path $min_file -Value $compressed_js -Force
}

Task Test -Depends Build {
    Write-Host "Testing $namespace.sln" -ForegroundColor Green

    $path = "$output_dir\$namespace.Tests\$namespace.Tests.dll"

    $exe = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"

    Write-Host "Test: $path" -ForegroundColor Yellow

    Exec {
        & $exe $path /InIsolation
    }
}

Task DeployDatabase -Depends Build, CompressJs {
    Write-Host "Deploying $namespace.sln Database" -ForegroundColor Green
    
    $exe = "$output_dir\$namespace.Database\$namespace.Database.exe"

    Exec {
        & $exe -build:true -environment:Deployed
    }
}

Task Deploy -Depends Test, CompressJs, DeployDatabase {
    Write-Host "Deploying $namespace.sln" -ForegroundColor Green

    $local = "$output_dir\$namespace.WebApplication\_PublishedWebsites\$namespace.WebApplication"
    
    Copy-Item -Path "$code_dir\app_offline.htm" -Destination "$local\app_offline.htm"

    Set-FtpConnection $ftp_site $ftp_user $ftp_pass $local
    
    Send-ToFtp "app_offline.htm"

    Send-ToFtp "bin\$namespace.Core.dll"
    Send-ToFtp "bin\$namespace.Core.pdb"
    Send-ToFtp "bin\$namespace.WebApplication.dll"
    Send-ToFtp "bin\$namespace.WebApplication.pdb"
    Send-ToFtp "Content\"
    Send-ToFtp "Views\"
    Send-ToFtp "NLog.config"
    Send-ToFtp "robots.txt"
    Send-ToFtp "web.config"
    
    Remove-FromFtp "app_offline.htm"
    
    #	Delete Content/
    #	Delete Views/
}