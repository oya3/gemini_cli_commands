param (
    [string]$inputFile
)

if (-not $inputFile) {
    Write-Error "Please specify an input file."
    exit 1
}

$inputPath = Resolve-Path $inputFile
$outputPath = [System.IO.Path]::ChangeExtension($inputPath.ProviderPath, ".pdf")

try {
    $word = New-Object -ComObject Word.Application
    $word.Visible = $false
    $doc = $word.Documents.Open($inputPath.ProviderPath)
    $doc.SaveAs([ref]$outputPath, 17) # 17 is wdFormatPDF
    $doc.Close()
    $word.Quit()
    Write-Host "Converted: $($inputPath.ProviderPath) -> $outputPath"
} catch {
    Write-Error "Error during conversion: $_"
    if ($word) { $word.Quit() }
    exit 1
}
