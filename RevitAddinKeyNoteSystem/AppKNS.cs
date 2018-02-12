#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
#endregion

namespace RevitAddinKeyNoteSystem
{
    class AppKNS : IExternalApplication
    {
        static string _path = typeof(Autodesk.Revit.ApplicationServices.Application).Assembly.Location;
        static string _appKNManager = Properties.Settings.Default.KNManagerFN;
        /// Singleton external application class instance.
        internal static AppKNS _app = null;
        /// Provide access to singleton class instance.
        public static AppKNS Instance {
            get { return _app; }
        }
        /// <summary>
        /// Return the KNManager application fullpathnane to run. This is being stored
        /// in Properties.Settings.Default. If the name is blank or does not point to
        /// a valid file then ask the user to find it. The choice will be saved to settings
        /// and then returned.
        /// </summary>
        public static string AppKNManager{
            get {
                if (File.Exists(_appKNManager)) {
                    return _appKNManager;
                } else { // The external exectutable filename is not known.
                    // Ask user to point to the application.
                    OpenFileDialog oFD = new OpenFileDialog {
                        Filter = "UserKeynoteManager File|*.exe",
                        Title = "Please Find the UserKeynoteManager Program File (To Be Remembered)"
                    };
                    // Assume the selected file is it and save to settings. 
                    if (oFD.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        _appKNManager = oFD.FileName;
                        Properties.Settings.Default.KNManagerFN = _appKNManager;
                        Properties.Settings.Default.Save();
                        return _appKNManager;
                    } 
                }
                // fall through to nothing
                return string.Empty;
            }
        }
        
        public Result OnStartup(UIControlledApplication a)
        {
            _app = this;
            _appKNManager = Properties.Settings.Default.KNManagerFN;
            AddKeyNoteSystem_This_Ribbon(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        public void AddKeyNoteSystem_This_Ribbon(UIControlledApplication a)
        {
            string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string ExecutingAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            // Create ribbon tab
            // Change to any existing custom ribbon or change name as you want.
            String thisNewTabName = "KeyNote";
            try
            {
                a.CreateRibbonTab(thisNewTabName);
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                // Assume error generated is due to "WTA" already existing
            }
            //   Create a push button in the ribbon panel 
            PushButtonData pbData = new PushButtonData("KeyNoteEditor", "KeyNote\nEditor", ExecutingAssemblyPath, ExecutingAssemblyName + ".OpenKeyNoteSystemCommand");
            PushButtonData pbDataA = new PushButtonData("Keynote", "KeyNote", ExecutingAssemblyPath, ExecutingAssemblyName + ".KeyNoteCommand");
            PushButtonData pbDataB = new PushButtonData("Update", "UpDate", ExecutingAssemblyPath, ExecutingAssemblyName + ".KeyNoteUpdate");

            //   Add new ribbon panel. 
            String thisNewPanelName = "KeyNote Editor";
            RibbonPanel thisNewRibbonPanelKN = a.CreateRibbonPanel(thisNewTabName, thisNewPanelName);
            // add button to ribbon panel
            PushButton pushButton = thisNewRibbonPanelKN.AddItem(pbData) as PushButton;

            // provide stackedbutton tips  - Note: This needs to happen before the ADD command.
            pbDataA.ToolTip = "Place a User Keynote.";
            string msgA = "Issues Revit command to place a User Keynote.";
            pbDataA.LongDescription = msgA;

            pbDataB.ToolTip = "Reload User Keynotes.";
            string msgB = "Reloads whichever User Keynote file this project is set to use.";
            pbDataB.LongDescription = msgB;
            
            // add stacked buttons to ribbon panel
            List<RibbonItem> keynoteButtons = new List<RibbonItem>();
            keynoteButtons.AddRange(thisNewRibbonPanelKN.AddStackedItems(pbDataA, pbDataB));

            //   Set the large image shown on button
            //Note that the full image name is namespace_prefix + "." + the actual imageName);
            pushButton.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".keynotes.png");

            // provide button tips
            pushButton.ToolTip = "Opens the User KeyNotes Editor.";
            string msg = "Opens the User KeyNote file associated to this Revit file using";
            msg = msg + " the KeyNoteManager application.";
            pushButton.LongDescription = msg;
            pushButton.ToolTipImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".KeyNoteMan.PNG");

            AddInfoSlideOut(thisNewRibbonPanelKN);
        } // AddKeyNoteSystem_This_Ribbon

        private void AddInfoSlideOut(RibbonPanel toThisRibbonPanel)
        {
            string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string ExecutingAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            toThisRibbonPanel.AddSlideOut();
            PushButtonData bInfo;
            switch (toThisRibbonPanel.Name)
            {
                case "KeyNote Editor":
                    bInfo = new PushButtonData("Info", "Info", ExecutingAssemblyPath, ExecutingAssemblyName + ".OpenKeyNoteManagerInHelp");
                    break;
                default:
                    bInfo = new PushButtonData("Info", "Info", ExecutingAssemblyPath, ExecutingAssemblyName + ".OpenKeyNoteManagerInHelp");
                    break;
            }
            bInfo.ToolTip = "This will open the help document regarding this tool.";
            bInfo.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".InfoLg.png");
            toThisRibbonPanel.AddItem(bInfo);
        }
        
        /// <summary>
        /// Load a new icon bitmap from embedded resources.
        /// For the BitmapImage, make sure you reference WindowsBase
        /// and PresentationCore, and import the System.Windows.Media.Imaging namespace. 
        /// </summary>
        BitmapImage NewBitmapImage(System.Reflection.Assembly a, string imageName)
        {
            Stream s = a.GetManifestResourceStream(imageName);
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = s;
            img.EndInit();
            return img;
        }
    }
}
