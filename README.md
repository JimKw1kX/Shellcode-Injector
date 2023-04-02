# Shellcode-Injector

This injector uses [QueueUserAPC function](https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-queueuserapc) to inject shellcode in to a system process. This injector currently injecting to `calc.exe` calculator but users can change any processes they want. User should use a convertor such as [GadgetToJScript](https://github.com/med0x2e/GadgetToJScript/tree/master) to convert to `VBA`, `JS`, or `HTA` etc formats then execute it to bypass detections. User may also need to tweak APIs to bypass detection depends on the target they want to bypass.


# DEMO


 
