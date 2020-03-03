using System;

namespace Iserv.Niis.Model.Models.Dictionaries.Ipc
{
    public class IpcCodeDetail
    {
        private string _ipcCode;
        public IpcCodeDetail(string ipcCode)
        {
            _ipcCode = ipcCode;
        }
        public string Section
        {
            get
            {
                return _ipcCode.Length >= 1 ? _ipcCode.Substring(0, 1) : string.Empty;
            }
        }
        public string Class
        {
            get
            {
                return _ipcCode.Length >= 3 ? _ipcCode.Substring(0, 3) : string.Empty;
            }
        }
        public string SubClass
        {
            get
            {
                return _ipcCode.Length >= 4 ? _ipcCode.Substring(0, 4) : string.Empty;
            }
        }
    }
}
