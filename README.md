[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Follow @JimKwik_X](https://img.shields.io/twitter/follow/JimKwik_X?style=social)](https://x.com/JimKwik_X)

# Shellcode-Injector

# Info:
A Shellcode injector that bypasses Attack Surface Reduction (ASR) rule of blocking arbitrary code execution. This script was created from the knowledge gained from [RTO2 Course](https://training.zeropointsecurity.co.uk/courses/red-team-ops-ii).


# Overview:

This injector uses [QueueUserAPC function](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-queueuserapc) to inject shellcode from C2s such as Cobalt Strike in to a system process. This injector currently injecting to `calc.exe` calculator but users can change any processes they want. User should use a convertor such as [GadgetToJScript](https://github.com/med0x2e/GadgetToJScript/tree/master) to convert to `VBA`, `JS`, or `HTA` etc formats then execute it to bypass detections. User may also need to tweak APIs to bypass detection depends on the target they want to bypass.


# DEMO

1. Generate shellcode in `C#` format and add to the injector, then use [GadgetToJScript](https://github.com/med0x2e/GadgetToJScript/tree/master) to convet it to `Js` format then execute.

![exeucte](https://github.com/JimSolomon/Shellcode-Injector/blob/main/2023-04-02_12-18.png)
 
Cobalt Strike received a beacon
![cs](https://github.com/JimSolomon/Shellcode-Injector/blob/main/2023-04-02_13-35.png)

2. Geneate a `VBA` format and inject to Word Macros

![VBA](https://github.com/JimSolomon/Shellcode-Injector/blob/main/VBA.png)
![Macros](https://github.com/JimSolomon/Shellcode-Injector/blob/main/Inject%20to%20Macros.png)

Cobalt Strike received a beacon
![beacon](https://github.com/JimSolomon/Shellcode-Injector/blob/main/Beacon%20from%20Macros.png)


