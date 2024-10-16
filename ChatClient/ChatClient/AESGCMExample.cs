using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public class AESGCMExample
    {
        public static byte[] Encrypt(byte[] plaintext, byte[] key, byte[] iv)
        {
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            KeyParameter keyParameter = new KeyParameter(key);
            AeadParameters parameter = new AeadParameters(keyParameter, 128, iv);            

            cipher.Init(true, parameter);
            byte[] output = new byte[cipher.GetOutputSize(plaintext.Length)];
            int legth = cipher.ProcessBytes(plaintext, 0, plaintext.Length, output, 0);
            cipher.DoFinal(output, legth);
            return output;
        }

        public static byte[] Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            KeyParameter keyParameter = new KeyParameter(key);
            AeadParameters parameter = new AeadParameters(keyParameter, 128, iv);
            
            cipher.Init(false, parameter);
            byte[] output = new byte[cipher.GetOutputSize(cipherText.Length)];
            int length = cipher.ProcessBytes(cipherText,0,cipherText.Length,output,0);
            cipher.DoFinal(output,length);
            return output;
        }
    
        
    }
}
