Adding the DirectX library
------------------------------------------------------
1-Right click refrences tab on the solution explorer and select add reference.
2-Browse to the path (C:\Windows\Microsoft.NET\DirectX for Managed Code\1.0.2902.0\Microsoft.DirectX.DirectInput.dll).
3- Select the previous DLL and add it to your refrences.
###########################################################################
Add the App.config file
------------------------------
1-Right click on the project name in the solution explorer and select "add" then "New item" then "Application Configuration file"
2-In this file write the following tag 
  <startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0"/>
  </startup>
3- Save and close the file 