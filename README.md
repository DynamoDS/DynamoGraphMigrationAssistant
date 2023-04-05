# DynamoGraphUpdater

DynamoGraphUpdater (DGU) is a View Extension for Dynamo which allows you to prepare your previous generation ui graphs for Dynamo versions 2.13.x and higher.

#### Workflow



## Building

### Recommended Build Environment
- VisualStudio 2022
- .Net Framework 4.8

### Build Process
- Fork or download the repository
- ```Build``` the soluition 
- **Manifest File** (tell Dynamo where to find this Extension) - Open the ```DynamoGraphUpdater_ViewExtensionDefinition.xml``` located in the ```manifest``` folder and change the path to the ```DynamoGraphUpdaterViewExtension.dll``` by providing the correct path inside the ```<AssemblyPath></AssemblyPath>``` tags (the file should be in the ```bin\Debug``` of your solution) 
- Copy the ```DynamoGraphUpdater_ViewExtensionDefinition.xml``` file from the manifest directory to the ```viewExtensions``` folder of Dynamo (this can be under the ```bin\Debug``` folder if you are working in Sandbox environment, or the ```C:\Program Files\Autodesk\Revit 202x\AddIns\DynamoForRevit\viewExtensions``` for Dynamo Revit).
-- Alternatively under the ```Build Events``` section of your solution, remove the **'REM'** infront of the Post-build event command line, and replace the **[YOUR_DYNAMO_SANDOX_LOCATION]** with the location of your Dynamo Sandbox solution (```REM copy /Y $(SolutionDir)DynamoGraphUpdaterViewExtension\manifest\*.xml [YOUR_DYNAMO_SANDBOX_LOCATION]\Dynamo\bin\AnyCPU\Debug\viewExtensions```)

If you have done eveything correctly, you should see 'Show Dynamo Graph Updater Extension' under the Extensions tab in Dynamo.

<img src="https://user-images.githubusercontent.com/5354594/186402333-7c49302b-a544-41ec-8dc2-20310c215419.png" width="200">

## Debugging & Testing
In order to debug or run/create Unit Tests, you will have to take a few additional steps.

## Debugging
- Download DynamoCoreRuntime 2.16.0 (or higher) from https://dynamobuilds.com/. Alternatively, you can build Dynamo from Dynamo repository and use the bin folder equivalently.
- Copy all contents of the DynamoCoreRuntime to ```DynamoGraphUpdaterViewExtension\DynamoGraphUpdaterTests\bin\Debug\```. If you are building Dynamo locally, copy all contents of Dynamo from Dynamo/bin/AnyCPU/Debug to ```DynamoGraphUpdaterViewExtension\DynamoGraphUpdaterTests\bin\Debug\```
- Copy DynamoGraphUpdater_ViewExtensionDefinition.xml from ```DynamoGraphUpdaterViewExtension\DynamoGraphUpdaterViewExtension\manifests\``` to ```DynamoGraphUpdaterViewExtension\DynamoGraphUpdaterTests\bin\Debug\viewExtensions\```
- Open the copied ```DynamoGraphUpdater_ViewExtensionDefinition.xml``` and change the assemply path to ```..\DynamoGraphUpdaterViewExtension.dll```
- Remove Export Sample Images from your Dynamo packages folder if you have it installed from package manager (otherwise ```DynamoGraphUpdaterViewExtension.dll``` will get loaded twice). 
- Launch DynamoSandbox.exe, then click View-> Open Export Sample Images and use start debugging as you normally would.

### Testing
- Install NUnit 2 Test Adapter from VisualStudio->Extensions->Manage Extensions->Online.
- Open Test Explorer from VisualStudio->Test->Test Explorer. Now you should see a list of TuneUpTests.
- **If you are running Visual Studio 2022 and you are having issues, try using **ReSharper** and the test module it provides instead.*
- Make sure you are using 64bit Testing architecture by going to ```Test->Processor Architecture for AnyCPU Projects->x64```
- Click the target test to run or run them all.

<img src="https://user-images.githubusercontent.com/5354594/190202380-b05b7f9e-2223-4442-ba4d-16dca27d8c47.png" width="450">

## Known Issues
- Runing graphs under Dynamo or Revit could trigger Notification Dialogs (prompts) which will interrupt the flow.
- In rare circumstances, graphs could cause silent crash. In these scenarious, try utilizing the 'log.txt' file saved in the root folder of your Target location. The Extension would allow you to attemt and Resume your run from the last recorded graph
