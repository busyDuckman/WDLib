/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WD_toolbox.Files;

namespace WD_toolbox.AplicationFramework
    {
    public class WDDocumentApp <DOCTYPE> : WDApp
    where DOCTYPE : WDDocument
        {
        DOCTYPE _document=null;
        public DOCTYPE Document
            {
            get { return _document; }
            }
        
        public readonly UndoHandler<DOCTYPE> Undo;
        
        
        public delegate void DocumentUpdateDelegate();
        DocumentUpdateDelegate onUpdate=null;
        public delegate DOCTYPE NewFileDelegate();
        NewFileDelegate onNewFile=null;
        public delegate DOCTYPE OpenFileDelegate(string file);
        OpenFileDelegate onOpenFile=null;
        public delegate DOCTYPE SaveFileDelegate(string file);
        SaveFileDelegate onSaveFile=null;

        void flagDocumentAltered()
            {
            _documentLastVersionIsSaved = false;
            }
        
        bool _documentLastVersionIsSaved;

        public bool DocumentLastVersionIsSaved
            {
            get { return _documentLastVersionIsSaved; }
            }

        string _doumentFilePath;

        public string DoumentFilePath
            {
            get { return _doumentFilePath ?? ""; }
            }
        
        public string DocumentName
            {
            get
                {
                return Files.Files.getName(DoumentFilePath);
                }
            }

        public string fileDialogExtensionFilter = "data files (*.dat)|*.dat|All files (*.*)|*.*";
        
        public bool isFileOpen()
            {
            return (Document != null);
            }
        
        #region constructors
        public WDDocumentApp(NewFileDelegate onNewFile, OpenFileDelegate onOpenFile, SaveFileDelegate onSaveFile, DocumentUpdateDelegate onUpdate)
            {
            System.Diagnostics.Debug.Assert(onNewFile != null);
            this.onNewFile = onNewFile;
            this.onOpenFile = onOpenFile;
            this.onSaveFile = onSaveFile;
            this.onUpdate = onUpdate;
            Undo = new UndoHandler<DOCTYPE>(delegate (DOCTYPE newDoc) {this._document = newDoc;});
            }

        public WDDocumentApp(NewFileDelegate onNewFile)
            {
            System.Diagnostics.Debug.Assert(onNewFile != null);
            this.onNewFile = onNewFile;
            }
        #endregion
        
        #region file menu
        public bool doFileOpen()
            {
            if(closeFileOkToContinueDialoge())
                {
                OpenFileDialog od = new OpenFileDialog();
                od.Filter = fileDialogExtensionFilter;
                od.CheckFileExists = true;
                if (od.ShowDialog() == DialogResult.OK)
                    {
                    try
                        {
                        if(onOpenFile == null) 
                            {
                            _document = SerialisationHelper<DOCTYPE>.open(od.FileName, serializationFormats.binary);
                            }
                        else
                            {
                            _document = onOpenFile(od.FileName);
                            }
                        _documentLastVersionIsSaved = true;
                        _doumentFilePath = od.FileName;
                        onUpdate();
                        return true;
                        }
                    catch(Exception ex)
                        {
                        MessageBox.Show(string.Format("The file {0} could not be opened becasue the following error occured: {1}", od.FileName, ex.Message), "Could not open file.");
                        onUpdate();
                        }
                    }
                }
              return false;
            }
        
        
        /// <summary>
        /// Generic "File ... Save" dialoges and operations
        /// </summary>
        /// <returns> Returns false on a user canceling a "save as" dialoge.</returns>
        public bool doFileSave()
            {
            if(DoumentFilePath == "")
                {
                return doFileSaveAs();
                }
            else
                {
                saveFile(DoumentFilePath);
                onUpdate();
                return true;
                }
            }

        /// <summary>
        /// Generic "File ... Save As" dialoges and operations
        /// </summary>
        /// <returns> Returns false on a user canceling a "save as" dialoge.</returns>
        public bool doFileSaveAs()
            {
            SaveFileDialog nd = new SaveFileDialog();
            nd.Filter = fileDialogExtensionFilter;
            nd.Title = "New file";
            nd.AddExtension = true;
            if (nd.ShowDialog() == DialogResult.OK)
                {
                if (!System.IO.File.Exists(nd.FileName))
                    {
                    saveFile(nd.FileName);
                    }
                else
                    {
                    MessageBox.Show("error file already exists");
                    return false;
                    }
                }
            else
                {
                return false;
                }

            onUpdate();
            return true;
            }
        
        public void doDoFileNew()
            {
            closeFileOkToContinueDialoge();
            _document = onNewFile();
            onUpdate();
            }
            
        public void doFileClose()
            {
            closeFileOkToContinueDialoge();
            }
        
        private bool closeFileOkToContinueDialoge()
            {
            if (Document == null)
                {
                return true;
                }

            if(DocumentLastVersionIsSaved)
                {
                if(MessageBox.Show("Confirm", "Are you sure you wish to create a new file?.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                    _document = null;
                    GC.Collect();
                    onUpdate();
                    return true;
                    }
                else
                    {
                    return false;
                    }
                }
            
            DialogResult r = MessageBox.Show("Save changes to current file before continuing?", "File currently open.", MessageBoxButtons.YesNoCancel);
            switch (r)
                {
                case DialogResult.Cancel:
                    return false;
                case DialogResult.No:
                    _document = null;
                    GC.Collect();
                    onUpdate();
                    return true;
                case DialogResult.Yes:
                    if(doFileSave())
                        {
                        _document = null;
                        onUpdate();
                        return true;
                        }
                    else
                        {
                        MessageBox.Show("File not saved operation abborted.");
                        return false;
                        }
                default:
                    return false;
                }
            }
            
        private void saveFile(String fileName)
            {
            try
                {
                if(onSaveFile == null)
                    {
                    SerialisationHelper<DOCTYPE>.save(fileName, Document, serializationFormats.binary);
                    }
                else
                    {
                    onSaveFile(fileName);
                    }
                _documentLastVersionIsSaved = true;
                _doumentFilePath = fileName;
                }
            catch(Exception ex)
                {
                MessageBox.Show(string.Format("The file {0} could not be saved because the following error occured: {1}",fileName, ex.Message), "Error occored saving file.");
                }
            }
        #endregion
        
        #region conversion
        public static explicit operator DOCTYPE(WDDocumentApp<DOCTYPE> obj)
            {
            return obj.Document;
            }
        #endregion
        }
    }
