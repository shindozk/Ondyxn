#!/bin/bash
# Ondyxn Build Script for Linux/macOS
# Usage: ./scripts/build.sh [-c Release] [--clean] [--test]

set -e

CONFIGURATION="Debug"
CLEAN=false
TEST=false

# Parse arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        -c|--configuration)
            CONFIGURATION="$2"
            shift 2
            ;;
        --clean)
            CLEAN=true
            shift
            ;;
        --test)
            TEST=true
            shift
            ;;
        *)
            echo "Unknown option: $1"
            exit 1
            ;;
    esac
done

echo "========================================"
echo "  Ondyxn Build Script"
echo "========================================"
echo ""

# Clean if requested
if [ "$CLEAN" = true ]; then
    echo "Cleaning build artifacts..."
    dotnet clean -c "$CONFIGURATION"
    echo "Clean completed."
fi

# Restore packages
echo ""
echo "Restoring packages..."
dotnet restore
echo "Restore completed."

# Build
echo ""
echo "Building solution ($CONFIGURATION)..."
dotnet build -c "$CONFIGURATION" --no-restore
echo "Build completed."

# Run tests if requested
if [ "$TEST" = true ]; then
    echo ""
    echo "Running tests..."
    dotnet test -c "$CONFIGURATION" --no-build --verbosity normal
    echo "Tests completed."
fi

echo ""
echo "========================================"
echo "  Build completed successfully!"
echo "========================================"
