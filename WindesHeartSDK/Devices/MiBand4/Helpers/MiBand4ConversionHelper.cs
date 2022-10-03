using System;
using System.Security.Cryptography;
using WindesHeartSDK.Helpers;

namespace WindesHeartSDK.Devices.MiBand4Device.Helpers
{
    public static class MiBand4ConversionHelper
    {

        public static byte[] CreateKey(byte[] value)
        {
            byte[] bytes = { 0x03, 0x00 };
            //El primer MiBand
            byte[] secretKey = { 0x04, 0xa7, 0xd1, 0xe2, 0xee, 0x81, 0x60, 0xb0, 0x95, 0x56, 0xc2, 0xc8, 0xea, 0x60, 0x76, 0x2a }; 
            //El segundo MiBand
            //byte[] secretKey = { 0x92, 0x29, 0xc9, 0xe3, 0x42, 0x5f, 0x8f, 0xd0, 0x7f, 0x9a, 0xc4, 0x68, 0x76, 0x6b, 0x45, 0x81 }; 
            value = ConversionHelper.CopyOfRange(value, 3, 19);
            byte[] buffer = EncryptBuff(secretKey, value);
            byte[] endBytes = new byte[18];
            Buffer.BlockCopy(bytes, 0, endBytes, 0, 2);
            Buffer.BlockCopy(buffer, 0, endBytes, 2, 16);
            return endBytes;
        }

        public static byte[] EncryptBuff(byte[] sessionKey, byte[] buffer)
        {
            AesManaged myAes = new AesManaged();

            myAes.Mode = CipherMode.ECB;
            myAes.Key = sessionKey;
            myAes.Padding = PaddingMode.None;

            ICryptoTransform encryptor = myAes.CreateEncryptor();
            return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }


    }
}
