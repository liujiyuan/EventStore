psake.cmd native-code.ps1  Copy-V8ToLibs -parameters @{ 'platform'='x64'; 'Configuration' = 'Debug'} -properties @{ 'platformToolset' = 'v100'}