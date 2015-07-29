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
        /// Decript data using private key
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

        /// <summary>
        /// Hash the data and generate signature
        /// </summary>
        /// <param name="dataToSign"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] HashAndSignBytes(byte[] dataToSign, RSAParameters key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the  
                // key from RSAParameters.  
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.ImportParameters(key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider 
                // to specify the use of SHA1 for hashing. 
                return RSA.SignData(dataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }


        /// <summary>
        /// Hash the data and generate signature
        /// </summary>
        /// <param name="dataToSign"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HashAndSignString(string dataToSign, RSAParameters key)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] signatureBytes = HashAndSignBytes(ByteConverter.GetBytes(dataToSign), key);

            return ByteConverter.GetString(signatureBytes);

        }

        /// <summary>
        /// Verify the data and signature
        /// </summary>
        /// <param name="dataToVerify"></param>
        /// <param name="signedData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool VerifySignedData(byte[] dataToVerify, byte[] signedData, RSAParameters key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the  
                // key from RSAParameters.
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSA.ImportParameters(key);

                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider 
                // to specify the use of SHA1 for hashing. 
                return RSA.VerifyData(dataToVerify, new SHA1CryptoServiceProvider(), signedData);

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        /// <summary>
        /// Verify the data and signature
        /// </summary>
        /// <param name="dataToVerify"></param>
        /// <param name="signedData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool VerifySignedData(string dataToVerify, byte[] signedData, string key)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            RSAParameters rsaParameters;
            if (CryptographyHelper.TryConvertPublicKeyToRSAParameters(key, out rsaParameters) == false)
            {
                return false;
            }
            return VerifySignedData(ByteConverter.GetBytes(dataToVerify), signedData, rsaParameters);

        }
    }
}