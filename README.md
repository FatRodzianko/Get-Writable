# Get-Writable
 
Searches for directories that contain a world writable .exe, a .dll, script file (.ps1, .bat, .sct, .cmd), or a .sys file.

## Usage

`Get-Writable.exe <path to start search>`

Example:
```
Get-Writable.exe "C:\\"
(dll+exe),C:\Program Files (x86)\GOG Galaxy\Games\Tooth and Tail
(dll),C:\Program Files (x86)\GOG Galaxy\Games\Tooth and Tail\fr
...(snip)...
```

Most of the output should be straight forward. `(exe), C:\Directory` means that the directory contains a writable exe, and so on. If you see `(directory+exe), C:\Directory`, that means that there were no files discovered that were writable, but that the `USERS` or `EVERYONE` groups have write permissions to the directory, meaning you can write arbitrary files to the directory but can't change the files that are already there. This could represent an opportunity for a DLL sideloading when those discovered exes are executed.
