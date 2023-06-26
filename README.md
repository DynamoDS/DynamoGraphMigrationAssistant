# Dynamo Graph Migration Assistant

DynamoGraphMigrationAssistant (DGMA) is a View Extension for Dynamo that allows you to prepare your previous generation ui graphs for Dynamo versions 2.13.x and higher.

<img src="https://raw.githubusercontent.com/DynamoDS/DynamoGraphMigrationAssistant/master/doc/sample.gif"/>

#### Workflow

https://user-images.githubusercontent.com/15744724/242657381-6ac21c72-24d9-4fd2-b72d-5de5f4729853.mp4

## Building

### Recommended Build Environment
- VisualStudio 2022
- .Net Framework 4.8

### Build Process
- Fork or download the repository
- ```Build``` the soluition 
- **Manifest File** (tell Dynamo where to find this Extension) - Open the ```DynamoGraphMigrationAssistant_ViewExtensionDefinition.xml``` located in the ```manifest``` folder and change the path to the ```DynamoGraphMigrationAssistantViewExtension.dll``` by providing the correct path inside the ```<AssemblyPath></AssemblyPath>``` tags (the file should be in the ```bin\Debug``` of your solution) 
- Copy the ```DynamoGraphMigrationAssistant_ViewExtensionDefinition.xml``` file from the manifest directory to the ```viewExtensions``` folder of Dynamo (this can be under the ```bin\Debug``` folder if you are working in Sandbox environment, or the ```C:\Program Files\Autodesk\Revit 202x\AddIns\DynamoForRevit\viewExtensions``` for Dynamo Revit).
-- Alternatively under the ```Build Events``` section of your solution, remove the **'REM'** in front of the Post-build event command line, and replace the **[YOUR_DYNAMO_SANDOX_LOCATION]** with the location of your Dynamo Sandbox solution (```REM copy /Y $(SolutionDir)DynamoGraphMigrationAssistantViewExtension\manifest\*.xml [YOUR_DYNAMO_SANDBOX_LOCATION]\Dynamo\bin\AnyCPU\Debug\viewExtensions```)

If you have done everything correctly, you should see 'Dynamo Migration Assistant' under the Extensions tab in Dynamo.

<img src="https://raw.githubusercontent.com/DynamoDS/DynamoGraphMigrationAssistant/master/doc/menuItem.png" width="200">

## Debugging & Testing
In order to debug or run/create Unit Tests, you will have to take a few additional steps.

## Debugging
- Download DynamoCoreRuntime 2.16.0 (or higher) from https://dynamobuilds.com/. Alternatively, you can build Dynamo from the Dynamo repository and use the bin folder equivalently.
- Copy all contents of the DynamoCoreRuntime to ```DynamoGraphMigrationAssistantViewExtension\DynamoGraphMigrationAssistantTests\bin\Debug\```. If you are building Dynamo locally, copy all contents of Dynamo from Dynamo/bin/AnyCPU/Debug to ```DynamoGraphMigrationAssistantViewExtension\DynamoGraphMigrationAssistantTests\bin\Debug\```
- Copy DynamoGraphMigrationAssistant_ViewExtensionDefinition.xml from ```DynamoGraphMigrationAssistantViewExtension\DynamoGraphMigrationAssistantViewExtension\manifests\``` to ```DynamoGraphMigrationAssistantViewExtension\DynamoGraphMigrationAssistantTests\bin\Debug\viewExtensions\```
- Open the copied ```DynamoGraphMigrationAssistant_ViewExtensionDefinition.xml``` and change the assemply path to ```..\DynamoGraphMigrationAssistantViewExtension.dll```
- Remove Export Sample Images from your Dynamo packages folder if you have it installed from the package manager (otherwise ```DynamoGraphMigrationAssistantViewExtension.dll``` will get loaded twice). 
- Launch DynamoSandbox.exe, then click View-> Open Export Sample Images and use start debugging as you normally would.

### Testing
- Install the NUnit 2 Test Adapter from VisualStudio->Extensions->Manage Extensions->Online.
- Open Test Explorer from VisualStudio->Test->Test Explorer. Now you should see a list of TuneUpTests.
- **If you are running Visual Studio 2022 and you are having issues, try using **ReSharper** and the test module it provides instead.*
- Make sure you are using 64bit Testing architecture by going to ```Test->Processor Architecture for AnyCPU Projects->x64```
- Click the target test to run or run them all.

<img src="https://user-images.githubusercontent.com/5354594/190202380-b05b7f9e-2223-4442-ba4d-16dca27d8c47.png" width="450">

## Known Issues
- At this time (up to Dynamo 2.18.x) the extension overrides the Python notification extension and the workspace references extension until new APIs are added. Related issues are here ([#7](https://github.com/DynamoDS/DynamoGraphMigrationAssistant/issues/7) and here [#11](https://github.com/DynamoDS/DynamoGraphMigrationAssistant/issues/11))
- In rare circumstances, graphs could cause a silent crash. In these scenarios, try utilizing the 'log.txt' file saved in the root folder of your Target location. The Extension would allow you to attempt and Resume your run from the last recorded graph
