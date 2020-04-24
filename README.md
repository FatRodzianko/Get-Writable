# Get-Writable
 
Searches for directories that contain a world writable .exe, a .dll, script file (.ps1, .bat, .sct), or a .sys file.

## Usage

`Get-Writable.exe <path to start search>`

Example:
```
Get-Writable.exe "C:\\"
(dll+exe),C:\Program Files (x86)\GOG Galaxy\Games\Tooth and Tail
(dll),C:\Program Files (x86)\GOG Galaxy\Games\Tooth and Tail\fr
...(snip)...
```
