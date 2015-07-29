using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Xml;

namespace DataStoreService.Services
{
    /// <summary>
    /// Data Envelope with Signature
    /// </summary>
    public class SignedEnvelope
    {
        public string AgentId { get; set; }

        public byte[] Signature { get; set; }
        
        public string Data { get;  set; }

    }

}