[string]$script:ftpHost
[string]$script:username
[string]$script:password
[string]$script:local
[System.Net.NetworkCredential]$script:Credentials 

function Set-FtpConnection($host, $username, $password, $local) {
    $script:Credentials = New-Object System.Net.NetworkCredential($username, $password) 
    $script:ftpHost  = $host
    $script:username = $username
    $script:password = $password
    $script:local = $local
}

function Remove-FromFtp ($item) {

    $local = Get-Local-Path $item

    $type = "file"

    if (Test-Path -Path $local -PathType Leaf) {
        $type = "file"
    }
    else{
        $type = "directory"
    }

    Write-Host "FTP Deleting $item..."

    $ftp_req = Create-FtpRequest $item
    $ftp_req.UseBinary = $true 
    $ftp_req.KeepAlive = $false 
    
    if($type -ieq "file") {
        $ftp_req.Method = [System.Net.WebRequestMethods+Ftp]::DeleteFile 
    } else {
        $ftp_req.Method = [System.Net.WebRequestMethods+Ftp]::RemoveDirectory 
    }
    
    $ftp_resp = $ftp_req.GetResponse()

    Write-Host " - Status: " + $ftp_resp.StatusDescription
}

function Send-ToFtp($item) { #, $ftpFolder) {
    Write-Host "FTP Uploading $item..."

    $local = Get-Local-Path $item
    
    if (Test-Path -Path $local -PathType Leaf) {
        Send-Single-File $item
    }
    else{
        foreach($x in Get-ChildItem -Recurse $local) {
            $path = [System.IO.Path]::GetFullPath($x.FullName).SubString([System.IO.Path]::GetFullPath($script:local).Length + 1)
            
            if ($x.Attributes -eq "Directory") {
                try {
                    $ftp_req = Create-FtpRequest $path
                    $ftp_req.Method = [System.Net.WebRequestMethods+Ftp]::MakeDirectory
                    $ftp_req.GetResponse()
                }
                catch [Net.WebException] {
                    Write-Host "$path probably exists ..."
                }

                continue;
            }
            Send-Single-File $path
        }
    }
}

function Send-Single-File ($item) {
    
    $path = Get-Local-Path $item

    $ftp_req = Create-FtpRequest $item
    $ftp_req.UseBinary = $true 
    $ftp_req.UsePassive = $true
    
    $ftp_req.Method = [System.Net.WebRequestMethods+Ftp]::UploadFile

    $content = gc -Encoding Byte $path

    $ftp_req.ContentLength = $content.Length

    $ftp_stream = $ftp_req.GetRequestStream()
    
    $ftp_stream.Write($content, 0, $content.Length)

    $ftp_stream.Close()

    $ftp_stream.Dispose()

    Write-Host " - Status: " + $ftp_req.StatusDescription

}

function Get-Local-Path($item) {
    $local = [System.IO.Path]::Combine($script:local, $item)

    $local = [System.IO.Path]::GetFullPath($local)

    return $local
}

function Create-FtpRequest ($path) {
    $full_path = [System.IO.Path]::Combine($script:ftpHost, $path)

    Write-Host "$full_path"
    
    $ftp_req = [System.Net.FtpWebRequest]::Create($full_path)

    $ftp_req.Credentials = $script:Credentials

    return $ftp_req
}



#function Get-FromFtp {
#    param([string]$sourceFolder, 
#          [string]$ftpFolder)
           
#    $fullFtpPath = [System.IO.Path]::Combine($script:ftpHost, $ftpFolder)
#    $dirs = Get-FtpDirecoryTree $fullFtpPath
#    foreach($dir in $dirs){
#       $path = [io.path]::Combine($sourceFolder, $dir)
#       if ((Test-Path $path) -eq $false) {
#          New-Item -Path $path -ItemType Directory | Out-Null
#       }
#    }
#    $files = Get-FtpFilesTree $fullFtpPath
#    foreach($file in $files){
#        $ftpPath = $fullFtpPath + "/" + $file
#        $localFilePath = [io.path]::Combine($sourceFolder, $file)
#        Write-Host "Downloading $ftpPath ..."
#        Get-FtpFile $ftpPath $localFilePath
#    }
#}

#function Remove-FromFtp($ftpFolder) {
#    $fullFtpPath = [System.IO.Path]::Combine($script:ftpHost, $ftpFolder)
#    $fileTree = Get-FtpFilesTree $fullFtpPath
#    if($fileTree -gt 0){
#        foreach($file in $fileTree) {
#            $ftpFile = [io.path]::Combine($fullFtpPath, $file)
#            Remove-FtpItem $ftpFile "file"
#        }
#    }
#    $dirTree = [array](Get-FtpDirecoryTree $fullFtpPath) | sort -Property @{ Expression = {$_.Split('/').Count} } -Desc
#    if($dirTree -gt 0) {
#        foreach($dir in $dirTree) {
#            $ftpDir = [io.path]::Combine($fullFtpPath, $dir)
#            Remove-FtpItem $ftpDir "directory"
#        }
#    }
#}

