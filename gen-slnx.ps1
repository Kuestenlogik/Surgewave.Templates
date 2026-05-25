Set-Location $PSScriptRoot
$lines = @('<Solution>')
$lines += '  <Folder Name="/src/">'

# Include the template pack project
Get-ChildItem src -Recurse -Filter *.csproj -Depth 1 | ForEach-Object {
    $relativePath = $_.FullName.Substring((Get-Location).Path.Length + 1).Replace('\', '/')
    $lines += "    <Project Path=`"$relativePath`" />"
}

$lines += '  </Folder>'
$lines += '</Solution>'
$lines | Out-File -FilePath Kuestenlogik.Surgewave.Templates.slnx -Encoding utf8
Write-Host "Generated Kuestenlogik.Surgewave.Templates.slnx with $($lines.Count) lines"
