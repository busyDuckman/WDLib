/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WD_toolbox.AplicationFramework
{
    /// <summary>
    /// Implement this interface and the assosiated extension class creates menu event handelers (eg save/open dialogs) for you.
    /// </summary>
    public interface IMenuSaveOpenNewCloseExit
    {
        /// <summary>
        /// The full path of the current file
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// True if the file has been modified since it was last saved.
        /// </summary>
        bool FileModified { get; set; }

        /// <summary>
        /// Open s a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        void OpenFile(string file);

        /// <summary>
        /// Overrides / Saves a file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        void SaveFile(string file);

        bool NewFile();

        void CloseFile();

        /// <summary>
        /// A set of file formats supported by the application.
        /// </summary>
        FileFormat[] SupportedSaveFormats { get; }
        FileFormat[] SupportedOpenFormats { get; }
    }

    public static class IMenuSaveOpenNewCloseExitExtension 
    {
        /// <summary>
        /// Call this from the save (as) menu and you are done.
        /// </summary>
        /// <param name="saveAs">Save vs. Save As functionality.</param>
        /// <returns></returns>
        public static bool DoFileSave(this IMenuSaveOpenNewCloseExit app, bool saveAs)
        {
            if ((app.FileName == null) || saveAs)
            {
                //save as
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = FileFormat.MakeFileDialogeString(app.SupportedSaveFormats, false, "");
                sfd.OverwritePrompt = true;
                if(sfd.ShowDialog(app as IWin32Window) == DialogResult.OK)
                {
                    string fileName = sfd.FileName;
                    //save
                    return applicationSave(app, fileName);
                }
                return false;
            }
            else
            {
                //save
                return applicationSave(app, app.FileName);
            }
        }

        /// <summary>
        /// Call this from the Open menu and you are done.
        /// </summary>
        public static bool DoFileOpen(this IMenuSaveOpenNewCloseExit app)
        {
            //save existing file
            if(!saveExistingFileBeforeNewOrOpen(app))
            {
                return false;
            }

            //open new file
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = FileFormat.MakeFileDialogeString(app.SupportedOpenFormats, false, "");
            ofd.CheckFileExists = true;
            if(ofd.ShowDialog(app as IWin32Window) == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                try
                {
                    app.OpenFile(fileName);
                    app.FileName = fileName;
                    app.FileModified = false;
                }
                catch (Exception ex)
                {
                    WDAppLog.logException(ErrorLevel.Error, ex);
                    showErrorDialoge(app, "Could not open file ({0}): {1}.", app.FileName, ex.Message);
                    return false;
                }
                app.FileModified = false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Call this from the Close menu and you are done.
        /// </summary>
        public static bool DoFileClose(this IMenuSaveOpenNewCloseExit app)
        {
            //save existing file
            if (!saveExistingFileBeforeNewOrOpen(app))
            {
                return false;
            }

            try
            {
                app.CloseFile();
                app.FileModified = false;
                app.FileName = null;
                return true;
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.Error, ex);
                showErrorDialoge(app, "Could create new file ({0}): {1}.", app.FileName, ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Call this from the New menu and you are done.
        /// </summary>
        public static bool DoFileNew(this IMenuSaveOpenNewCloseExit app)
        {
            //save existing file
            if (!saveExistingFileBeforeNewOrOpen(app))
            {
                return false;
            }

            try
            {
                if (app.NewFile())
                {
                    app.FileModified = true;
                    app.FileName = null;
                    return true;
                }

                return false; // a new file was not created for some reason (maybe the user pressed cancel on a dialogue).
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.Error, ex);
                showErrorDialoge(app, "Could create new file ({0}): {1}.", app.FileName, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Call this from the Exit menu and you are done.
        /// </summary>
        public static bool DoFileExit(this IMenuSaveOpenNewCloseExit app)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //save existing file
                if (!saveExistingFileBeforeNewOrOpen(app))
                {
                    return false;
                }

                if (app is Form)
                {
                    ((Form)app).Close();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void showErrorDialoge(IMenuSaveOpenNewCloseExit app, string message, params object[] args)
        {
            string text = string.Format(message, args);
            MessageBox.Show((app as IWin32Window), text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        private static bool applicationSave(IMenuSaveOpenNewCloseExit app, string path)
        {
            string fileName = path;
            try
            {
                app.SaveFile(fileName);
                app.FileModified = false;
                app.FileName = fileName;
                return true;
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.Error, ex);
                showErrorDialoge(app, "Could not save file ({0}): {1}.", path, ex.Message);
                return false;
            }
        }

        // true if no reason to stop with open or new
        private static bool saveExistingFileBeforeNewOrOpen(IMenuSaveOpenNewCloseExit app)
        {
            if(!app.FileModified)
            {
                return true;
            }

            if (MessageBox.Show((app as IWin32Window),
                    "Save existing file.",
                    "Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                    return DoFileSave(app, false);
            }
            return true;
        }


    }
}
