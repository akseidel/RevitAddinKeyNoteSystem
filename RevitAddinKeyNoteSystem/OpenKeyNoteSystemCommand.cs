
#region "Imported Namespaces"
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
#endregion

namespace RevitAddinKeyNoteSystem
{
    [Transaction(TransactionMode.Manual)]
    public class OpenKeyNoteSystemCommand : IExternalCommand
    {
        /// <summary>
        /// The one and only method required by the IExternalCommand interface, the main entry point for every external command.
        /// </summary>
        /// <param name="commandData">Input argument providing access to the Revit application, its documents and their properties.</param>
        /// <param name="message">Return argument to display a message to the user in case of error if Result is not Succeeded.</param>
        /// <param name="elements">Return argument to highlight elements on the graphics screen if Result is not Succeeded.</param>
        /// <returns>Cancelled, Failed or Succeeded Result code.</returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
                Document thisDoc = uidoc.Document;

                string _KNPath = string.Empty;
                string _KNFile = string.Empty;
                ModelPath mP = KeynoteTable.GetKeynoteTable(thisDoc).GetExternalFileReference().GetPath();
                string knF = ModelPathUtils.ConvertModelPathToUserVisiblePath(KeynoteTable.GetKeynoteTable(thisDoc).GetExternalFileReference().GetPath());
                try
                {
                    if (!knF.Equals(string.Empty))
                    {
                        if (File.Exists(knF))
                        {
                            _KNPath = Path.GetDirectoryName(knF);
                            _KNFile = Path.GetFileName(knF);
                            SomeKNHelpers.OpenKeyNoteManager(_KNPath, _KNFile);
                        }
                        else
                        {
                            MessageBox.Show("Looked for: " + knF, "No File Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("There is no keynote table file associated to this Revit file." + knF, "No Keynote File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Trouble: " + err.Message, "Trouble");
                    // SomeHelpers.TaskDialogRequirements();
                }
            }
            catch (Autodesk.Revit.Exceptions.ArgumentNullException)
            {
                MessageBox.Show("Error. This file probably has never been saved, so it has no path to follow.", "Sorry Charlie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaskDialogRequirements();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. This file probably does not have a central model. " + ex.Message, "Sorry Charlie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaskDialogRequirements();
            }
            //Must return some code
            return Result.Succeeded;
        }

        public static void TaskDialogRequirements()
        {
            TaskDialog mainDialog = new TaskDialog("So It Did Not Work?");
            string msg = "These are the requirements for this to work.\n\n";
            msg = msg + "1) The Revit keynote filename must end with the text, \"*_Keyed_Notes.txt\"\n\n";
            msg = msg + "2) The Revit keynote file must be in a folder named \"KeyNotes\" at this project's central file location.\n\n";
            msg = msg + "3) This Revit file must be associated to a \"central\" Revit file or be a \"central\" Revit file.";
            mainDialog.MainInstruction = msg;

            string msg2 = "You will be prompted to open the correct file if for some reason there are multiple files named to end with \"*_Keyed_Notes.txt\".";
            msg2 = msg2 + " Otherwise, the single keynote file will automatically open. If you want to keep the mutliple files then make sure to alter the tail end of their names so ";
            msg2 = msg2 + "that they cannot match the \"*_Keyed_Notes.txt\" pattern.";
            mainDialog.MainContent = msg2;

            mainDialog.TitleAutoPrefix = false;
            mainDialog.VerificationText = "Are you?";
            TaskDialogResult tResult = mainDialog.Show();
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class KeyNoteCommand : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                RevitCommandId id_built_in;
                id_built_in = RevitCommandId.LookupPostableCommandId(PostableCommand.UserKeynote);
                uiapp.PostCommand(id_built_in);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry Charlie - not able to post command UserKeynote");
            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class KeyNoteUpdate : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;
                IntPtr revitHandle = Process.GetCurrentProcess().MainWindowHandle;
                using (Transaction t = new Transaction(doc, "UserKeynoteFileReload"))
                {
                    t.Start();
                    ModelPath mP = KeynoteTable.GetKeynoteTable(doc).GetExternalFileReference().GetPath();
                    string knF = ModelPathUtils.ConvertModelPathToUserVisiblePath(KeynoteTable.GetKeynoteTable(doc).GetExternalFileReference().GetPath());
                    if (File.Exists(knF))
                    {
                        //Instead we will just Reload instead of LoadFrom, which also works.
                        KeynoteTable.GetKeynoteTable(doc).Reload(null);
                        RevitHelpers.SetStatusBarText(revitHandle, "Did reload " + Path.GetFileName(knF) + " by gum!");
                    }
                    else
                    {
                        MessageBox.Show(knF, "File Not Found!");
                    }
                    t.Commit();
                } // end using
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry Charlie - Unable to update the UserKeynotes!");
            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class OpenKeyNoteManagerInHelp : IExternalCommand
    {
        private string hflag = "-h";
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
                Document thisDoc = uidoc.Document;

                try
                {
                    SomeKNHelpers.OpenKeyNoteManager(hflag, string.Empty);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Trouble: " + err.Message, "Trouble");
                }
            }
            catch (Autodesk.Revit.Exceptions.ArgumentNullException)
            {
                MessageBox.Show("Autodesk.Revit.Exceptions.ArgumentNullException Error", "Sorry Charlie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some Error: " + ex.Message, "Sorry Charlie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //Must return some code
            return Result.Succeeded;
        }
    }
    
    public class SomeKNHelpers
    {
        public static void OpenKeyNoteManager(string _KNPath, string _KNFile)
        {
            // startup in help mode is rude and cude
            string hflag = "-h";
            const string appKNManager = @"N:\\COMMON\\UTILS\\RevitKNManager\\WpfRevitUserKeynoteManager.exe";
            try
            {
                if (File.Exists(appKNManager))
                {
                    if (!string.IsNullOrEmpty(_KNFile))
                    {
                        if (_KNPath.Contains(" "))
                        {
                            _KNPath = string.Concat("\"", string.Concat(_KNPath, "\""));
                        }
                        if (_KNFile.Contains(" "))
                        {
                            _KNFile = string.Concat("\"", string.Concat(_KNFile, "\""));
                        }
                        string _args = _KNPath + " " + _KNFile;
                        Process.Start(appKNManager, _args);
                    }
                    else
                    {
                        if (_KNPath.Equals(hflag, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Process.Start(appKNManager, hflag);
                        }
                        else
                        {
                            Process.Start(appKNManager);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(appKNManager + " not found.", "Hello", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error trying to run " + appKNManager, "Hello", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show(ex.Message, "Sorry Charlie");
            }
        }

        //required for passing filepath arguments that contain spaces
        public static string QuotdStr(string sTr)
        {
            return "\"" + sTr + "\"";
        }

    }
}
