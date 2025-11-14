using System.Runtime.InteropServices;

[assembly: Guid("a80553e8-cd38-426d-a5a7-2dc08afb8db3")]
[assembly: System.CLSCompliant(false)]

/*
if $(ConfigurationName) == Release (
..\..\..\..\ILMerge.exe  "/out:$(ProjectDir)..\bin\$(TargetFileName)" "$(TargetDir)$(TargetFileName)" "$(TargetDir)AxMSTSCLib.DLL" "$(TargetDir)MSTSCLib.DLL" "/lib:..\..\..\SAL\bin"
)
 */