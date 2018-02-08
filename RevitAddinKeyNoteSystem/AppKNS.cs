#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
#endregion

namespace RevitAddinKeyNoteSystem
{
    class AppKNS : IExternalApplication
    {
        static string _path = typeof(Application).Assembly.Location;
        /// Singleton external application class instance.
        internal static AppKNS _app = null;
        /// Provide access to singleton class instance.
        public static AppKNS Instance {
            get { return _app; }
        }
        ///// Provide access to the radio button state
        //internal static string _pb_state = String.Empty;
        //public static string PB_STATE {
        //    get { return _pb_state; }
        //}
        ///// Provide access to the offset state
        //internal static XYZ _pOffSet = new XYZ(1, 1, 0);
        //public static XYZ POFFSET {
        //    get { return _pOffSet; }
        //}
        public Result OnStartup(UIControlledApplication a)
        {
            _app = this;
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