#function Get-FtpDirecoryTree($fullFtpPath) {    
#    if($fullFtpPath.EndsWith("/") -eq $false) {
#        $fullFtpPath = $fullFtpPath += "/"
#    }
    
#    $folderTree = New-Object "System.Collections.Generic.List[string]"
#    $folders = New-Object "System.Collections.Generic.Queue[string]"
#    $folders.Enqueue($fullFtpPath)
#    while($folders.Count -gt 0) {
#        $folder = $folders.Dequeue()
#        $directoryContent = Get-FtpDirectoryContent $folder		
#        $dirs = Get-FtpDirectories $folder
#        foreach ($line in $dirs){
#            $dir = @($directoryContent | Where { $line.EndsWith($_) })[0]
#            [void]$directoryContent.Remove($dir)
#            $folders.Enqueue($folder + $dir + "/")
#            $folderTree.Add($folder.Replace($fullFtpPath, "") + $dir + "/")
#        }
#    }
#    return ,$folderTree
#}

#function Get-FtpFilesTree($fullFtpPath) {
#    if($fullFtpPath.EndsWith("/") -eq $false) {
#        $fullFtpPath = $fullFtpPath += "/"
#    }
    
#    $fileTree = New-Object "System.Collections.Generic.List[string]"
#    $folders = New-Object "System.Collections.Generic.Queue[string]"
#    $folders.Enqueue($fullFtpPath)
#    while($folders.Count -gt 0){
#        $folder = $folders.Dequeue()
#        $directoryContent = Get-FtpDirectoryContent $folder
#        $dirs = Get-FtpDirectories $folder
#        foreach ($line in $dirs){
#            $dir = @($directoryContent | Where { $line.EndsWith($_) })[0]
#            [void]$directoryContent.Remove($dir)
#            $folders.Enqueue($folder + $dir + "/")
#        }
#        $directoryContent | ForEach { 
#            $fileTree.Add($folder.Replace($fullFtpPath, "") + $_) 
#        }
#    }

#    return ,$fileTree
#}

#function Get-FtpDirectories($folder) {
#    $dirs = New-Object "system.collections.generic.list[string]"
#    $operation = [System.Net.WebRequestMethods+Ftp]::ListDirectoryDetails
#    $reader = Get-Stream $folder $operation
#    while (($line = $reader.ReadLine()) -ne $null) {
#       if ($line.Trim().ToLower().StartsWith("d") -or $line.Contains(" <DIR> ")) {
#            $dirs.Add($line)
#        }
#    }
#    $reader.Dispose();
#    return ,$dirs
#}

#function Get-FtpDirectoryContent($folder) {
#    $files = New-Object "System.Collections.Generic.List[String]"
#    $operation = [System.Net.WebRequestMethods+Ftp]::ListDirectory
#    $reader = Get-Stream $folder $operation
#    while (($line = $reader.ReadLine()) -ne $null) {
#       $files.Add($line.Trim()) 
#    }
#    $reader.Dispose();
#    return ,$files
#}

#function Get-FtpFile($ftpPath, $localFilePath) { 
#    $ftp_req = [System.Net.FtpWebRequest]::create($ftpPath) 
#    $ftp_req.Credentials = $script:Credentials
#    $ftp_req.Method = [System.Net.WebRequestMethods+Ftp]::DownloadFile 
#    $ftp_req.UseBinary = $true 
#    $ftp_req.KeepAlive = $false 
#    $ftp_resp = $ftp_req.GetResponse() 
#    $responseStream = $ftp_resp.GetResponseStream() 
    
#    [byte[]]$readBuffer = New-Object byte[] 1024
#    $targetFile = New-Object IO.FileStream ($localFilePath, [IO.FileMode]::Create) 
#    while ($readLength -ne 0) { 
#        $readLength = $responseStream.Read($readBuffer,0,1024) 
#        $targetFile.Write($readBuffer,0,$readLength) 
#    } 
    
#    $targetFile.close() 
#} 

#function Get-Stream($url, $meth) {
#    $fullFtpPath = [System.Net.WebRequest]::Create($url)
#    $fullFtpPath.Credentials = $script:Credentials
#    $fullFtpPath.Method = $meth
#    $response = $fullFtpPath.GetResponse()
#    return New-Object IO.StreamReader $response.GetResponseStream()
#}

Export-ModuleMember Set-FtpConnection, Remove-FromFtp, Send-ToFtp #, Send-ToFtp, Get-FromFtp, Remove-FromFtp