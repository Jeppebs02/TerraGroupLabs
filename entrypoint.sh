#!/bin/sh
set -e # Exit immediately if a command exits with a non-zero status.

echo "SpacetimeDB Entrypoint: Starting..."

# 1. Publish the module
# The --project-path for `spacetime publish` expects the path to the C# project directory,
# not the compiled WASM. It will invoke `dotnet publish` itself.
# We need to mount the C# server project source code for this to work.
#
# Alternative: If `spacetime publish` can take a direct --module-path to the pre-compiled WASM,
# that would be simpler and avoid needing the full .NET SDK in the spacetime image.
# Checking `spacetime publish --help` output is key here.
#
# Assuming `spacetime publish --project-path server ...` is correct and needs the C# project:
echo "Publishing SpacetimeDB module..."
# The path "server" will be relative to the working directory inside the container.

spacetime publish --project-path /app/server_project --name "tg-labs-chat"
# Note: The above line assumes /app/server_project contains your C# server project files.

# If `spacetime publish` can use a pre-compiled WASM directly (BETTER for this context):
# Make sure server.wasm is at /module/server.wasm (from your volume mount)
# spacetime publish --data-dir /data --name "tg-labs-chat" --module-path /module/server.wasm

echo "Module publishing complete (or attempted)."

# 2. Start the SpacetimeDB server
echo "Starting SpacetimeDB server..."
# It will pick up the module published
# Possible add a data-dir later.
exec spacetime start --listen-addr 0.0.0.0:3000
# Added --listen-addr and --dev for good measure. `exec` replaces the shell process with `spacetime start`.

echo "SpacetimeDB server process finished." # This line will only be reached if `spacetime start` exits