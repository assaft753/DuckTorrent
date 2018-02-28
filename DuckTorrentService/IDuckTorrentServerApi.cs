using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DuckTorrentService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDuckTorrentServerApi" in both code and config file together.
    [ServiceContract]
    public interface IDuckTorrentServerApi
    {
        [OperationContract]
        string SignIn(string userByXML);
        [OperationContract]
        string SearchFile(string fileByXML);
        [OperationContract]
        string SignOut(string userByXML);
        [OperationContract]
        string CheckUserExists(string userByXML);
        [OperationContract]
        string RefreshFiles(string userByXML);
        [OperationContract]
        Boolean CheckEnable(string userByXML);

    }
}
