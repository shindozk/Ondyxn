# Ondyxn Build Script
# Usage: .\scripts\build.ps1 [-Configuration Release] [-Clean]

param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",
    
    [switch]$Clean,
    
    [switch]$Test
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Ondyxn Build Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Clean if requested
if ($Clean) {
    Write-Host "Cleaning build artifacts..." -ForegroundColor Yellow
    dotnet clean -c $Configuration
    if ($LASTEXITCODE -ne 0) { throw "Clean failed" }
    Write-Host "Clean completed." -ForegroundColor Green
}

# Restore packages
Write-Host "`nRestoring packages..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) { throw "Restore failed" }
Write-Host "Restore completed." -ForegroundColor Green

# Build
Write-Host "`nBuilding solution ($Configuration)..." -ForegroundColor Yellow
dotnet build -c $Configuration --no-restore
if ($LASTEXITCODE -ne 0) { throw "Build failed" }
Write-Host "Build completed." -ForegroundColor Green

# Run tests if requested
if ($Test) {
    Write-Host "`nRunning tests..." -ForegroundColor Yellow
    dotnet test -c $Configuration --no-build --verbosity normal
    if ($LASTEXITCODE -ne 0) { throw "Tests failed" }
    Write-Host "Tests completed." -ForegroundColor Green
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  Build completed successfully!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
