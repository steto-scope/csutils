using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csutils.Cryptography
{
    /// <summary>
    /// Algorithm to obfuscate text, especially filenames. 
    /// Any character that isn't included in the specified alphabet (either using a custom alphabet or a predefined range) is ignored.
    /// The algorithm works in CBC-mode. 
    /// </summary>
    public sealed class Shenanigans
    {
        private string a;
        private int[] box;

        /// <summary>
        /// New Instance of Shenanigans.
        /// </summary>
        /// <param name="preset">Pick a predefined range or Custom</param>
        /// <param name="alphabet">If Custom range, alphabet is used. alphabet has to contain all characters that should be obfuscated (once!)</param>
        /// <param name="sbox">If Custom range: array of length alphabet that contains the numbers [0,alphabet.Length) in a random order. No duplicates allowed, it has to be bijective. </param>
        public Shenanigans(AlphabetPreset preset, string alphabet = null, int[] sbox = null)
        {
            switch (preset)
            {
                case AlphabetPreset.POSIXFullyPortableFilenames:
                    a = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789._";
                    box = new int[] { 35, 52, 29, 61, 34, 20, 33, 30, 31, 24, 7, 60, 59, 1, 25, 26, 48, 55, 17, 14, 50, 8, 53, 57, 10, 6, 37, 22, 40, 23, 5, 15, 13, 28, 43, 63, 45, 12, 47, 62, 38, 3, 19, 42, 9, 21, 49, 58, 36, 56, 44, 41, 39, 11, 4, 51, 46, 18, 27, 2, 0, 32, 16, 54 };
                    break;
                case AlphabetPreset.Text:
                    a = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþ";
                    box = new int[] { 194, 149, 148, 121, 15, 115, 195, 47, 84, 73, 52, 176, 55, 77, 16, 92, 125, 97, 207, 174, 112, 20, 203, 131, 162, 141, 190, 82, 96, 36, 165, 63, 57, 0, 191, 186, 75, 58, 100, 123, 67, 2, 98, 150, 86, 35, 72, 202, 184, 106, 163, 25, 66, 175, 108, 179, 214, 183, 28, 19, 99, 8, 173, 213, 138, 27, 124, 81, 164, 94, 69, 147, 116, 188, 80, 201, 87, 22, 113, 85, 13, 134, 42, 182, 26, 111, 135, 101, 139, 153, 105, 44, 64, 127, 29, 130, 192, 104, 40, 204, 132, 10, 90, 210, 103, 158, 146, 102, 4, 38, 118, 171, 208, 211, 152, 59, 212, 41, 193, 189, 205, 168, 155, 32, 185, 23, 107, 70, 24, 142, 60, 88, 93, 3, 30, 156, 39, 187, 199, 180, 6, 159, 89, 209, 133, 181, 145, 78, 151, 49, 83, 50, 197, 48, 95, 117, 56, 5, 160, 74, 76, 46, 172, 12, 9, 17, 170, 1, 31, 68, 11, 177, 79, 7, 128, 51, 91, 196, 114, 166, 65, 61, 54, 140, 109, 21, 154, 33, 144, 34, 129, 143, 71, 167, 37, 126, 137, 120, 178, 200, 53, 18, 206, 122, 45, 136, 215, 198, 14, 119, 157, 62, 110, 43, 161, 169 };
                    break;
                default:
                case AlphabetPreset.NTFS:
                    a = @"!#$%&'()+,-.0123456789;=@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþ";
                    box = new int[] { 137, 44, 98, 64, 14, 67, 148, 68, 188, 182, 151, 25, 61, 94, 109, 22, 206, 101, 191, 126, 181, 11, 80, 83, 116, 5, 154, 153, 77, 205, 0, 73, 85, 149, 79, 169, 35, 121, 76, 177, 81, 104, 196, 32, 136, 34, 62, 99, 143, 86, 42, 90, 38, 36, 60, 193, 6, 19, 112, 50, 37, 167, 155, 103, 82, 189, 51, 9, 185, 157, 102, 144, 28, 168, 176, 197, 175, 74, 45, 161, 7, 43, 202, 159, 18, 24, 97, 15, 8, 158, 39, 12, 199, 165, 184, 122, 171, 2, 13, 54, 179, 119, 106, 183, 127, 152, 131, 17, 48, 46, 1, 55, 52, 140, 134, 174, 117, 110, 187, 180, 41, 192, 47, 172, 88, 166, 164, 124, 160, 113, 138, 33, 59, 21, 53, 87, 49, 190, 135, 133, 29, 93, 108, 89, 178, 111, 65, 66, 10, 16, 78, 123, 70, 162, 200, 20, 71, 26, 107, 96, 129, 128, 4, 173, 63, 92, 198, 91, 40, 114, 201, 75, 195, 23, 139, 30, 72, 95, 57, 204, 145, 147, 56, 31, 84, 141, 150, 105, 120, 27, 118, 186, 69, 100, 146, 125, 115, 203, 130, 58, 156, 194, 142, 132, 3, 170, 163 };
                    break;
                case AlphabetPreset.Custom:
                    if (alphabet == null || sbox == null || alphabet.Length < 1 || sbox.Length < 1)
                        throw new ArgumentException("alphabet and sbox are required");
                    if (alphabet.Length != sbox.Length)
                        throw new ArgumentException("Length of alphabet unequal to sbox");
                    if (sbox.Distinct().Count() != sbox.Length || sbox.Min() < 0 || sbox.Max() >= alphabet.Length)
                        throw new ArgumentException("sbox has to be an array of range [0,alphabet.Length) which contains a permutation of the indexes");
                    box = sbox;
                    a = alphabet;
                    break;
            }
        }

        /// <summary>
        /// Makes a string unreadable. 
        /// </summary>
        /// <param name="text">text to obfuscate</param>
        /// <param name="seed">initial value (like a password). if 0, text.Length will be used</param>
        /// <returns></returns>
        public string Obfuscate(string text, uint seed = 0)
        {
            if (text == null)
                return null;

            int len = text.Length;
            int[] data = new int[len];

            //translate to intermediate format
            for (int i = 0; i < len; i++)
                data[i] = a.IndexOf(text[i]);

            //obfuscate
            int last = seed > 0 ? (int)seed : len;
            for (int i = 0; i < len; i++)
            {
                if (data[i] >= 0)
                {
                    data[i] = (data[i] + (last % a.Length)) % a.Length;
                    last += a.IndexOf(text[i]);
                    data[i] = box[data[i]];
                }
            }

            //translate back
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
                if (data[i] >= 0)
                    sb.Append(a[data[i]]);
                else
                    sb.Append(text[i]);

            return sb.ToString();
        }

        /// <summary>
        /// Makes the string readable. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="seed">initial value (like a password). if 0, text.Length will be used</param>
        /// <returns></returns>
        public string Unfuscate(string text, uint seed = 0)
        {
            if (text == null)
                return null;

            int len = text.Length;
            int[] data = new int[len];

            //translate to intermediate format
            for (int i = 0; i < len; i++)
                data[i] = a.IndexOf(text[i]);

            //resolve
            int last = seed > 0 ? (int)seed : len;
            for (int i = 0; i < len; i++)
            {
                if (data[i] >= 0)
                {
                    data[i] = Array.IndexOf<int>(box, data[i]);
                    data[i] = Math.Abs(data[i] + (a.Length - (last % a.Length))) % a.Length;
                    last += data[i];
                }
            }

            //translate back
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
                if (data[i] >= 0)
                    sb.Append(a[data[i]]);
                else
                    sb.Append(text[i]);

            return sb.ToString();
        }

        /// <summary>
        /// Static version for Shenanigans obfuscate
        /// </summary>
        /// <param name="text">text to obfuscate</param>
        /// <param name="preset">Preset for the obfuscation</param>
        /// <param name="seed">initial value (like a password). if 0, text.Length will be used</param>
        /// <param name="alphabet">If AlphabetPreset.Custom is used: alphabet has to contain all characters that should be obfuscated (once!) </param>
        /// <param name="sbox">If AlphabetPreset.Custom is used: array of length alphabet that contains the numbers [0,alphabet.Length) in a random order. No duplicates allowed, it has to be bijective.</param>
        /// <returns></returns>
        public static string ObfuscateString(string text, AlphabetPreset preset = AlphabetPreset.Text, uint seed = 0, string alphabet = null, int[] sbox = null)
        {
            Shenanigans s = preset == AlphabetPreset.Custom ? new Shenanigans(AlphabetPreset.Custom, alphabet, sbox) : new Shenanigans(preset);
            return s.Obfuscate(text, seed);
        }

        /// <summary>
        /// Static version for Shenanigans unfuscate
        /// </summary>
        /// <param name="text">text to unfuscate</param>
        /// <param name="preset">Preset for the unfuscation</param>
        /// <param name="seed">initial value (like a password). if 0, text.Length will be used</param>
        /// <param name="alphabet">If AlphabetPreset.Custom is used: alphabet has to contain all characters that should be obfuscated (once!) </param>
        /// <param name="sbox">If AlphabetPreset.Custom is used: array of length alphabet that contains the numbers [0,alphabet.Length) in a random order. No duplicates allowed, it has to be bijective.</param>
        /// <returns></returns>
        public static string UnfuscateString(string text, AlphabetPreset preset = AlphabetPreset.Text, uint seed = 0, string alphabet = null, int[] sbox = null)
        {
            Shenanigans s = preset == AlphabetPreset.Custom ? new Shenanigans(AlphabetPreset.Custom, alphabet, sbox) : new Shenanigans(preset);
            return s.Unfuscate(text, seed);
        }
    }

    /// <summary>
    /// Presets for Shenanigans. The built-in presets focus on characters found in the Extended ASCII charset (although operations are performed on unicode strings). 
    /// Use Custom if you want to obfuscate characters that aren't available in ASCII but in Unicode (such as Emoticons or Chinese characters)
    /// </summary>
    public enum AlphabetPreset
    {
        /// <summary>
        /// Includes characters [a-zA-Z0-9._-]
        /// </summary>
        POSIXFullyPortableFilenames,
        /// <summary>
        /// Includes a set of 207 printable characters allowed as NTFS file or directory name (will not destroy a valid filepath). Included are all printable chars found in the extended ASCII charset minus range [?*:|\/&gt;&lt;]
        /// </summary>
        NTFS,
        /// <summary>
        /// Includes all printable characters found in the extended ASCII charset. 
        /// </summary>
        Text,
        /// <summary>
        /// Custom sbox and alphabet
        /// </summary>
        Custom
    }
}
