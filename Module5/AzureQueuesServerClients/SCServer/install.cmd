set "_path=%~dp0"
for %%a in ("%_path%") do set "p_dir=%%~dpa"
for %%a in (%p_dir:~0,-1%) do set "p2_dir=%%~dpa"
set p="%p2_dir%Public\server\FileServerService.exe"
SC create FileServerService displayname="File Server Service" binPath="%p%" start=auto
SC failure FileServerService reset= 60000 actions=restart/1000/restart/1000/run/1000 command="%p%"
pause