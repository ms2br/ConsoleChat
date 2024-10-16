using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public class Encryption
    {
        byte[] Key { get; set; } = new Guid("12345678-90AB-CDEF-1234-567890ABCDEF").ToByteArray();
        byte[] Iv { get; set; } = new byte[12];

        public byte[] CombineEncryptMessage(string message)
        {
            byte[] encryptMessage = EncryptMessage(Encoding.UTF8.GetBytes(message));
            byte[] combineMessage = new byte[Iv.Length + encryptMessage.Length];
            Array.Copy(Iv, 0, combineMessage, 0, Iv.Length);
            Array.Copy(encryptMessage, 0, combineMessage, Iv.Length, encryptMessage.Length);
            return combineMessage;
        }

        public byte[] DistributedEncryptMessage(byte[] combineMessage)
        {
            Array.Copy(combineMessage, 0, Iv, 0, Iv.Length);
            byte[] cipherText = new byte[combineMessage.Length - 12];
            Array.Copy(combineMessage, Iv.Length, cipherText, 0, cipherText.Length);           
            return DecryptMessage(cipherText);
        }

        byte[] EncryptMessage(byte[] plainText)
        {
            Iv = SecureRandom.GetNextBytes(new SecureRandom(), 12);
            return AESGCMExample.Encrypt(plainText, Key, Iv);
        }

        byte[] DecryptMessage(byte[] ciphertext)
        => AESGCMExample.Decrypt(ciphertext, Key, Iv);
    }
}
