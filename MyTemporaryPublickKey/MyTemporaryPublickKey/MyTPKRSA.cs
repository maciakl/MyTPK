// MyTPKRSA (c) 2011 Lukasz Grzegorz Maciak

/*
* License: This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 3 of the License, or (at your
* option) any later version. This program is distributed in the hope that it
* will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General
* Public License for more details.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace MyTemporaryPublickKey
{
    class MyTPKRSA
    {

        RSACryptoServiceProvider RSA;
        AesCryptoServiceProvider AES;

        private string _KEY;
        private string _IV;

        public MyTPKRSA()
        {
            // this is the folder where we are going to store the symmetric keys
            string MyTPKFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "myTPK");

            if(!Directory.Exists(MyTPKFolder))
                Directory.CreateDirectory(MyTPKFolder);

            // paths to where we store symmetric keys
            _KEY = Path.Combine(MyTPKFolder, "KEY");
            _IV = Path.Combine(MyTPKFolder, "IV");

            getPersistentKey();
        }

        /**
         *      Get the assymetric key pair from the system container
         **/
        public void getAssymetricKeys()
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "MyTPK";

            RSA = new RSACryptoServiceProvider(2048, cspParams);
        }

        /**
         ** Get the both the symmetric and assymetric keys from where they are stored
         **/
        public void getPersistentKey()
        {
            getAssymetricKeys();

            try
            {
                getSymmetricKeys();
            }
            catch (CryptographicException) // the AES keys may have been created with an old key and never deleted
            {
                File.Delete(_KEY);
                File.Delete(_IV);
                newSymmetricKeys();
            }
            catch (FileNotFoundException) // files are not there, create new
            {
                newSymmetricKeys();
            }
        }


        /**
         *      Create a new symmetric key pair
         **/
        public void newSymmetricKeys()
        {
            getAssymetricKeys();
            AES = new AesCryptoServiceProvider();

            AES.GenerateKey(); // 32 
            AES.GenerateIV();  // 16

            byte[] key = AES.Key;
            byte[] IV = AES.IV;

            key = RSA.Encrypt(key, false);
            IV = RSA.Encrypt(IV, false);

            writeFile(_KEY, key);
            writeFile(_IV, IV);
        }

        /**
         *     Load the symmetric key from file system
         **/
        public void getSymmetricKeys()
        {
            AES = new AesCryptoServiceProvider();

            byte[] key = File.ReadAllBytes(_KEY);
            byte[] IV = File.ReadAllBytes(_IV);

            key = RSA.Decrypt(key, false);
            IV = RSA.Decrypt(IV, false);
        }

        /**
         *      Delete all the keys
         **/
        public void deletePersistentKey()
        {
            CspParameters cspParams = new CspParameters();
            cspParams.KeyContainerName = "MyTPK";

            RSA = new RSACryptoServiceProvider(cspParams);

            //Explicitly set the PersistKeyInCsp property to false
            //to delete the key entry in the container.
            RSA.PersistKeyInCsp = false;
            RSA.Clear();

            File.Delete(_KEY);
            File.Delete(_IV);
        }
            

        public string PublicKey
        {
            get { return getPublicKey(); }
        }

        public byte[] Encrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
           
            RSA.ImportParameters(RSAKeyInfo);
            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }

        public byte[] Decrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
          
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.ImportParameters(RSAKeyInfo);

                return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);

        }

        public byte [] readFile(string path)
        {
            byte[] buff = File.ReadAllBytes(path); 
            return buff;
        }

        public void writeFile(string path, byte[] data)
        {
            File.WriteAllBytes(path, data);
        }
        
        public void EncryptFile(string path, string key)
        {                        
    
            // get the symmetric key and IV, concatinate them
            // then append them to the beggining of the encrypted file
            byte [] buff = new byte[AES.Key.Length + AES.IV.Length]; //48
                        
            Buffer.BlockCopy(AES.Key, 0, buff, 0, AES.Key.Length);
            Buffer.BlockCopy(AES.IV, 0, buff, AES.Key.Length, AES.IV.Length);
                                    
            RSACryptoServiceProvider temprsa = new RSACryptoServiceProvider(2048);
            temprsa.FromXmlString(key);
                       
            buff = Encrypt(buff, temprsa.ExportParameters(false), false); // 256
            temprsa = null;

            byte[] data = readFile(path);

            ICryptoTransform encryptor = AES.CreateEncryptor();
            data = encryptor.TransformFinalBlock(data, 0, data.Length);

            byte[] output = new byte[buff.Length + data.Length];
            
            Buffer.BlockCopy(buff, 0, output, 0, buff.Length);
            Buffer.BlockCopy(data, 0, output, buff.Length, data.Length);

            writeFile(path + ".mytpk", output);
        }

        public void DecryptFile(string path)
        {
            byte[] buff = readFile(path);

            byte[] key = new byte[32];
            byte[] iv = new byte[16];

            byte[] temp = new byte[256]; // encrypted key
            byte[] encdata = new byte[buff.Length - 256];

            Buffer.BlockCopy(buff, 0, temp, 0, temp.Length);
            Buffer.BlockCopy(buff, 256, encdata, 0, encdata.Length);

            temp = Decrypt(temp, RSA.ExportParameters(true), false);
            
            Buffer.BlockCopy(temp, 0, key, 0, key.Length);
            Buffer.BlockCopy(temp, 32, iv, 0, iv.Length);

            

            AES.Key = key;
            AES.IV = iv;

            ICryptoTransform decryptor = AES.CreateDecryptor();
            byte [] data = decryptor.TransformFinalBlock(encdata, 0, encdata.Length);

            writeFile(path.Substring(0, path.Length-6), data);

        }


        private string getPublicKey()
        {
            return RSA.ToXmlString(false);
        }



    }
}
