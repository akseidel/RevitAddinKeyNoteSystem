# RevitAddinKeyNoteSystem&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![](RevitAddinKeyNoteSystem/Images/KNE_ribbon_panel_image1.jpg)
The RevitAddinKeyNoteSystem is a Revit add-in that is the Revit ribbon component for the **WpfRevitUserKeynoteManager** application.


### RevitAddinKeyNoteSystem

The ribbon panel RevitAddinKeyNoteSystem creates handles four basic needs the user needs in regard to Revit User Keynotes:

- **KeyNote Editor** – launches the **WpfRevitUserKeynoteManager**<sup>1</sup> application, a User Keynote editor, with the current Revit project's User Keynote file loaded up ready for creating and editing User Keynotes. This editor provides a few convenient editing features germane to User Keynote table files. It allows more than one person to edit the same User Keynotes file at the same time, provided they edit different keynote categories.

- **UpDate** - reloads the current Revit project's User Keynote table file.

- **KeyNote** - launches the Revit place keynote command.

- **Displays Help** – explains how to use the **WpfRevitUserKeynoteManager** application by launching it in its own documentation mode.

<sup>1</sup>:The **WpfRevitUserKeynoteManager** repository is here: [WpfRevitUserKeynoteManager][6f3f15e6]

  [6f3f15e6]: https://github.com/akseidel/WpfRevitUserKeynoteManager "https://github.com/akseidel/WpfRevitUserKeynoteManager"

#### Details

- The full pathname to the **WpfRevitUserKeynoteManager** application is soft coded in the **RevitAddinKeyNoteSystem** add-in as the "KNManager" setting in the RevitAddinKeyNoteSystem application's Properties.Settings.Default. The add-in checks the pathname's existence when the "KeyNote Editor" command button is pressed. If that pathname does not exist then a dialog shows asking to point to the **WpfRevitUserKeynoteManager** application. Whatever is selected then is stored in the user's Properties.Settings.Default to be retrieved the next time.
- The **WpfRevitUserKeynoteManager** application has its own documentation feature. The **RevitAddinKeyNoteSystem** ribbon panel help button runs the **WpfRevitUserKeynoteManager** "in help mode" so to speak. That button assumes the **WpfRevitUserKeynoteManager** application is ***not running***. That button will do nothing if the **WpfRevitUserKeynoteManager** application ***is running*** because **WpfRevitUserKeynoteManager** is a single running instance WPF application. There is more information about this in the  **WpfRevitUserKeynoteManager** repository.   

#### Installation

- Place the **WpfRevitUserKeynoteManager** application and its RTF instructions file, i.e. not this RevitAddinKeyNoteSystem Revit add-in but the program it launches,  at a path where Revit users have access the application.
- Place this RevitAddinKeyNoteSystem add-in \*.dll and its \*.addin manifest file where Revit expects to find add-ins. There are a few places. %AppData%\\Roaming\\Autodesk\\Revit\\Addins\\\<yyyy\> is one.

###### How to get the add-in \*.dll and its \*.addin manifest file?
- Use the already built files.
  - The folder 'AlreadyBuilt' in this repository has an already built version of the add-in dll and the add-in addin manifest file. The editor path in this add-in is soft coded to a path that will not apply. Therefore on first run the editor's pathname will be requested.
- Or build it in Visual Studio.
  - In VS Studio edit the Properties.Settings.Default.KNManager default setting for this RevitAddinKeyNoteSystem project to be the **WpfRevitUserKeynoteManager** application's full pathname. This optional step allows the add-in to innately know where the **WpfRevitUserKeynoteManager** application is.
  - In VS Studio edit the build events as needed. They are currently setup to sign the add-in so that Revit 2017 does not complain. Unless you know how to sign the application this way these build events will need to be removed. How the signing works is not going to be explained here. One set of build events places the add-in \*.dll and its \*.addin manifest file in a "FreshBuilds" folder. That has been found to be convenient.
  - In VS fix the reference locations and rebuild the application.
