# Get-Writable
 
Searches for world writable directories that contain either a .exe, a .dll, or both.

## Usage

`Get-Writable.exe <path to start search>`

Example:
```
Get-Writable.exe "C:\\"
(dll+exe),C:\Program Files (x86)\GOG Galaxy\Games\Tooth and Tail
(dll),C:\Program Files (x86)\GOG Galaxy\Games\Tooth and Tail\fr
...(snip)...
```
