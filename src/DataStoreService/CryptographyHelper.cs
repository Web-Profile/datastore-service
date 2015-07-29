using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Xml;
using System.IO;
using System.Text;

namespace DataStoreService
{
    public class CryptographyHelper
    {

        /// <summary>
        /// Get Private key from RSACryptoServiceProvider
        /// </summary>
        /// <param name="RSA"></param>
        /// <returns></returns>
        public static string GetPrivateKeyString(RSACryptoServiceProvider RSA)
        {
            if (RSA == null)
            {
                throw new ArgumentNullException("RSA is null");
            }
            return RSA.ToXmlString(true);
        }

        /// <summary>
        /// Get Public key from RSACryptoServiceProvider
        /// </summary>
        /// <param name="RSA"></param>
        /// <returns></returns>
        public static string GetPublicKeyToString(RSACryptoServiceProvider RSA)
        {
            if (RSA == null)
            {
                throw new ArgumentNullException("RSA is null");
            }
            return RSA.ToXmlString(true);
        }

        /// <summary>
        /// Try to convert RSACryptoServiceProvider Public key from string to RSAParameters
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="rsaKey"></param>
        /// <returns></returns>
        public static bool TryConvertPublicKeyToRSAParameters(string publicKey, out RSAParameters rsaKey)
        {
            bool canFind = false;
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(publicKey);
                rsaKey = RSA.ExportParameters(false);
                canFind = true;
            }
            catch
            {
                rsaKey = new RSAParameters();  //Hack because RSAParameters can't be null
            }
            return canFind;
        }

        /// <summary>
        /// Try to convert RSACryptoServiceProvider private key from string to RSAParameters
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="rsaKey"></param>
        /// <returns></returns>
        public static bool TryConvertPrivateKeyRSAParameters(string privateKey, out RSAParameters rsaKey)
        {
            bool canFind = false;
            try
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.FromXmlString(privateKey);
                rsaKey = RSA.ExportParameters(true);
                canFind = true;
            }
            catch
            {
                rsaKey = new RSAParameters();  //Hack because RSAParameters can't be null
            }
            return canFind;
        }

        /// <summary>
        /// Encrypt data using public key 
        /// </summary>
        /// <param name="DataToEncrypt"></param>
        /// <param name="RSAKeyInfo"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding = false)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider. 
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs 
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.   
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException   
            //to the console. 
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }

        }

        /// <summary>
        /// Decrip data using private key
        /// </summary>
        /// <param name="DataToDecrypt"> DataToDecrypt</param>
        /// <param name="RSAKeyInfo"></param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding = false)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider. 
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs 
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.     
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException   
            //to the console. 
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }

        }
    }
}