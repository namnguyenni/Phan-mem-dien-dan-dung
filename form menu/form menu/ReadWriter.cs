using System;
using System.IO;
using System.Management;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace form_menu
{
    class ReadWriter
    {

        private string ID_PC;


        public string SecurityLienceKeyMD5(string origin)
        {
            MD5 md = MD5.Create();
            byte[] inputbyte = System.Text.Encoding.ASCII.GetBytes(origin);
            byte[] hash = md.ComputeHash(inputbyte);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log exception here
                
            }

            return objectOut;
        }
        /////


        /// <summary>
        /// Get infomation hardware
        /// </summary>
        /// <param name="Key"></param>
        private void GetInfoHardware(string Key)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
        "select * from " + Key);
        }

        /// <summary>
        /// get info main board
        /// </summary>
        /// <returns></returns>
        private string GetMainBoardID()
        {
            string mainboardID = "";
            try
            {
                var managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
                foreach (var managementObject in managementObjectSearcher.Get())
                {
                    mainboardID += managementObject["SerialNumber"].ToString();
                }
            }
            catch (Exception)
            {
                mainboardID = "KHONGTIMTHAYMAINBOARDID";
                throw;
            }

            return mainboardID;
        }
        /// <summary>
        /// create id pc
        /// </summary>
        public string GetIDPC()
        {
            ID_PC = "ELECTRICSIMULATION" + GetCPUID() + GetMainBoardID();
            return ID_PC;
        }

        /// <summary>
        /// get cpu id
        /// </summary>
        /// <returns></returns>
        private string GetCPUID()
        {
            string cpuID = string.Empty;
            try
            {                
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (cpuID == "")
                    {
                        //Remark gets only the first CPU ID
                        cpuID = mo.Properties["processorID"].Value.ToString();

                    }
                }
            }
            catch (Exception)
            {
                cpuID = "KHONGTIMTHAYIDCPU";
                throw;
            }
            return cpuID;
        }

        /// <summary>
        /// save lience key
        /// </summary>
        public void SaveLienceKey()
        {
            if (!Directory.Exists("C:\\temp"))
            {
                Directory.CreateDirectory("C:\\temp");
            }


            LienceKey LIENCE = new LienceKey();
            LIENCE.SetDate();
            LIENCE.date = SecurityLienceKeyMD5(LIENCE.date);
            LIENCE.SetIDPC(SecurityLienceKeyMD5(GetIDPC()));
            try {
                string path = "C:\\temp\\SECURITY.e";
                SerializeObject<LienceKey>(LIENCE, path);
            }
            catch(Exception) { }

        }

        public bool CheckedDate()
        {
            return true;
        }

        /// <summary>
        /// check lience key
        /// </summary>
        /// <returns></returns>
        public bool CheckedLIENCEKEY()
        {
            try
            {
                string path = "C:\\temp\\SECURITY.e";
                var KEY = DeSerializeObject<LienceKey>(path);
                GetIDPC();
                if (SecurityLienceKeyMD5(ID_PC) != KEY.GetIDPC())
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
            return true;


        }


    }
}
