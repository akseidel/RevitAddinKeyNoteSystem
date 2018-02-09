![](RevitAddinKeyNoteSystem/Images/KNE_ribbon_panel_image1.jpg)
# RevitAddinKeyNoteSystem
The RevitAddinKeyNoteSystem is a Revit add-in that is the Revit ribbon component to the **WpfRevitUserKeynoteManager** application.


### RevitAddinKeyNoteSystem

The ribbon panel created by the RevitAddinKeyNoteSystem directly presents four basic needs the user needs in regard to Revit User Keynotes. These are:

- **KeyNote Editor** launches the User Keynote editor with the the current Revit project's User Keynote file loaded up ready for creating and editing user keynotes. The User Keynote file editor is the **WpfRevitUserKeynoteManager** application. This editor provides a few convenient editing features germane to User Keynote table files. This editor allows more than one person at a time to edit the same User Keynotes file at the same time provided they are each editing different keynote categories.

- **UpDate** reloads the current Revit project's User Keynote table file.

- **KeyNote** issues the Revit place keynote command.

- **Displays Help** regarding how to use the **WpfRevitUserKeynoteManager** application. The add-in's ribbon panel pulldown is where to find this command. The **WpfRevitUserKeynoteManager** application contains its own documentation. The button in the ribbon pulldown actually launches the **WpfRevitUserKeynoteManager** application in its own documentation mode instead of with the current project file's user keynote table preloaded.

The **WpfRevitUserKeynoteManager** repository is here: [WpfRevitUserKeynoteManager][6f3f15e6]

  [6f3f15e6]: https://github.com/akseidel/WpfRevitUserKeynoteManager "https://github.com/akseidel/WpfRevitUserKeynoteManager"

#### Details

- As of this writing the path to the **WpfRevitUserKeynoteManager** application is hard coded in the **RevitAddinKeyNoteSystem** add-in.
- The **WpfRevitUserKeynoteManager** application has its own documentation feature. The **RevitAddinKeyNoteSystem** ribbon panel help button runs the **WpfRevitUserKeynoteManager** "in help mode" so to speak. That button assumes the **WpfRevitUserKeynoteManager** application is ***not*** running. That button will do nothing if the **WpfRevitUserKeynoteManager** application ***is*** running because **WpfRevitUserKeynoteManager** is a single running instance WPF application.  

(unfinished documentation)
