using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Winger.Utils
{
    public class SaveLoadUtils
    {
        // TODO: implement save/load utils
        #region Singleton
        private static SaveLoadUtils instance;

        private SaveLoadUtils() { }

        public static SaveLoadUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SaveLoadUtils();
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// The last file save location
        /// </summary>
        public string LastFileSave = null;

        /// <summary>
        /// Saves the given data to the last file save location
        /// </summary>
        /// <returns>the location of the save file</returns>
        public string Save(string data)
        {
            if (LastFileSave == null)
            {
                return null;
            }
            return SaveAs(LastFileSave, data);
        }

        /// <summary>
        /// Saves a file to the file path with the given data
        /// </summary>
        /// <param name="file">the file path with file name and extension</param>
        /// <param name="data">the data to save to the file</param>
        /// <returns>the location of the save file</returns>
        public string SaveAs(string file, string data)
        {
            LastFileSave = file;
            return null;
        }

        /// <summary>
        /// Loads up a file to a string
        /// </summary>
        /// <param name="file">the location of the file to load</param>
        /// <returns>the file data as a string</returns>
        public string Load(string file)
        {
            return FileUtils.FileToString(file);
        }

        /// <summary>
        /// Opens up the save as dialog
        /// </summary>
        /// <param name="data">the data to save to the file</param>
        /// <returns>the location of the file saved</returns>
        public string SaveAsDialog(string data)
        {
            // TODO: on save as dialog save button click, return SaveAs(fileLocation, data)
            return null;
        }

        /// <summary>
        /// Opens up the load file dialog
        /// </summary>
        /// <returns>the string data from the file picked in the dialog</returns>
        public string LoadDialog()
        {
            // TODO: on loaddialog load button click, return Load(fileLocation);
            return null;
        }
    }
}
