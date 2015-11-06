using csutils.Cryptography;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csutils.Test.Cryptography
{
    [TestFixture]
    class ShenanigansTest
    {
        [TestCase]
        public void NTFSPreset()
        {
            Shenanigans s = new Shenanigans(AlphabetPreset.NTFS);

            Assert.AreEqual("a", s.Unfuscate(s.Obfuscate("a")));
            Assert.AreEqual("þ", s.Unfuscate(s.Obfuscate("þ")));
            Assert.AreEqual("*", s.Unfuscate(s.Obfuscate("*")));
            Assert.AreEqual("*", s.Obfuscate("*"));
            Assert.AreEqual("llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll", s.Unfuscate(s.Obfuscate("llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll")));
            Assert.AreEqual("!#$%&'()+,-.0123456789;=@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþ", s.Unfuscate(s.Obfuscate("!#$%&'()+,-.0123456789;=@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþ")));

            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc")));
            Assert.AreEqual(s.Obfuscate("abc"), s.Obfuscate("abc", 3));
            Assert.AreNotEqual(s.Obfuscate("abc", 3), s.Obfuscate("abc", 4));

            Assert.AreEqual(null, s.Unfuscate(s.Obfuscate(null)));
            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc", 0)));
        }

        [TestCase]
        public void TextPreset()
        {
            Shenanigans s = new Shenanigans(AlphabetPreset.Text);

            Assert.AreEqual("a", s.Unfuscate(s.Obfuscate("a")));
            Assert.AreEqual("þ", s.Unfuscate(s.Obfuscate("þ")));
            Assert.AreEqual("*", s.Unfuscate(s.Obfuscate("*")));
            Assert.AreNotEqual("*", s.Obfuscate("*"));
            Assert.AreEqual("llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll", s.Unfuscate(s.Obfuscate("llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll")));
            Assert.AreEqual("!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþ", s.Unfuscate(s.Obfuscate("!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþ")));

            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc")));
            Assert.AreEqual(s.Obfuscate("abc"), s.Obfuscate("abc", 3));
            Assert.AreNotEqual(s.Obfuscate("abc", 3), s.Obfuscate("abc", 4));

            Assert.AreEqual(null, s.Unfuscate(s.Obfuscate(null)));
            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc", 0)));
        }

        [TestCase]
        public void POSIXFullyPortableFilenamePreset()
        {
            Shenanigans s = new Shenanigans(AlphabetPreset.POSIXFullyPortableFilenames);

            Assert.AreEqual("a", s.Unfuscate(s.Obfuscate("a")));
            Assert.AreEqual("þ", s.Unfuscate(s.Obfuscate("þ")));
            Assert.AreEqual("*", s.Unfuscate(s.Obfuscate("*")));
            Assert.AreEqual("*", s.Obfuscate("*"));
            Assert.AreEqual("lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll", s.Unfuscate(s.Obfuscate("lllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll")));
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789._", s.Unfuscate(s.Obfuscate("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789._")));

            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc")));
            Assert.AreEqual(s.Obfuscate("abc"), s.Obfuscate("abc", 3));
            Assert.AreNotEqual(s.Obfuscate("abc", 3), s.Obfuscate("abc", 4));

            Assert.AreEqual(null, s.Unfuscate(s.Obfuscate(null)));
            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc", 0)));
        }

        [TestCase]
        public void CustomPreset()
        {
            Shenanigans s = new Shenanigans(AlphabetPreset.Custom, "abc", new int[] { 2, 0, 1 });

            Assert.AreEqual("a", s.Unfuscate(s.Obfuscate("a")));
            Assert.AreEqual("þ", s.Unfuscate(s.Obfuscate("þ")));
            Assert.AreEqual("*", s.Unfuscate(s.Obfuscate("*")));
            Assert.AreEqual("*", s.Obfuscate("*"));
            Assert.AreEqual("llll", s.Unfuscate(s.Obfuscate("llll")));
            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc")));

            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc")));
            Assert.AreEqual(s.Obfuscate("abc"), s.Obfuscate("abc", 3));
            Assert.AreNotEqual(s.Obfuscate("abc", 3), s.Obfuscate("abc", 4));

            Assert.AreEqual(null, s.Unfuscate(s.Obfuscate(null)));
            Assert.AreEqual("abc", s.Unfuscate(s.Obfuscate("abc", 0)));
        }

        [TestCase]
        public void CustomPresetNegative()
        {
            try { Shenanigans s = new Shenanigans(AlphabetPreset.Custom, null); Assert.Fail(); }
            catch { }
            try { Shenanigans s = new Shenanigans(AlphabetPreset.Custom, "", null); Assert.Fail(); }
            catch { }
            try { Shenanigans s = new Shenanigans(AlphabetPreset.Custom, "", new int[] { }); Assert.Fail(); }
            catch { }
            try { Shenanigans s = new Shenanigans(AlphabetPreset.Custom, "abc", new int[] { 1, 2 }); Assert.Fail(); }
            catch { }
            try { Shenanigans s = new Shenanigans(AlphabetPreset.Custom, "abc", new int[] { 1, 2, 3 }); Assert.Fail(); }
            catch { }
            try { Shenanigans s = new Shenanigans(AlphabetPreset.Custom, "abc", new int[] { 1, 1, 0 }); Assert.Fail(); }
            catch { }
            try { Shenanigans s = new Shenanigans(AlphabetPreset.Custom, "abc", new int[] { -1, 1, 0 }); Assert.Fail(); }
            catch { }

        }
    }
}
