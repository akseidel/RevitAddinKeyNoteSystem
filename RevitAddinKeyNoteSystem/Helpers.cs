using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RevitAddinKeyNoteSystem
{
    internal static class Helpers
    {
        internal static void RunExplorerHere(string path)
        {
            Process.Start("Explorer.exe", path);
        }

        internal static void WebBrowserToHere(string target)
        {
            try
            {
                Process.Start(target);
            }
            catch
                (
                 System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }


        }

        internal static string MakeFileNameDateTimeRegexPattern(List<string> fnameExtList)
        {
            /// regex pattern matches 012345.01.pic at string end
            // pat_general_datetime = @"\d{6}\.\d{2}\.pic\Z";
            string pat_general_datetime = @"\d{6}\.\d{2}\.(";
            // string resStr = @"^.+\.(";
            foreach (string s in fnameExtList)
            {
                pat_general_datetime = pat_general_datetime + s + '|';
            }
            pat_general_datetime = pat_general_datetime.TrimEnd('|');
            pat_general_datetime = pat_general_datetime + @")\Z";
            return pat_general_datetime;
        }

        internal static string GiveMeBestPathOutOfThisPath(string proposedPath)
        {
            string[] words = proposedPath.Split('\\');
            string bestPath = String.Empty;
            foreach (string token in words)
            {
                string attempt = string.Empty;
                if (token.Equals(words[0]))
                {
                    attempt = token;
                }
                else
                {
                    attempt = String.Concat(bestPath, '\\', token);
                }
                if (Directory.Exists(attempt))
                {
                    bestPath = attempt;
                }
                else
                {
                    break;
                }
            }
            return bestPath;
        }

        internal static void SetTextToASelectedFolder(object sender, string msg, bool dotmode, string basePath)
        {
            TextBox tb = sender as TextBox;
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                SelectedPath = tb.Text
            };
            if (dotmode)
            {
                // if the dotmode folder does not exist then we want to drop back no
                // further than necessary woith the default folder. 
                string proposedPath = CombineIntoPath(basePath, tb.Text);
                folderDialog.SelectedPath = GiveMeBestPathOutOfThisPath(proposedPath);
                if (!Directory.Exists(folderDialog.SelectedPath))
                {
                    folderDialog.SelectedPath = basePath; // for the time being
                }
            }
            folderDialog.Description = msg;
            var result = folderDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var folder = folderDialog.SelectedPath;
                    string fp = EnsurePathStringEndsInBackSlash(folder);
                    if (!dotmode)
                    {
                        tb.Text = fp;
                    }
                    else
                    {
                        string t = fp.ReplaceString(basePath, ".\\", StringComparison.CurrentCultureIgnoreCase);
                        tb.Text = t;
                    }
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:

                    break;
            }
        }

        internal static void SetTextToASelectedFile(object sender, string msg, bool fullpath)
        {
            TextBox tb = sender as TextBox;
            var filepickerDialog = new System.Windows.Forms.OpenFileDialog
            {
                Title = msg
            };

            var result = filepickerDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:

                    string thepick = filepickerDialog.FileName;

                    if (!fullpath)
                    {
                        thepick = Path.GetFileName(thepick);
                    }

                    tb.Text = thepick;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }

        internal static string CombineIntoPath(string partA_RootPath, string partB_PartialPathWithDot, string partC_OptionalFileName = "")
        {
            try
            {
                string fullPath = Path.Combine(partA_RootPath, partB_PartialPathWithDot);
                fullPath = fullPath.Replace(".\\", "") + partC_OptionalFileName;
                return fullPath;
            }
            catch (ArgumentException e)
            {
                string msg = "Part of this path name " + partB_PartialPathWithDot + partC_OptionalFileName + " is illegal. You need to correct it";
                msg = msg + "\n\n" + e.Message;
                FormMsgWPF explain = new FormMsgWPF(null, 3);
                explain.SetMsg(msg, "The Path Has Illegal An Character");
                explain.ShowDialog();
            }
            return string.Empty;
        }

        internal static string EnsurePathStringEndsInBackSlash(string path)
        {
            if (!path.EndsWith("\\", StringComparison.CurrentCultureIgnoreCase))
            {
                path = path + "\\";
            }
            return path;
        }

        internal static void EndsInBackSlash(object sender)
        {
            TextBox tb = sender as TextBox;
            tb.Text = EnsurePathStringEndsInBackSlash(tb.Text);
            tb.CaretIndex = tb.Text.Length;
        }
      
        // colors a filepath textbox as to filepath's existance
        // Also returns as bool for path existance. False = path does not exist
        internal static bool MarkTextBoxForPath(TextBox theTextBox, string basePath, bool dotMode = false)
        {
            // if (path == null) { return; }
            if (theTextBox == null) { return false; }
            string path = theTextBox.Text;
            if (dotMode) { path = Helpers.CombineIntoPath(basePath, path); }
            if (Directory.Exists(path))
            {
                theTextBox.Foreground = Brushes.Black;
                return true;
            }
            else
            {
                theTextBox.Foreground = Brushes.Red;
                return false;
            }
        }

        // colors a filename textbox as to filename's existance or allowed characters
        internal static void MarkTextBoxForFile(TextBox theTextBox, string basePath, string subpath)
        {
            if (subpath == null) { return; }
            if (theTextBox == null) { return; }
            subpath = CombineIntoPath(basePath, subpath);
            subpath = CombineIntoPath(subpath, theTextBox.Text);
            if (File.Exists(subpath))
            {
                theTextBox.Foreground = Brushes.Black;
            }
            else
            {
                theTextBox.Foreground = Brushes.Red;
            }
        }

        // colors a filename textbox as to filename's existance or allowed characters
        internal static void MarkLabelForFile(Label theLabel, string fullPath, bool sense)
        {
            if (fullPath == null) { return; }
            if (theLabel == null) { return; }
            if (!IsValidWindowsFileName(Path.GetFileName(fullPath)))
            {
                theLabel.Foreground = Brushes.Red;
            }
            if (File.Exists(fullPath))
            {
                if (sense)
                {
                    theLabel.Foreground = Brushes.Black;
                }
                else
                {
                    theLabel.Foreground = Brushes.Red;
                }
            }
            else
            {
                if (sense)
                {
                    theLabel.Foreground = Brushes.Red;
                }
                else
                {
                    theLabel.Foreground = Brushes.Black;
                }
            }
        }
       
        internal static void CreateThisPath(string newPathToCreate)
        {
            try
            {
                Directory.CreateDirectory(@newPathToCreate);
            }
            catch (Exception er)
            {
                string ttl = "Create Directory Error";
                string msg = "Unable to create the paths in " + newPathToCreate + "\n\n" + er.Message;
                FormMsgWPF explain = new FormMsgWPF(null, 3);
                explain.SetMsg(msg, ttl);
                explain.ShowDialog();
            }
        }

        // reports if text in textbox would be ok for a windows filename
        internal static void SniffTextBoxToBeAValidFileName(TextBox theTextBox)
        {
            if (theTextBox == null) { return; }
            string fn = theTextBox.Text;
            if (fn.Trim() == string.Empty) { return; }
            if (!IsValidWindowsFileName(fn))
            {
                string msg = "Windows will not allow \n\n" + fn + "\n\n to be a file name.";
                FormMsgWPF explain = new FormMsgWPF(null, 3);
                explain.SetMsg(msg, "By The Way. Not A Valid Name");
                explain.ShowDialog();
            }
        }

        /// <summary>
        /// Only works properly on file names. Do not use for full path names.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        internal static bool IsValidWindowsFileName(string expression)
        {
            // https://stackoverflow.com/questions/62771/how-do-i-check-if-a-given-string-is-a-legal-valid-file-name-under-windows
            string sPattern = @"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:\"";|/]+$";
            return (Regex.IsMatch(expression, sPattern, RegexOptions.CultureInvariant));
        }
        
        internal static bool ExploreFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            //Clean up file path so it can be navigated OK
            filePath = Path.GetFullPath(filePath);
            Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
            return true;
        }
        
    }

    internal static class StringExtensions
    {
        internal static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);
            return (value.Length <= maxLength
                    ? value
                    : value.Substring(0, maxLength)
                    );
        }

        internal static string ReplaceString(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }
    }
    
    /// <summary>
    /// Used to convert system drawing colors to WPF brush
    /// </summary>
    internal static class ColorExt
    {
        internal static System.Windows.Media.Brush ToBrush(System.Drawing.Color color)
        {
            {
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
            }
        }
    }
}
